using UnityEngine;
using UnityEditor;
using System.Collections;

public class SceneSaveHook : UnityEditor.AssetModificationProcessor
{

    static string[] OnWillSaveAssets (string[] paths)
    {

        foreach (string path in paths) {
            if (path.EndsWith (".unity")) {
				SceneOverlay.ClearOverlay();
            }
        }
        return paths;
    }
}
