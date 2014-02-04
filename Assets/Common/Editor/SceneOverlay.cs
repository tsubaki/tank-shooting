using UnityEngine;
using UnityEditor;
using System.Collections;

public class SceneOverlay : EditorWindow
{
    [MenuItem ("Window/Scene Overlay")]

    static void Init ()
    {
        EditorWindow.GetWindow <SceneOverlay>();
    }
    
    void OnGUI ()
    {
        GameObject mergerObject = GameObject.Find ("Scene Merger");
        if (!mergerObject) {
			Debug.LogError("'Scene Merger' is not found");
            return;
        }

        SceneMerger merger = mergerObject.GetComponent<SceneMerger> ();

        if (GUILayout.Button ("Load")) {
            ClearOverlay ();
			PushSceneObjects();
			LoadScenesToOverlay (merger.mergeScenes);
			PopSceneObjects();
        }
        
        if (GUILayout.Button ("Clean up")) {
            ClearOverlay ();
        }
    }

	public static void ClearOverlay ()
	{
		var overlay = GameObject.Find ("_overlay");
		if (overlay)
			DestroyImmediate (overlay);
	}
	
	public static void LoadScenesToOverlay(string[] mergeScenes)
	{
		var overlay = GameObject.Find("_overlay");
		foreach  (string  name in mergeScenes)
		{
			EditorApplication.OpenSceneAdditive("Assets/Scenes/" + name + ".unity");
		}

		overlay = new GameObject("_overlay");
		overlay.hideFlags |= HideFlags.NotEditable;
		foreach (GameObject  go in FindObjectsOfType (typeof(GameObject)) as GameObject[])
		{
			if (go.transform.parent == null && go.name != "_cache" && go != overlay)
			{
				go.transform.parent = overlay.transform;
			}
		}

		foreach(var comp in overlay.transform.GetComponentsInChildren<Transform>())
		{
			comp.gameObject.hideFlags = HideFlags.NotEditable;
		}
	}

	public void PushSceneObjects()
	{
		var cache = new GameObject("_cache");
		
		foreach (GameObject go  in FindObjectsOfType(typeof(GameObject )) as GameObject [])
		{
			
			if (go.transform.parent == null)
			{
				go.transform.parent = cache.transform;
			}
		}
	}

	public void PopSceneObjects()
	{
		var cache = GameObject.Find("_cache");
		
		ArrayList children = new ArrayList();
		
		foreach (Transform  child in cache.transform)
		{
			children.Add(child);
		}
		
		foreach (Transform child in children)
		{
			child.parent = null;
		}
		
		DestroyImmediate(cache);
	}
}
