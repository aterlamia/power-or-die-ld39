using System.Collections.Generic;

public class City {
  private HashSet<Building> _buildings;
  private int _nrOfResidents;
  public float PowerConsumption { get; private set; }

  public City(int nrOfResidents) {
    PowerConsumption = 0f;
    _nrOfResidents = nrOfResidents;
    _buildings = new HashSet<Building>();
  }

  public int getResidents() {
    return _nrOfResidents;
  }

  public void increaseResidentsBy(int count) {
    _nrOfResidents += count;
  }

  public void addBuilding(Building building) {
    PowerConsumption += building.PowerAdd;
    PowerConsumption -= building.PowerTake;
    _buildings.Add(building);
  }
}