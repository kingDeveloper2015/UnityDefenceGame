using UnityEngine;
using System.Collections;

public class TrialVersionController : MonoBehaviour {

    public bool isTrial = false;
    //
	// Use this for initialization
	void Awake () {
	    if(isTrial)
        {
            Destroy(gameObject);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
