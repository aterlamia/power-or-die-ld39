using System;
using System.Collections.Generic;
using UnityEngine;

public class City {
  private HashSet<Building> _buildings;
  private int _nrOfResidents;
  public float PowerConsumption { get; private set; }
  private int _housesCount = 0;
  private double _timeLastUpdate;

  public City(int nrOfResidents) {
    PowerConsumption = 0f;
    _nrOfResidents = nrOfResidents;
    _buildings = new HashSet<Building>();
  }

  public int getResidents() {
    return _nrOfResidents;
  }

  public void addResidents(int residents) {
    _nrOfResidents += residents;
  }

  public void increaseResidentsBy(int count) {
    _nrOfResidents += count;
  }

  public void update() {
    PowerConsumption = 0;
    foreach (Building building in _buildings) {
      building.Update();
      PowerConsumption += building.getPowerAdd();

      if (building.Type == BuildType.CityCenter) {
        PowerConsumption -= (building.PowerModPerPerson * Math.Max(0, _nrOfResidents - (14 * _housesCount)));
      }
      PowerConsumption -= building.PowerTake;
    }  
    
    if (Time.time - _timeLastUpdate >= 120 && _nrOfResidents >= 10) {
      _nrOfResidents += 2;
      _timeLastUpdate = Time.time;
    }
  }

  public void addBuilding(Building building) {
    
    _buildings.Add(building);
    if (building.Type == BuildType.House) {
      _housesCount++;
    }
  }
}