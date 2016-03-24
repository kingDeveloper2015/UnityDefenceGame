using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Level15 : MonoBehaviour {
	[SerializeField]
	float initialDelay = 1f, delayInRound, destroyTime;
	[SerializeField]
	List<float> delayInExplosions;
	int currentActive = 0;
	[SerializeField]
	List<GameObject> exploders;

	void Start()
	{
		StartCoroutine(Explosion());
	}

	IEnumerator Explosion()
	{
		yield return new WaitForSeconds(initialDelay);

		while (!GameController.Instance.gameOver)
		{
			for (int i = 0; i < exploders.Count; i++)
			{
				exploders[i].SetActive(true);
				StartCoroutine(Deactivate(exploders[i]));
				yield return new WaitForSeconds(delayInExplosions[i]);

			}
			yield return new WaitForSeconds(delayInRound);
		}
	}

	IEnumerator Deactivate(GameObject _gameObject)
	{
		yield return new WaitForSeconds(destroyTime);
		_gameObject.SetActive(false);
	}
}
