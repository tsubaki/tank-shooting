using UnityEngine;
using System.Collections;

public class MoveAgent : MonoBehaviour {

	private Transform target;
	private NavMeshAgent agent;

	void Start () {
		var t = GameObject.FindWithTag("Player");
		target = t.transform;
		agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		agent.SetDestination(target.position);
	}
}
