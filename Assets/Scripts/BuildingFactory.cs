public class BuildingFactory {
  public static Building createBuilding(BuildType buildType, ResourcesManager resourcesManager) {
    Building building;
    switch (buildType) {
      case BuildType.ShieldGenerator:
        building = new Building("Shield1", BuildType.ShieldGenerator, 0f, 0.15f, 100f, 0, resourcesManager);
        break;
      case BuildType.PowerPlant:
        building = new PowerBuilding("Plant1", BuildType.PowerPlant, 0.06f, 0f, 200f, 0, resourcesManager);
        break;
      case BuildType.Mine:
        building = new CoalMineBuilding("Mine1", BuildType.Mine, 0f, 0.04f, 200f, 0, resourcesManager);
        break;
      case BuildType.Storage:
        building = new Building("Storage", BuildType.Storage, 0f, 0.02f, 100f, 0, resourcesManager);
        break;
      case BuildType.House:
        building = new Building("House", BuildType.House, 0f, 0.06f, 150f, 0, resourcesManager);
        break;
      case BuildType.CityCenter:
      default:
        building = new Building("CityHall", BuildType.CityCenter, 0f, 0.02f, 100f, 0.1f, resourcesManager);
        break;
    }

    return building;
  }
}