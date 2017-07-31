using UnityEngine;

public class EnergyManager : MonoBehaviour {
  private float _powerLeft = 1500;
  private float _powerConsumption = -0.01f;

  private State _state;

  public float PowerLeft {
    get { return _powerLeft; }
    set { _powerLeft = value; }
  }

  public float PowerConsumption {
    get {
      if (_state.Researching) {
       return  _powerConsumption - _state.ResearchCost;
      }
      return _powerConsumption;
    }
    set { _powerConsumption = value; }
  }

  public void removePower(float powerToRemove) {
    PowerLeft -= powerToRemove;
  }

  // Use this for initialization
  void Start() {
    _state = gameObject.GetComponent<State>();
  }

  // Update is called once per frame
  void Update() {
    if (_state.Researching) {
      _powerLeft -= _state.ResearchCost;
    }
    if (_powerLeft <=  0) {
      Debug.Log("you died");
      _state.Lost = true;
    }
    if (_state.FirstTimePowerEqual == false && PowerConsumption >= 0f) {
      _state.FirstTimePowerEqual = true;
    }
  }
}