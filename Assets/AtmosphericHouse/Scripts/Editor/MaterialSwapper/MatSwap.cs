using UnityEngine;
using UnityEditor;

public class MatSwapper : EditorWindow
{

    //*****  (©) Finward Studios 2021. All rights reserved. *****\\

    static string[] materialPaths = { "Assets/AtmosphericHouse/Materials/", "Assets/AtmosphericHouse/Materials/Materials_tiled/", "Assets/AtmosphericHouse/Materials/Materials_additional/" };
    public GameObject 	gameObject;
	public string 		originalString 	  = "_clean";
	public string 		replacementString = "_worn";
	public bool 		isRecursive = true;

	private bool 		_simpleMode = true;
	
	[MenuItem("Tools/Finward Studios/Material Swapper")]
	public static void OpenCustomWindow()																	// Window init
	{
		var window 			= GetWindow(typeof(MatSwapper));
		var title 			= new GUIContent();
		title.text 			= "Material Swapper";
		window.titleContent = title;
	}

	private void OnGUI()																					// Window Main loop
	{
        // ***Would assign selected game object automatically***
		//gameObject			= Selection.activeGameObject;
		gameObject 			= (GameObject)EditorGUILayout.ObjectField("GameObject",gameObject,typeof(GameObject),true);
					
        isRecursive = EditorGUILayout.Toggle("Include Children", isRecursive);

        EditorGUILayout.Separator();
        EditorGUILayout.Separator();

        if (GUILayout.Button("Mode Toggle")){																// Mode Button
			_simpleMode = !_simpleMode;
			if (_simpleMode){																				// Sets Default Strings
				originalString 	  = "_clean";
				replacementString = "_worn";
			}
		}
					
		EditorGUILayout.Separator();																		// GUI Seperator
		
		//																									// Mode Switch
		switch (_simpleMode)
		{
			case true:																						// Simple Mode
				EditorGUILayout.LabelField("Mode","Simple");
				EditorGUILayout.LabelField("Current Material",originalString);
				EditorGUILayout.LabelField("Assigned Material",replacementString);
			break;
			case false:																						// Normal Mode
				EditorGUILayout.LabelField("Mode","Normal");		
				originalString 		= EditorGUILayout.TextField("Current Material",originalString);
				replacementString	= EditorGUILayout.TextField("Assigned Material",replacementString); 
			break;
		}
		
		//isRecursive = EditorGUILayout.Toggle("Include Children",isRecursive);
		
		EditorGUILayout.Separator();																		// GUI Seperator
		
		if (GUILayout.Button("Swap Materials"))																// Swap Button
		{
			if (gameObject == null){																		// Checks for Null GameObject
				Debug.LogError("Cannot switch materials without assigning gameobject.");
				return;																						// Exits Function
			}
			
			if(isRecursive){																				// Is Recursive Loop
				foreach (Transform T  in gameObject.GetComponentsInChildren<Transform>())					// Foeach Loop
				{
					if (T.gameObject.TryGetComponent(typeof(MeshRenderer),out Component mR))				// Checks for Renderer
					{	Swap((MeshRenderer)mR ,originalString,replacementString);}				            // Calls Regular Swap
					
					else if(T.gameObject.TryGetComponent(typeof(SkinnedMeshRenderer),out Component sMR))	// Checks for Skinned Renderer
					{	Swap((SkinnedMeshRenderer)sMR,originalString,replacementString);}		            // Calls Skinned Swap

					else {/*Debug.LogError("Object without mesh renderer");*/}
				}
			}
		
			if(!isRecursive){																				// Isn't Recursive Loop
				if (gameObject.TryGetComponent(typeof(MeshRenderer),out Component mR))						// Checks for Renderer
				{	Swap( (MeshRenderer)mR ,originalString,replacementString);}			                 	// Calls Regular Swap 
				
				else if(gameObject.TryGetComponent(typeof(SkinnedMeshRenderer),out Component sMR))			// Checks for Skinned Renderer
				{	Swap((SkinnedMeshRenderer)sMR,originalString,replacementString);}		            	// Call Skinned Swap

				else {/*Debug.LogError("Object without mesh renderer");*/}
			}
		}
		
		if (GUILayout.Button("Reverse Current/Assigned Strings")){											// Reverse Button
			string temp 		= originalString;
			originalString 		= replacementString;
			replacementString 	= temp;
		}
		EditorGUILayout.Separator();
	}

	// Swap for Standard Mesh Renderer
	public static void Swap(MeshRenderer meshRenderer, string originalString, string replacementString)
	{
		Material[] M = meshRenderer.sharedMaterials; 														// Creates Materials Array and Assigns MeshRenderer Materials to it
		for (int i=0;i<meshRenderer.sharedMaterials.Length;i++)												// Steps Through Each Material of Materials
		{
			Material currentMaterial = meshRenderer.sharedMaterials[i];
			string oldString = currentMaterial.name;														// Gets material name							
			if (oldString.Contains(originalString))															// Validity Checks
			{
				string newString = oldString.Replace(originalString,replacementString);						// Manipulates material name
				//Material newMaterial = Resources.Load<Material>(materialPath + newString);                  // Writes new material to array
				Material newMaterial = FindMaterial(newString);
				if (newMaterial != null) {M[i] = newMaterial;} 												// Null Mat Check
				else{
					Debug.LogError("Failed to get new material, using old.\n"+newString);		            // Throws Null Error
					M[i] = currentMaterial;																	// Sets New Mat o Current Mat
				}
			}
		}
		Undo.RecordObject(meshRenderer,"Material Change");													// Records material change
		meshRenderer.sharedMaterials = M;																	// Writes new array to mesh renderer
	}

	// Swap for Skinned Mesh Renderer
	public static void Swap(SkinnedMeshRenderer meshRenderer, string originalString, string replacementString)
	{
		Material[] M = meshRenderer.sharedMaterials; 														// Creates Materials Array and Assigns MeshRenderer Materials to it
		for (int i=0;i<meshRenderer.sharedMaterials.Length;i++)												// Steps Through Each Material of Materials
		{
			Material currentMaterial = meshRenderer.sharedMaterials[i];
			string oldString = currentMaterial.name;														// Gets Material Name
			if (oldString.Contains(originalString))															// Validity Checks
			{
				string newString = oldString.Replace(originalString,replacementString);						// Manipulates Material Name
				//Material newMaterial = Resources.Load<Material>(materialPath + newString);                  // Writes New Material to Array
				Material newMaterial = FindMaterial(newString);
				if (newMaterial != null) {M[i] = newMaterial;} 												// Null Mat Check
				else
				{
					Debug.LogError("Failed to get new material, using old.\n"+newString);		            // Throws Null Error
					M[i] = currentMaterial;																	// Sets New Mat to Current Mat
				}
			}
		}
		Undo.RecordObject(meshRenderer,"Material Change");													// Records Material Change
		meshRenderer.sharedMaterials = M;																	// Writes New Array to MeshRenderer			
	}

	public static Material FindMaterial(string materialPath)
	{
		foreach (string path in materialPaths)
		{
			if ((Material)AssetDatabase.LoadMainAssetAtPath(path + materialPath + ".mat") != null)
				return (Material)AssetDatabase.LoadMainAssetAtPath(path + materialPath + ".mat");
		}

		return null;
	}
}

//*****  (©) Finward Studios 2021. All rights reserved. *****\\