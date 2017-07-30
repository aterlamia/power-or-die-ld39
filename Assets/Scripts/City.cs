using System;
using System.Collections.Generic;
using UnityEngine;

public class City {
  private HashSet<Building> _buildings;
  private int _nrOfResidents;
  public float PowerConsumption { get; private set; }
  private int _housesCount = 0;

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
  }

  public void addBuilding(Building building) {
    _buildings.Add(building);
    if (building.Type == BuildType.House) {
      _housesCount++;
    }
  }
}