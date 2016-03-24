using UnityEngine;
using System.Collections;

public class ChangeLevel : MonoBehaviour {

public void levelchange(string levelname)
	{
		Application.LoadLevel (levelname);
	}
}
