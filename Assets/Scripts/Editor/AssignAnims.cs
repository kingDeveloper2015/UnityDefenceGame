using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class AssignAnims : MonoBehaviour {

	[MenuItem("Tools/AssignAnimsToParent &s")]
	public static void AssignAnimsToParent(){
		AnimatorHandler ah;
		if(Selection.activeGameObject){
			ah = Selection.activeGameObject.GetComponent<AnimatorHandler>();
			if(ah != null){
				List<Animator> list = ah.GetList();
				if(list == null){
					list = new List<Animator>();
				}else{
					
					list.Clear();
				}

				Transform parent = Selection.activeGameObject.transform;

				for (int i = 0; i < parent.childCount; i++) {
					Animator a = parent.GetChild(i).GetComponent<Animator>();
					if(a != null){
						list.Add(a);
					}
						
				}
			}
		}
	}


	[MenuItem("Tools/AssignAnimsToParent &a")]
	public static void ToggleActive(){
		if (Selection.gameObjects != null) {
			foreach (var obj in Selection.gameObjects) {
				obj.SetActive (!obj.activeSelf);
			}
		}
	}

	[MenuItem("Tools/Movement/Left %#LEFT")]
	public static void Left(){
		if (Selection.transforms != null) {
			foreach (var obj in Selection.transforms) {
				obj.Translate(Vector3.left*2);
			}
		}
	}

	[MenuItem("Tools/Movement/Right %#RIGHT")]
	public static void Right(){
		if (Selection.transforms != null) {
			foreach (var obj in Selection.transforms) {
				obj.Translate(Vector3.right*2);
			}
		}
	}

	[MenuItem("Tools/Movement/Up %#UP")]
	public static void Up(){
		if (Selection.transforms != null) {
			foreach (var obj in Selection.transforms) {
				obj.Translate(Vector3.up*2);
			}
		}
	}

	[MenuItem("Tools/Movement/Down %#DOWN")]
	public static void Down(){
		if (Selection.transforms != null) {
			foreach (var obj in Selection.transforms) {
				obj.Translate(Vector3.down*2);
			}
		}
	}
}
