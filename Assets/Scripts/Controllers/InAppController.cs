using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Purchasing;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// An example of basic Unity IAP functionality.
/// To use with your account, configure the product ids (AddProduct)
/// and Google Play key (SetPublicKey).
/// </summary>
public class InAppController : MonoBehaviour, IStoreListener
{

    public GameObject questionPopup;
    public GameObject freeLevelSkipPopup;

    // Unity IAP objects 
    private IStoreController m_Controller;
    private IAppleExtensions m_AppleExtensions;

    private int m_SelectedItemIndex = -1; // -1 == no product
    private bool m_PurchaseInProgress;

    private Selectable m_InteractableSelectable; // Optimization used for UI state management

    /// <summary>
    /// This will be called when Unity IAP has finished initialising.
    /// </summary>
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        m_Controller = controller;
        m_AppleExtensions = extensions.GetExtension<IAppleExtensions>();


        // On Apple platforms we need to handle deferred purchases caused by Apple's Ask to Buy feature.
        // On non-Apple platforms this will have no effect; OnDeferred will never be called.
        m_AppleExtensions.RegisterPurchaseDeferredListener(OnDeferred);

        Debug.Log("Available items:");
        foreach (var item in controller.products.all)
        {
            if (item.availableToPurchase)
            {
                Debug.Log(string.Join(" - ",
                    new[]
                    {
                        item.metadata.localizedTitle,
                        item.metadata.localizedDescription,
                        item.metadata.isoCurrencyCode,
                        item.metadata.localizedPrice.ToString(),
                        item.metadata.localizedPriceString
                    }));
            }
        }

