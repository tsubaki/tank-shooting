using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {

	[SerializeField]
	Transform target;

	void FixedUpdate () {
	
		transform.position = Vector3.Lerp(transform.position, target.position, 0.02f);

	}
}
