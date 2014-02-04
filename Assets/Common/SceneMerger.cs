using UnityEngine;
using System.Collections;

public class SceneMerger : MonoBehaviour
{
    public string[] mergeScenes;
    private static bool semaphore ;

    void Awake ()
    {
        if (semaphore)
        {
            return;
        }
        semaphore = true;

        ClearOverlay ();

        foreach (string  scene  in mergeScenes)
        {
            Application.LoadLevelAdditive (scene);
        }
    }

    public void ClearOverlay ()
    {
        var overlay = GameObject.Find ("_overlay");
        if (overlay)
            DestroyImmediate (overlay);
    }
}