        // Prepare model for purchasing
        if (m_Controller.products.all.Length > 0)
        {
            m_SelectedItemIndex = 0;
        }

    }

    /// <summary>
    /// This will be called when a purchase completes.
    /// </summary>
    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e)
    {
        Debug.Log("Purchase OK: " + e.purchasedProduct.definition.id);
        Debug.Log("Receipt: " + e.purchasedProduct.receipt);

        m_PurchaseInProgress = false;
        AnalyticsItem aItem = new AnalyticsItem();
        aItem.Desc = "In App Purchase Compleated :" + e.purchasedProduct.definition.id;
        aItem.Value = "Receipt : " + e.purchasedProduct.receipt;
        if (AnalyticsManager.instance != null)
        {
            AnalyticsManager.instance.SendItemToAnaytics(aItem);
            AnalyticsManager.instance.KeyItemBought();
        }

        LoadBestLevel();


        // Indicate we have handled this purchase, we will not be informed of it again.x
        return PurchaseProcessingResult.Complete;
    }

    void LoadBestLevel()
    {
        int bestScore = PlayerPrefs.GetInt("BestLevelScore", 0);

        if (bestScore == 0)
            bestScore = 1;

        string lvlName = "Level" + PlayerPrefs.GetInt("BestLevelScore", bestScore);
        SceneManager.LoadScene(lvlName);
    }

    /// <summary>
    /// This will be called is an attempted purchase fails.
    /// </summary>
    public void OnPurchaseFailed(Product item, PurchaseFailureReason r)
    {
        Debug.Log("Purchase failed: " + item.definition.id);
        Debug.Log(r);

        AnalyticsItem aItem = new AnalyticsItem();
        aItem.Desc = "In App Purchase Failed :" + item.definition.id;
        aItem.Value = "Reason : " + r.ToString();
        if (AnalyticsManager.instance != null)
            AnalyticsManager.instance.SendItemToAnaytics(aItem);

        m_PurchaseInProgress = false;
    }

    public void OnInitializeFailed(InitializationFailureReason error)
    {
        Debug.Log("Billing failed to initialize!");
        switch (error)
        {
            case InitializationFailureReason.AppNotKnown:
                Debug.LogError("Is your App correctly uploaded on the relevant publisher console?");
                break;
            case InitializationFailureReason.PurchasingUnavailable:
                // Ask the user if billing is disabled in device settings.
                Debug.Log("Billing disabled!");
                break;
            case InitializationFailureReason.NoProductsAvailable:
                // Developer configuration error; check product metadata.
                Debug.Log("No products available for purchase!");
                break;
        }
    }

    public void Awake()
    {
        var module = StandardPurchasingModule.Instance();
        module.useMockBillingSystem = true; // Microsoft
        // The FakeStore supports: no-ui (always succeeding), basic ui (purchase pass/fail), and 
        // developer ui (initialization, purchase, failure code setting). These correspond to 
        // the FakeStoreUIMode Enum values passed into StandardPurchasingModule.useFakeStoreUIMode.
        module.useFakeStoreUIMode = FakeStoreUIMode.Default;

        var builder = ConfigurationBuilder.Instance(module);

        builder.Configure<IGooglePlayConfiguration>().SetPublicKey("MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA2O/9/H7jYjOsLFT/uSy3ZEk5KaNg1xx60RN7yWJaoQZ7qMeLy4hsVB3IpgMXgiYFiKELkBaUEkObiPDlCxcHnWVlhnzJBvTfeCPrYNVOOSJFZrXdotp5L0iS2NVHjnllM+HA1M0W2eSNjdYzdLmZl1bxTpXa4th+dVli9lZu7B7C2ly79i/hGTmvaClzPBNyX+Rtj7Bmo336zh2lYbRdpD5glozUq+10u91PMDPH+jqhx10eyZpiapr8dFqXl5diMiobknw9CgcjxqMTVBQHK6hS0qYKPmUDONquJn280fBs1PTeA6NMG03gb9FLESKFclcuEZtvM8ZwMMRxSLA9GwIDAQAB");
        builder.AddProduct("Levelkey", ProductType.Consumable, new IDs
        {
            {"com.wiaworlds.homebound.levelkey", GooglePlay.Name},
            {"com.wiaworlds.homebound.levelkey", AppleAppStore.Name},
            {"com.wiaworlds.homebound.levelkey", MacAppStore.Name},
            {"com.wiaworlds.homebound.levelkey", WinRT.Name}
        });

        //builder.AddProduct("sword", ProductType.NonConsumable, new IDs
        //{
        //    {"com.unity3d.unityiap.unityiapdemo.sword.c", GooglePlay.Name},
        //    {"com.unity3d.unityiap.unityiapdemo.sword.6", AppleAppStore.Name},
        //    {"com.unity3d.unityiap.unityiapdemo.sword.mac", MacAppStore.Name},
        //    {"com.unity3d.unityiap.unityiapdemo.sword", WindowsPhone8.Name}
        //});
        //builder.AddProduct("subscription", ProductType.Subscription, new IDs
        //{
        //    {"com.unity3d.unityiap.unityiapdemo.subscription", GooglePlay.Name, AppleAppStore.Name}
        //});

        // Now we're ready to initialize Unity IAP.
        UnityPurchasing.Initialize(this, builder);
    }

    /// <summary>
    /// This will be called after a call to IAppleExtensions.RestoreTransactions().
    /// </summary>
    private void OnTransactionsRestored(bool success)
    {
        Debug.Log("Transactions restored.");
    }

    /// <summary>
    /// iOS Specific.
    /// This is called as part of Apple's 'Ask to buy' functionality,
    /// when a purchase is requested by a minor and referred to a parent
    /// for approval.
    /// 
    /// When the purchase is approved or rejected, the normal purchase events
    /// will fire.
    /// </summary>
    /// <param name="item">Item.</param>
    private void OnDeferred(Product item)
    {
        Debug.Log("Purchase deferred: " + item.definition.id);
    }

    public void FreeLevelSkip()
    {
        LoadBestLevel();
    }
    public void BuyLevelKeyButton()
    {
        questionPopup.GetComponent<Animator>().SetTrigger("Show");
        if(AnalyticsManager.instance != null)
            AnalyticsManager.instance.BuyKeyItemCalled();
    }

    public void ConfirmBuyLevelKeyButton()
    {
        if (m_PurchaseInProgress == true)
        {
            return;
        }

        m_Controller.InitiatePurchase(m_Controller.products.all[0]);

        // Don't need to draw our UI whilst a purchase is in progress.
        // This is not a requirement for IAP Applications but makes the demo
        // scene tidier whilst the fake purchase dialog is showing.
        m_PurchaseInProgress = true;
    }

    public void CancelBuyLevelKeyButton()
    {
        questionPopup.GetComponent<Animator>().ResetTrigger("Show");
        questionPopup.GetComponent<Animator>().SetTrigger("Hide");
        if(AnalyticsManager.instance != null)
        AnalyticsManager.instance.BuyKeyItemCanceled();
        //questionPopup.SetActive(false);
    }
    public void CancelFreeSkipButton()
    {
        freeLevelSkipPopup.GetComponent<Animator>().ResetTrigger("Show");
        freeLevelSkipPopup.GetComponent<Animator>().SetTrigger("Hide");
        //questionPopup.SetActive(false);
    }
    //public void UpdateHistoryUI()
    //{
    //    if (m_Controller == null)
    //    {
    //        return;
    //    }

    //    var itemText = "Item\n\n";
    //    var countText = "Purchased\n\n";

    //    for (int t = 0; t < m_Controller.products.all.Length; t++)
    //    {
    //        var item = m_Controller.products.all[t];

    //        // Collect history status report

    //        itemText += "\n\n" + item.definition.levelID;
    //        countText += "\n\n" + item.hasReceipt.ToString();
    //    }

    //    // Show history
    //    GetText(false).text = itemText;
    //    GetText(true).text = countText;
    //}


    /// <summary>
    /// Gets the restore button when available
    /// </summary>
    /// <returns><c>null</c> or the restore button.</returns>
    private Button GetRestoreButton()
    {
        GameObject restoreButtonGameObject = GameObject.Find("Restore");
        if (restoreButtonGameObject != null)
        {
            return restoreButtonGameObject.GetComponent<Button>();
        }
        else
        {
            return null;
        }
    }

    private Text GetText(bool right)
    {
        var which = right ? "TextR" : "TextL";
        return GameObject.Find(which).GetComponent<Text>();
    }

    public void OnDisable()
    {
        AnalyticsManager.OnFreeSkip -= GiveFreeSkip;
    }

    public void OnEnable()
    {
        AnalyticsManager.OnFreeSkip += GiveFreeSkip;
    }

    private void GiveFreeSkip()
    {
        freeLevelSkipPopup.SetActive(true);
        freeLevelSkipPopup.GetComponent<Animator>().SetTrigger("Show");
    }
}

