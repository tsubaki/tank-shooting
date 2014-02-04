using UnityEngine;
using System.Collections;

public class MoveTank : MonoBehaviour
{
	private Vector2 input;
	public float speed = 1;

	void Update ()
	{
		input.x = Input.GetAxis ("Horizontal");
		input.y = Input.GetAxis ("Vertical");
	}

	void FixedUpdate ()
	{
		transform.Rotate (Vector3.up, input.x * speed);
		rigidbody.velocity = transform.forward * input.y * speed;
	}
}
