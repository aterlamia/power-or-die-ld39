using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScienceDIalog : MonoBehaviour {
	public Button Btn1;
	public Button Btn2;
	public Canvas CanvasObject;

	private State _state;

	// Use this for initialization
	void Start () {
		_state = GameObject.Find("Scene").GetComponent<State>();

		Button btn = Btn1.GetComponent<Button>();
		btn.onClick.AddListener(CloseWin);

		Button btn2 = Btn2.GetComponent<Button>();
		btn2.onClick.AddListener(StartS2);
	}

	private void CloseWin() {
  	CanvasObject.enabled = false;
	}

	void StartS2() {
		_state.StartResearch2();

		CanvasObject.enabled = false;
		Btn2.GetComponent<Button>().interactable = false;
	}		

	void StartS1() {
		_state.StartResearch1();
		CanvasObject.enabled = false;	
		Btn1.GetComponent<Button>().interactable = false;
	}

	// Update is called once per frame
	void Update () {
		
	}
}
