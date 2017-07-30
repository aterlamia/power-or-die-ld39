using UnityEngine;

public class State : MonoBehaviour {
  public State() {
    FirstTimePowerEqual = false;
  }

  public bool FirstTimePowerEqual { get; set; }
  public bool FirstTimePowerEqualMsgShow { get; set; }
  public bool Lost { get; set; }

  /// Dirty hacky class to hold things globbaly ewww
  // Use this for initialization
  void Start() {
    Lost = false;
  }

  // Update is called once per frame
  void Update() { }
}