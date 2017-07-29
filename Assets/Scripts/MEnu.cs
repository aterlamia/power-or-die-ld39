using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MEnu : MonoBehaviour {
	// Update is called once per frame
	void Update () {
		var pos = Camera.main.ViewportToWorldPoint(new Vector3(0.1f, 0.90f, 0.005f));
		pos.z = -5f;
		transform.position = pos;
	}
	
	
	void OnGUI() {
		var pos = Camera.main.ViewportToWorldPoint(new Vector3(0.1f, 0.90f, 0.005f));
		pos.z = -5f;
		transform.position = pos;
	}
}
