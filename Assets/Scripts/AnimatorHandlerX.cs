using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimatorHandlerX : MonoBehaviour 
{
	public List<SpriteRenderer> customAnimators;

	void OnEnable ()
	{
		for (int i = 0; i < customAnimators.Count; i++) {
			customAnimators [i].enabled = (true);
		}
	}
     
}
