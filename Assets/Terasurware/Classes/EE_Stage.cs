using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EE_Stage : ScriptableObject
{	
	public List<Param> param = new List<Param> ();

	[System.SerializableAttribute]
	public class Param
	{
		
		public float Time;
		public int[] pos;
	}
}