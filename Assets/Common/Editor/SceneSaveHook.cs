using UnityEngine;
using UnityEditor;
using System.Collections;

public class SceneSaveHook : AssetModificationProcessor
{
    static string[] OnWillSaveAssets (string[] paths)
    {
        foreach (string path in paths) {
            if (path.EndsWith (".unity")) {
                GameObject sceneMergerGO = GameObject.Find ("Scene Merger");
                if (sceneMergerGO) {
                    SceneMerger merger = sceneMergerGO .GetComponent<SceneMerger> ();
                    merger.ClearOverlay ();
                }
            }
        }
        return paths;
    }
}
