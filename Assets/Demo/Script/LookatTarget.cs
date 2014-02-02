using UnityEngine;
using System.Collections;

public class LookatTarget : MonoBehaviour {

	[SerializeField]
	Transform target;

	void Update () {

		transform.LookAt(target.position);
	
	}
}
