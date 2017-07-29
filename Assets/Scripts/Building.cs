public class Building {
  private readonly string _name;
  public BuildType Type { get; private set; }
  public float PowerAdd { get; private set; }
  public float PowerTake { get; private set; }
  public float PowerToBuild { get; private set; }
  public float PowerModPerPerson { get; private set; }

  public Building(string name, BuildType type, float powerAdd, float powerTake, float powerToBuild, float powerModPerPerson) {
    _name = name;
    Type = type;
    PowerAdd = powerAdd;
    PowerTake = powerTake;
    PowerToBuild = powerToBuild;
    PowerModPerPerson = powerModPerPerson;
  }
}