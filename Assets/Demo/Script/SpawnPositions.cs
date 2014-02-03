using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnPositions : SingletonMonoBehaviour<SpawnPositions>
{
	public List<Transform> positions = new List<Transform> ();

	[ContextMenu("register object")]
	void Register ()
	{
		positions.Clear ();
		positions.AddRange (GetComponentsInChildren<Transform> ());
		positions.Remove (transform);
	}

	public Transform GetPosition (string name)
	{
		return positions.Find ((obj) => obj.name.Equals (name));
	}

	void OnDrawGizmos ()
	{
		foreach (var obj in positions) {
			Gizmos.DrawLine (obj.transform.position, obj.transform.forward + obj.transform.position);
		}
	}
}
