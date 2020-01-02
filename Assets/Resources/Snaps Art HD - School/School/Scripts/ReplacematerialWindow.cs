using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ReplacematerialWindow : EditorWindow{
	/**
	* GENERAL VARIABLES
	**/
	// Variables
	Material ReplaceMat     = null;
	Material ReplacementMat = null;
	bool CheckChildren      = false;
	
	
	/**
	* BEHAVIOR
	**/
	// Window initialize
	[MenuItem ("Tools/Replace Material")]
	public static void showWindow () {
		ReplacematerialWindow window = (ReplacematerialWindow)EditorWindow.GetWindow(typeof(ReplacematerialWindow),false, "Replace material");
	}
	
	// Window GUI
	void OnGUI () {
		// Variables in the window
		ReplaceMat     = (Material)EditorGUILayout.ObjectField("Material to replace", ReplaceMat, typeof(Material), false);
		ReplacementMat = (Material)EditorGUILayout.ObjectField("Material replacement", ReplacementMat, typeof(Material), false);
		CheckChildren  = EditorGUILayout.Toggle("Check children", CheckChildren);
		
		// Buttons
		if(GUILayout.Button("Replace Now!!!")){
			if (ReplaceMat == null) {
				AcceptMenssage("No material to replace selected");
			}
			else if (ReplacementMat == null) {
				AcceptMenssage("No replacement material selected");
			}
			else {
				GameObject[] ActualSelection = Selection.gameObjects;
				if (ActualSelection.Length == 0) {
					AcceptMenssage("No objects selected");
				}
				else{
					if(CheckChildren == true) {
						ActualSelection = AddChildren(ActualSelection);
					}
					ReplaceNow(ActualSelection);
				}
			}
		}
	}
	
	
	/**
	* FUNTIONS USED IN THIS EDITOR WINDOW
	**/
	// Replace Material action
	void ReplaceNow(GameObject[] ObjectsSelected){
		// Needed Variables
		Renderer thisRender       = null;
		Material[] thisRenderMats = null;
		int MaterialsChanged      = 0;
		
		// Check every selected Object
		foreach (GameObject actObject in ObjectsSelected) {
			thisRender = (Renderer)actObject.GetComponent<Renderer>();
			// Check gameObjects renderer
			if(thisRender != null){
				thisRenderMats = thisRender.sharedMaterials;
				// Change materials
				for (int ind = 0; ind < thisRenderMats.Length; ind++) {
					if (thisRenderMats[ind] == ReplaceMat) {
						thisRenderMats[ind] = ReplacementMat;				
						MaterialsChanged += 1;
					}
				}
				// Undo action
				//Undo.RegisterCompleteObjectUndo(actObject, "Object materials replacement change");
				Undo.RecordObject (thisRender, "Object materials replacement change");
				// Assing material
				thisRender.sharedMaterials = thisRenderMats;
			}
		}
		
		// The function has made its work
		AcceptMenssage( MaterialsChanged.ToString() + " material" + (MaterialsChanged != 1 ? "s" : "") + " modified");
	}
	// Accept Menssage window
	void AcceptMenssage(string menssage){
        if (EditorUtility.DisplayDialog("Material Replace", menssage, "Ok")){
            //Close();
        }
	}
	// Add children
	GameObject[] AddChildren(GameObject[] ObjectsSelected){
		// Creates a dynamic array
		List<GameObject> thisChildren = new List<GameObject>();
		
		// Adds selection objects to dynamic array
		foreach (GameObject actObject in ObjectsSelected) {
			thisChildren.Add(actObject);
		}
		
		// Checks and adds all children
		CheckChild(ref thisChildren);
		
		// Creates a temporal array with both parents and children
		GameObject[] TempObjectArray = new GameObject[thisChildren.Count];
		for (int ind = 0; ind < thisChildren.Count; ind++) {
			TempObjectArray[ind] = thisChildren[ind];
		}
		
		// Returns new array
		return TempObjectArray;
	}
	void CheckChild(ref List<GameObject> thisChildren){
		int ind = 0;
		while (ind < thisChildren.Count){
			if (thisChildren[ind].transform.childCount != 0) {
				for (int indJ = 0; indJ < thisChildren[ind].transform.childCount; indJ++) {
					thisChildren.Add(thisChildren[ind].transform.GetChild(indJ).gameObject);
				}
			}
			ind++;
		}
	}
}
