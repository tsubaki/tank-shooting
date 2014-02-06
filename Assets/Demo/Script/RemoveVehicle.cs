using UnityEngine;
using System.Collections;

public class RemoveVehicle : MonoBehaviour
{
	void OnTriggerEnter (Collider c)
	{
		c.attachedRigidbody.gameObject.SetActive (false);
	}
}
