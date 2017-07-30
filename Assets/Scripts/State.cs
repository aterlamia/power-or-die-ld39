using System;
using UnityEngine;

public class State : MonoBehaviour {
  public State() {
    FirstTimePowerEqual = false;
    PowerPlants = 1;
  }

  public bool FirstTimePowerEqual { get; set; }
  public bool FirstTimePowerEqualMsgShow { get; set; }
  public bool Lost { get; set; }

  public bool FirstHouseBuildMsgShow { get; set; }
  public bool FirstHouseBuild { get; set; }
  public int PowerPlants { get; set; }
  public bool PowerPlantMsgShown { get; set; }

  private float _research1;
  private double _timeLastUpdateS1;
  private double _timeLastUpdateS2;
  private bool _research2Done = false;
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
      if (Time.time - _timeLastUpdateS1 > 30) {
        Researching = false;
        Research2Done = true;
      }
      if (Time.time - _timeLastUpdateS2 > 90) {
        Researching = false;
        Research2Done = true;
      }
    }
  }

  public void StartResearch1() {
    Researching = true;
    _timeLastUpdateS1 = Time.time;
    ResearchCost = 0.01f;
  }
  
  public void StartResearch2() {
    Researching = true;
    _timeLastUpdateS2 = Time.time;
    ResearchCost = 0.02f;
  }
}