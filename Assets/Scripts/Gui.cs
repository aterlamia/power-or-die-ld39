using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gui : MonoBehaviour {
  private EnergyManager _manager;
  private Sprite[] _buttons;
  private GameObject _buildingObj;

  private GUIStyle _guiStyle;

  // Use this for initialization
  void Start() {
    _manager = gameObject.GetComponent<EnergyManager>();
    _guiStyle = new GUIStyle {normal = {textColor = Color.red}};
  }

  // Update is called once per frame`
  void Update() {
    
  }

  void OnGUI() {
    int TextWidth = 100;

    GUI.Label(new Rect(10, 10, TextWidth, 22), "Power Left :" + ((int) _manager.PowerLeft).ToString());

    GUI.Label(new Rect(10, 30, TextWidth, 22), "Power Drain :" + (_manager.PowerConsumption).ToString(), _guiStyle);
  }
}