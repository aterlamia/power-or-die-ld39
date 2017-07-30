using UnityEngine;

public class Building {
  protected readonly string _name;
  protected readonly ResourcesManager _resourcesManager;
  public BuildType Type { get; private set; }
  public float PowerAdd { get; set; }
  public float PowerTake { get; private set; }
  public float PowerToBuild { get; private set; }
  public float PowerModPerPerson { get; private set; }

  public Building(string name, BuildType type, float powerAdd, float powerTake, float powerToBuild,
    float powerModPerPerson, ResourcesManager resourcesManager) {
    _name = name;
    _resourcesManager = resourcesManager;
    Type = type;
    PowerAdd = powerAdd;
    PowerTake = powerTake;
    PowerToBuild = powerToBuild;
    PowerModPerPerson = powerModPerPerson;
  }

  public virtual void Update() {
    
  }

  public virtual float getPowerAdd() {
    return PowerAdd;
  }

  public virtual float getPowerTake() {
    return PowerTake;
  }
}

class CityBuilding : Building {
  public CityBuilding(string name, BuildType type, float powerAdd, float powerTake, float powerToBuild, float powerModPerPerson, ResourcesManager resourcesManager) : base(name, type, powerAdd, powerTake, powerToBuild, powerModPerPerson, resourcesManager) { }
}

class CoalMineBuilding : Building {
  protected float _timeLastUpdate;

  public CoalMineBuilding(string name, BuildType type, float powerAdd, float powerTake, float powerToBuild,
    float powerModPerPerson, ResourcesManager resourcesManager) : base(name, type, powerAdd, powerTake, powerToBuild,
    powerModPerPerson, resourcesManager) {
    _timeLastUpdate = Time.time;
  }

  public override void Update() {
    if (Time.time - _timeLastUpdate >= 1) {
      _resourcesManager.Coal += 0.104f;
      _timeLastUpdate = Time.time;
    }
  }
}

class PowerBuilding : Building {
  protected float _timeLastUpdate;
  protected bool _isWorking = false;

  public PowerBuilding(string name, BuildType type, float powerAdd, float powerTake, float powerToBuild,
    float powerModPerPerson, ResourcesManager resourcesManager) : base(name, type, powerAdd, powerTake, powerToBuild,
    powerModPerPerson, resourcesManager) {
    _timeLastUpdate = Time.time;
  }

  public override void Update() {
    if (Time.time - _timeLastUpdate >= 30 || _isWorking == false) {
      _timeLastUpdate = Time.time;
      if (_resourcesManager.Coal > 1) {
        _resourcesManager.Coal -= 1;
        _isWorking = true;
      } else {
        _isWorking = false;
      }
    }
  }

  public override float getPowerAdd() {
    if (_isWorking) {
      return PowerAdd;
    }
    return 0f;
  }
}