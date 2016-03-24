using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class ButtonEvents : MonoBehaviour
{ 	
	public Image fade;


	public void Restart () {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}

	public void LoadLevel(string s){
		fade.DOColor (new Color (0, 0, 0, 1), 0.25f).OnComplete (()=>WaitAndLoadLevel(s));
	}

	private void WaitAndLoadLevel(string s){
        SceneManager.LoadScene(s);
	}

	public void Start(){
		fade.DOColor (new Color (0, 0, 0, 0), 0.25f);
	}

    public void OnButtonNextLevel() {
        Scene aux = SceneManager.GetActiveScene();
		int totalScene = int.Parse(Resources.Load<TextAsset> ("TotalLevel").text );
		if (aux.buildIndex < totalScene) {
            SceneManager.LoadScene(aux.buildIndex + 1);
        } else {
            SceneManager.LoadScene(0);
        }
        
    }
}		
