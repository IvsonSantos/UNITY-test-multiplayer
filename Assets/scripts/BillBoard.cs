using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		// keep the Healthbar looking at the Main Camera
		transform.LookAt(Camera.main.transform);
	}
}
