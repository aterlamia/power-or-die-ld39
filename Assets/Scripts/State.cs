using System;
using UnityEngine;
using UnityEngine.UI;

public class State : MonoBehaviour {
  public GameObject PowerBuilding;
  public GameObject House;
  public GameObject SciButton;

  public State() {
    FirstTimePowerEqual = false;
    PowerPlants = 1;
  }

  public bool FirstTimePowerEqual {
    get { return _firstTimePowerEqual; }
    set {
      _firstTimePowerEqual = value;
      if (_firstTimePowerEqual) {
        House.tag = "Untagged";
        House.GetComponent<SpriteRenderer>().color = Color.white;
        if (House.GetComponent<SpriteRenderer>().enabled) {
          House.GetComponent<BoxCollider2D>().enabled = true;
        }
      }
    }
  }

  public bool FirstTimePowerEqualMsgShow { get; set; }
  public bool Lost { get; set; }

  public bool FirstHouseBuildMsgShow { get; set; }

  public bool FirstHouseBuild {
    get { return _firstHouseBuild; }
    set {
      _firstHouseBuild = value;
      if (_firstHouseBuild) {
        PowerBuilding.tag = "Untagged";
        PowerBuilding.GetComponent<SpriteRenderer>().color = Color.white;
        if (PowerBuilding.GetComponent<SpriteRenderer>().enabled) {
          PowerBuilding.GetComponent<BoxCollider2D>().enabled = true;
        }
      }
    }
  }

  public int PowerPlants {
    get { return _powerPlants; }
    set {
      _powerPlants = value;
      if (_powerPlants >= 2) {
        SciButton.tag = "Untagged";
        SciButton.GetComponent<SpriteRenderer>().color = Color.white;
        if (SciButton.GetComponent<SpriteRenderer>().enabled) {
          SciButton.GetComponent<BoxCollider2D>().enabled = true;
        }
      }
    }
  }

  public bool PowerPlantMsgShown { get; set; }

  private float _research1;
  private double _timeLastUpdateS1;
  private double _timeLastUpdateS2;
  private bool _research2Done = false;
  private bool _firstTimePowerEqual;
  private bool _firstHouseBuild;
  private int _powerPlants;
  public float ResearchCost { get; private set; }
  public bool Researching { get; private set; }

  public bool Research2Done {
    get { return _research2Done; }
    set { _research2Done = value; }
  }

  /// Dirty hacky class to hold things globbaly ewww
  // Use this for initialization
  void Start() {
    Lost = false;
  }

  // Update is called once per frame
  void Update() {
    if (Researching) {
      if (Time.time - _timeLastUpdateS2 > 90) {
        Researching = false;
        Research2Done = true;
      }
    }
  }

  public void StartResearch2() {
    Researching = true;
    _timeLastUpdateS2 = Time.time;
    ResearchCost = 0.02f;
  }
}