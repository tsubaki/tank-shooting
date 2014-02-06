using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour {

	[SerializeField]
	Light light;

	[SerializeField]
	public float intervalFire, intervalLight;

	private float currentFire, currentLight;

	public LayerMask mask;

	// Update is called once per frame
	void Update () {
		float time = Time.time;
		if( Input.GetButton("Fire1") &&  time > currentFire)
		{
			currentFire = time + intervalFire;
			currentLight = time + intervalLight;
			light.enabled = true;
			light.intensity = 8;

			RaycastHit hit;
			if( Physics.Raycast(transform.position + Vector3.up, transform.forward, out hit, 50, mask))
			{
				if( hit.collider.CompareTag("Enemy") )
					hit.collider.attachedRigidbody.gameObject.SetActive(false);
			}
		}

		if( time > currentLight )
		{
			light.enabled = false;
		}else{
			light.intensity /= 1.5f;
		}
	}

	void OnDrawGizmos() {
		Gizmos.DrawRay(transform.position + Vector3.up , transform.forward * 50);
	}

}
