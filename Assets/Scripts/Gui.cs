using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gui : MonoBehaviour {
  private EnergyManager _manager;
  private Sprite[] _buttons;
  private GameObject _buildingObj;
  private ResourcesManager _resourcesManager;
  private GUIStyle _guiStyleNegative;
  private GUIStyle _guiStylePositive;

  private City _city;
  // Use this for initialization
  void Start() {
    _manager = gameObject.GetComponent<EnergyManager>();
    _resourcesManager = gameObject.GetComponent<MapCreator>().ResourceManager;
    _guiStyleNegative = new GUIStyle {normal = {textColor = Color.red}};
    _guiStylePositive = new GUIStyle {normal = {textColor = Color.green}};

    _city = GameObject.Find("Scene").GetComponent<MapCreator>().City1;
  }

  // Update is called once per frame`
  void Update() { }

  void OnGUI() {
    int TextWidth = 100;

    GUI.Label(new Rect(10, 10, TextWidth, 22), "Power Left :" + ((int) _manager.PowerLeft).ToString());

    if (_manager.PowerConsumption > 0) {
      GUI.Label(new Rect(10, 30, TextWidth, 22), "Power Gain :" + (Round(_manager.PowerConsumption,2)).ToString(),
        _guiStylePositive);
    } else {
      GUI.Label(new Rect(10, 30, TextWidth, 22), "Power Drain :" + (Round(_manager.PowerConsumption,2)).ToString(),
        _guiStyleNegative);
    }

    
    if (_resourcesManager != null) {
      if (_resourcesManager.Coal <= 0) {
        GUI.Label(new Rect(10, 50, TextWidth, 22), "Coal :" + (Round(_resourcesManager.Coal, 2)).ToString(),
          _guiStyleNegative);
      } else {
        GUI.Label(new Rect(10, 50, TextWidth, 22), "Coal :" + (Round(_resourcesManager.Coal, 2)).ToString());
      }
    }
    
    GUI.Label(new Rect(10, 70, TextWidth, 22), "Colonists :" + (_city.getResidents()).ToString());
  }
  
  public float Round(float value, int digits)
  {
    float mult = Mathf.Pow(10.0f, (float)digits);
    return Mathf.Round(value * mult) / mult;
  }
  
  
}