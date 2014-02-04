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
            return;
        }

        SceneMerger merger = mergerObject.GetComponent<SceneMerger> ();

        if (GUILayout.Button ("Load")) {
            merger.ClearOverlay ();
            merger.PushSceneObjects ();
            merger.LoadScenesToOverlay ();
            merger.PopSceneObjects ();
        }
        
        if (GUILayout.Button ("Clean up")) {
            merger.ClearOverlay ();
        }
    }
}
