using UnityEngine;
using System.Collections;

public class CustomAnimator : MonoBehaviour {

	public Sprite[] frames;
	public float frameDelay;
	public bool disableAfterAnimation;
	public SpriteRenderer rend;
	public GameObject g;

	IEnumerator Animate () {
//		MaterialPropertyBlock block = new MaterialPropertyBlock();
//		int id = Shader.PropertyToID ("_MainTex");
		var delay = new WaitForSeconds (frameDelay);
//		rend.GetPropertyBlock (block);
		if(frames != null)
		for (int i = 0; i < frames.Length; i++) {
//				block.SetTexture (id, frames [i].texture);
				rend.sprite = frames [i];
				yield return delay;
		}
		yield return 0;

		if (disableAfterAnimation) {
//			g.SetActive (false);
			rend.enabled = false;
		}
		
	}

	void OnEnable(){
		StartCoroutine (Animate ());
	}
}
