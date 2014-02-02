using UnityEngine;
using System.Collections;

public class MoveTank : MonoBehaviour {

	private Vector2 input;
	public float speed = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		input.x = Input.GetAxis("Horizontal");
		input.y = Input.GetAxis("Vertical");
	}

	void FixedUpdate()
	{
		//rigidbody.MoveRotation(Quaternion.FromToRotation(Vector3.forward, new Vector3(input.x, input.y)));
		transform.Rotate(Vector3.up, input.x * speed);
		rigidbody.velocity =  transform.forward * input.y * speed;
	}
}
