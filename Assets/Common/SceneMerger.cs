using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
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

#if UNITY_EDITOR

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

    public void LoadScenesToOverlay()
    {
        var overlay = GameObject.Find("_overlay");

        foreach  (string  name in mergeScenes)
        {
            EditorApplication.OpenSceneAdditive("Assets/Scenes/" + name + ".unity");
        }

        overlay = new GameObject("_overlay");

        foreach (GameObject  go in FindObjectsOfType (typeof(GameObject)) as GameObject[])
        {
            if (go.transform.parent == null && go.name != "_cache" && go != overlay)
            {
                go.transform.parent = overlay.transform;
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

#endif
}
