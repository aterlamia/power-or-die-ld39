using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyManager : MonoBehaviour {

	private float _powerLeft = 2000f;
	private float _powerConsumption = 0.02f;
	public float PowerLeft {
		get { return _powerLeft; }
		set { _powerLeft = value; }
	}

	public float PowerConsumption {
		get { return _powerConsumption; }
		set { _powerConsumption = value; }
	}

	public void removePower(float powerToRemove) {
		PowerLeft -= powerToRemove;
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {	
		
	}
	
	
}
