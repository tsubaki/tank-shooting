using UnityEngine;
using System.Collections;

public class RemoveVehicle : MonoBehaviour
{
	void OnTriggerEnter (Collider c)
	{
		c.gameObject.SetActive (false);
	}
}
