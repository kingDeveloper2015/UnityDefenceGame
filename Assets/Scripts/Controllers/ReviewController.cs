using UnityEngine;
using System.Collections;

public class ReviewController : MonoBehaviour {

    Animator anim;
    void Awake()
    {
        anim = GetComponent<Animator>();
    }
	// Use this for initialization
	void Start () {
	
	}
	
    public void ShowReview()
    {
        anim.SetTrigger("Show");
    }

    public void CloseReview()
    {
        anim.SetTrigger("Hide");
        anim.ResetTrigger("Show");
    }

    public void OpenReivewPage()
    {
        Application.OpenURL("www.apple.com");
    }
	// Update is called once per frame
	void Update () {
	
	}
}
