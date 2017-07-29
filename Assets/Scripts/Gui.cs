using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gui : MonoBehaviour {
  private EnergyManager _manager;
  private Sprite[] _buttons;
  private GameObject _buildingObj;

  // Use this for initialization
  void Start() {
    _manager = gameObject.GetComponent<EnergyManager>();
  }

  // Update is called once per frame`
  void Update() {
    _manager.PowerLeft = _manager.PowerLeft + _manager.PowerConsumption;
  }

  void OnGUI() {
    int TextWidth = 45;

    GUI.Label(new Rect(10, 10, TextWidth, 22), ((int) _manager.PowerLeft).ToString());
  }
}