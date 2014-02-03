using UnityEngine;
using System.Collections;

public class MoveVehicle : MonoBehaviour
{
	[SerializeField]
	float
		speed = 2;
	void FixedUpdate ()
	{
		rigidbody.velocity = transform.forward * speed;
	}
}
