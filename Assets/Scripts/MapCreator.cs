using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MapCreator : MonoBehaviour {
  private int _width = 100;
  private int _height = 100;
  private Tile[,] _map;
  private bool[,] _visibleMap;
  private int[,] _buildings;
  private int[,] _shield;

  public City City1 { get; private set; }
  public EnergyManager EnergyManager;

  public GameObject TilePrefab;
  public GameObject ShieldPrefab;
  public GameObject CityPrefab;

  public string Seed;
  private float _sizeX;
  private float _sizeY;
  private float _widthOffset;
  public Vector2 StartPos;

  private Dictionary<BuildType, Sprite> _sprites;
  private Dictionary<Building, GameObject> _buildingToGameObjects;
  private Dictionary<GameObject, Building> _gameObjectToBuilding;

  public Sprite[] BaseTiles;

  // Use this for initialization
  void Start() {
    _sizeX = (float) Math.Sqrt(3f) / 2f * 2f;
    _sizeY = 2f;
    _widthOffset = 2f * (3f / 4f);
    _buildingToGameObjects = new Dictionary<Building, GameObject>();
    _gameObjectToBuilding = new Dictionary<GameObject, Building>();
    _sprites = new Dictionary<BuildType, Sprite>();

    InitSprites();
    InitMap();
    DrawMap();
  }

  private void InitSprites() {
    _sprites[BuildType.PowerPlant] = Resources.Load<Sprite>("Power");
    _sprites[BuildType.CityCenter] = Resources.Load<Sprite>("City");
    _sprites[BuildType.Mine] = Resources.Load<Sprite>("Coal");
  }

  // Update is called once per frame
  void Update() {
    City1.update();
    EnergyManager.PowerConsumption = City1.PowerConsumption;
  }

  private void InitMap() {
    var pseudoRandom = new System.Random(Seed.GetHashCode());
    City1 = new City(10);
    _map = new Tile[_width, _height];
    _visibleMap = new Boolean[_width, _height];
    for (var x = 0; x < _width; x++) {
      for (var y = 0; y < _height; y++) {
        _visibleMap[x, y] = false;
        _map[x, y] = new Tile(x, y, pseudoRandom.Next(0, 4));
      }
    }
    Vector3 cityPos = tilePos(StartPos.x, StartPos.y);
    Camera.main.transform.position = new Vector3(cityPos.x, cityPos.y, -12);
    InitCity(StartPos);
  }

  private void InitCity(Vector2 startPos) {
    _buildings = new int[_width, _height];
    _shield = new int[_width, _height];

    _buildings[(int) startPos.x, (int) startPos.y] = 1;
    _shield[(int) startPos.x, (int) startPos.y] = 1;
    _shield[(int) startPos.x - 1, (int) startPos.y + 1] = 1;
    _shield[(int) startPos.x, (int) startPos.y + 1] = 1;
    _shield[(int) startPos.x + 1, (int) startPos.y] = 1;
    _shield[(int) startPos.x, (int) startPos.y - 1] = 1;
    _shield[(int) startPos.x - 1, (int) startPos.y - 1] = 1;
    _shield[(int) startPos.x - 1, (int) startPos.y] = 1;

    foreach (Vector2 vector in getPatch((int) startPos.x, (int) startPos.y)) {
      foreach (Vector2 otherVector in getPatch((int) vector.x, (int) vector.y)) {
        _visibleMap[(int) otherVector.x, (int) otherVector.y] = true;
      }
    }
  }

  private Vector2[] getPatch(int x, int y) {
    Vector2[] vectors = new Vector2[7];
    vectors[0] = new Vector2(x, y);
    vectors[1] = new Vector2(x - 1, y + 1);
    vectors[2] = new Vector2(x, y + 1);
    vectors[3] = new Vector2(x + 1, y);
    vectors[4] = new Vector2(x, y - 1);
    vectors[5] = new Vector2(x - 1, y - 1);
    vectors[6] = new Vector2(x - 1, y);

    return vectors;
  }

  private float getHexPositionX(int column, int row) {
    var offsetX = 0f;

    if (row % 2 == 0) {
      offsetX = -(_sizeX / 2f);
    } else {
      offsetX = 0f;
    }

    return (column * _sizeX) + offsetX;
  }

  public GameObject getBuildingTemplate(Building building) {
    var buildingObj = Instantiate(TilePrefab, new Vector3(10, 10, -1), Quaternion.identity, transform);
    var renderer = buildingObj.GetComponent<SpriteRenderer>();
    renderer.sprite = _sprites[building.Type];
    _buildingToGameObjects[building] = buildingObj;
    _gameObjectToBuilding[buildingObj] = building;
    return buildingObj;
  }

  public void FinalizeTemplate(GameObject tile) {
    Building building = _gameObjectToBuilding[tile];
    City1.addBuilding(building);
    EnergyManager.removePower(building.PowerToBuild);
  }

  public void CancelTemplate(GameObject tile) {
    _buildingToGameObjects.Remove(_gameObjectToBuilding[tile]);
    _gameObjectToBuilding.Remove(tile);
  }

  private Vector3 tilePos(float x, float y) {
    return new Vector3(getHexPositionX((int) x, (int) y), (int) y * _widthOffset, 0);
  }

  private void DrawMap() {
    for (var x = 0; x < _width; x++) {
      for (var y = 0; y < _height; y++) {
        var pos = new Vector3(getHexPositionX(x, y), y * _widthOffset, 0);

        var tileObj = Instantiate(TilePrefab, pos, Quaternion.identity, this.transform) as GameObject;
        tileObj.GetComponentInChildren<TextMesh>().text =
          string.Format("{0},{1}", x, y);
        var renderer = tileObj.GetComponent<SpriteRenderer>();
        if (_visibleMap[x, y]) {
          renderer.sprite = BaseTiles[_map[x, y].BaseType];
        } else {
          renderer.sprite = BaseTiles[4];
        }
        if (_shield[x, y] == 1) {
          var shieldObj = Instantiate(ShieldPrefab, pos, Quaternion.identity, this.transform) as GameObject;
        }

        if (_buildings[x, y] == 1) {
          var buildingObj = Instantiate(CityPrefab, pos, Quaternion.identity, this.transform) as GameObject;
          var building = new Building("CityHall", BuildType.CityCenter, 0f, 0.05f, 100f, 0f);
          _buildingToGameObjects[building] = buildingObj;
          _gameObjectToBuilding[buildingObj] = building;
          City1.addBuilding(building);
        }
      }
    }
  }
}