using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPool : MonoBehaviour
{	
	public GameObject Prefub;
	public int maxCount = 100;
	public int preperCount = 50;

	public int interval = 1;

	private List<GameObject> pooledObjectList = new List<GameObject> ();
	private static GameObject poolAttachedObject = null;

	void OnEnable ()
	{
		if (interval > 0)
			StartCoroutine (RemoveObjectCheck ());
	}

	void OnDisable ()
	{
		StopAllCoroutines ();
	}

	
	public void OnDestroy ()
	{
		if (poolAttachedObject.GetComponents<ObjectPool> ().Length == 1) {
			poolAttachedObject = null;
		}
		
		foreach (var obj in pooledObjectList) {
			Destroy (obj);
		}
		pooledObjectList.Clear ();
	}

	public GameObject GetInstance ()
	{
		foreach (GameObject obj in pooledObjectList) {
			if (obj.activeSelf == false) {
				obj.SetActive (true);
				return obj;	
			}
		}

		if (pooledObjectList.Count < maxCount) {
			GameObject obj = (GameObject)GameObject.Instantiate (Prefub);
			obj.SetActive (true);
			obj.transform.parent = transform;
			pooledObjectList.Add (obj);
			return obj;
		}

		return null;
	}

	IEnumerator RemoveObjectCheck ()
	{
		while (true) {
			RemoveObject (preperCount);
			yield return new WaitForSeconds (interval);
		}
	}

	public void RemoveObject (int max)
	{
		if (pooledObjectList.Count > max) {
			int needRemoveCount = pooledObjectList.Count - max;
			
			foreach (GameObject obj in pooledObjectList.ToArray()) {
				if (needRemoveCount == 0) {
					break;
				}
				if (obj.activeSelf == false) {
					pooledObjectList.Remove (obj);
					Destroy (obj);
					needRemoveCount --;
				}
			}
		}
	}

	public static ObjectPool GetObjectPool (GameObject obj)
	{
		if (poolAttachedObject == null) {
			poolAttachedObject = GameObject.Find ("ObjectPool");
			if (poolAttachedObject == null) {
				poolAttachedObject = new GameObject ("ObjectPool");
			}
		}

		foreach (var pool in poolAttachedObject.GetComponents<ObjectPool>()) {
			if (pool.Prefub == obj) {
				return pool;
			}
		}

		var newPool = poolAttachedObject.AddComponent<ObjectPool> ();
		newPool.Prefub = obj;
		return newPool;
	}
}