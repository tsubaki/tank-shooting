using UnityEngine;
using System.Collections;

public class SpawnVehicle : MonoBehaviour
{
	public EE_Stage stage;

	private float nextSpawnTime = 0;
	private int currentRaw = 0;

	[SerializeField]
	GameObject[]
		prefabs;

	private ObjectPool[] pools;

	void Start ()
	{
		pools = new ObjectPool[prefabs.Length];
		for (int i=0; i< prefabs.Length; i++) {
			pools [i] = ObjectPool.GetObjectPool (prefabs [i]);
		}
	}


	void OnEnable ()
	{
		currentRaw = 0;
		nextSpawnTime = 0;

		if (stage != null) {

		} else {
			enabled = false;
			Debug.LogError ("stage is null!");
			return;
		}
		
	}


	void Update ()
	{
		if (nextSpawnTime < Time.time) {
			var stageParam = stage.param [currentRaw];
			Spawn (stageParam.pos);
			nextSpawnTime = Time.time + stageParam.Time;
			currentRaw ++;
		}
		if (currentRaw >= stage.param.Count) {
			//enabled = false;
			currentRaw = 0;
		}
	}

	void Spawn (int[] pos)
	{
		for (int i=0; i<pos.Length; i++) {
			if (pos [i] == 0)
				continue;
			var vehicleObj = pools [pos [i] - 1].GetInstance ();
			var location = SpawnPositions.Instance.positions [i];
			vehicleObj.transform.position = location.position;
			vehicleObj.transform.rotation = location.rotation;
		}
	}
}
