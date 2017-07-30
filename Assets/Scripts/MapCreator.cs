using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class MapCreator : MonoBehaviour {
  public int Width;
  public int Height;
  public City City1 { get; private set; }
  public EnergyManager EnergyManager;
  public Sprite[] BaseTiles;
  public Vector2 StartPos;
  public GameObject TilePrefab;
  public GameObject ShieldPrefab;
  public GameObject CityPrefab;
  public bool showFog;
  public Vector2[] CoalPatches;
  public string Seed;

  private Tile[,] _map;
  private bool[,] _visibleMap;
  private int[,] _buildings;
  private int[,] _shield;

  private float _sizeX;
  private float _widthOffset;

  private Dictionary<BuildType, Sprite> _sprites;
  private Dictionary<Building, GameObject> _buildingToGameObjects;
  private Dictionary<GameObject, Building> _gameObjectToBuilding;

  private Dictionary<Tile, GameObject> _tileToGameObjects;
  private Dictionary<GameObject, Tile> _gameObjectToTile;

  private Canvas _canvas;
  private EnergyManager _manager;

  // Use this for initialization
  void Start() {
    _sizeX = (float) Math.Sqrt(3f) / 2f * 2f;
    _widthOffset = 2f * (3f / 4f);
    _buildingToGameObjects = new Dictionary<Building, GameObject>();
    _gameObjectToBuilding = new Dictionary<GameObject, Building>();
    _tileToGameObjects = new Dictionary<Tile, GameObject>();
    _gameObjectToTile = new Dictionary<GameObject, Tile>();
    _sprites = new Dictionary<BuildType, Sprite>();

    _canvas = GameObject.FindObjectOfType<Canvas>();
    _canvas.enabled = true;
    
    _manager = gameObject.GetComponent<EnergyManager>();
    
    InitSprites();
    InitMap();
    DrawMap();
  }

  private void InitSprites() {
    _sprites[BuildType.PowerPlant] = Resources.Load<Sprite>("Power");
    _sprites[BuildType.CityCenter] = Resources.Load<Sprite>("City");
    _sprites[BuildType.Mine] = Resources.Load<Sprite>("Mine");  
    _sprites[BuildType.Storage] = Resources.Load<Sprite>("Storage");
    _sprites[BuildType.ShieldGenerator] = Resources.Load<Sprite>("ShieldGen");
  }

  void Update() {

    if (_canvas.enabled) {
      return;
    }
    
    City1.update();
    EnergyManager.PowerConsumption = City1.PowerConsumption;
    _manager.PowerLeft = _manager.PowerLeft + _manager.PowerConsumption;
  }

  // Init the map
  private void InitMap() {
    var pseudoRandom = new System.Random(Seed.GetHashCode());
    City1 = new City(10);
    _map = new Tile[Width, Height];
    _visibleMap = new Boolean[Width, Height];
    for (var x = 0; x < Width; x++) {
      for (var y = 0; y < Height; y++) {
        _visibleMap[x, y] = false;
        _map[x, y] = new Tile(x, y, pseudoRandom.Next(0, 5));
      }
    }
    AddCoalPatches(pseudoRandom);

    Vector3 cityPos = tilePos(StartPos.x, StartPos.y);
    Camera.main.transform.position = new Vector3(cityPos.x, cityPos.y, -12);
    InitCity(StartPos);

    // broken mine
    _map[21, 18].BaseType = 7;
  }

  private void AddCoalPatches(Random pseudoRandom) {
    foreach (Vector2 vector in CoalPatches) {
      var patches = getPatch((int) vector.x, (int) vector.y);

      foreach (Vector2 patch in patches) {
        _map[(int) patch.x, (int) patch.y].BaseType = 6;
        _map[(int) patch.x, (int) patch.y].HasCoal = true;
        _map[(int) patch.x, (int) patch.y].CoalAmount = pseudoRandom.Next(500, 3000);
      }
    }
  }

  private void InitCity(Vector2 startPos) {
    _buildings = new int[Width, Height];
    _shield = new int[Width, Height];

    Vector2[] circleVectors = getPatch((int) startPos.x, (int) startPos.y);
    _buildings[(int) startPos.x, (int) startPos.y] = 1;

    foreach (Vector2 vector in circleVectors) {
      _shield[(int) vector.x, (int) vector.y] = 1;
      foreach (Vector2 otherVector in getPatch((int) vector.x, (int) vector.y)) {
        removeFogOfWar((int) otherVector.x, (int) otherVector.y, 1);
      }
    }
  }

  // Returns a "circle" of hexes
  private Vector2[] getPatch(int x, int y) {
    Vector2[] vectors = new Vector2[7];

    if (y % 2 == 0) {
      vectors[0] = new Vector2(x, y);
      vectors[1] = new Vector2(x - 1, y + 1);
      vectors[2] = new Vector2(x, y + 1);
      vectors[3] = new Vector2(x + 1, y);
      vectors[4] = new Vector2(x, y - 1);
      vectors[5] = new Vector2(x - 1, y - 1);
      vectors[6] = new Vector2(x - 1, y);
    } else {
      vectors[0] = new Vector2(x, y);
      vectors[1] = new Vector2(x + 1, y + 1);
      vectors[2] = new Vector2(x + 1, y - 1);
      vectors[3] = new Vector2(x, y - 1);
      vectors[4] = new Vector2(x + 1, y);
      vectors[5] = new Vector2(x - 1, y);
      vectors[6] = new Vector2(x, y + 1);
    }
    return vectors;
  }

  private float getHexPositionX(int column, int row) {
    float offsetX;
    if (row % 2 == 0) {
      offsetX = -(_sizeX / 2f);
    } else {
      offsetX = 0f;
    }

    return (column * _sizeX) + offsetX;
  }

  public GameObject getBuildingTemplate(Building building) {
    var buildingObj = Instantiate(TilePrefab, new Vector3(10, 10, -1), Quaternion.identity, transform);
    buildingObj.name = "Drag";
    buildingObj.layer = LayerMask.NameToLayer("DragTiles");
    var renderer = buildingObj.GetComponent<SpriteRenderer>();
    renderer.sprite = _sprites[building.Type];
    _buildingToGameObjects[building] = buildingObj;
    _gameObjectToBuilding[buildingObj] = building;
    return buildingObj;
  }

  public void FinalizeTemplate(GameObject tileObject, GameObject parentTileObject) {
    Building building = _gameObjectToBuilding[tileObject];
    City1.addBuilding(building);
    EnergyManager.removePower(building.PowerToBuild);
    tileObject.tag = "Untagged";
    tileObject.layer = LayerMask.NameToLayer("BaseTiles");
    var parentTile = _gameObjectToTile[parentTileObject];
    removeFogOfWar(parentTile.X, parentTile.Y, 1, false);

    if (building.Type == BuildType.ShieldGenerator) {
      addShield((int) parentTile.X, (int) parentTile.Y, 2);
    }
  }

  public void CancelTemplate(GameObject tile) {
    _buildingToGameObjects.Remove(_gameObjectToBuilding[tile]);
    _gameObjectToBuilding.Remove(tile);
  }

  private void addShield(int x, int y, int r) {
    foreach (Vector2 otherVector in getPatch(x, y)) {
      _visibleMap[(int) otherVector.x, (int) otherVector.y] = true;
      Tile tile = _map[(int) otherVector.x, (int) otherVector.y];
      GameObject tileObj = _tileToGameObjects[tile];

      if (_shield[(int) otherVector.x, (int) otherVector.y] != 1) {
        var shieldObj =
          Instantiate(ShieldPrefab, tileObj.transform.position, Quaternion.identity, this.transform) as GameObject;
        _shield[(int) otherVector.x, (int) otherVector.y] = 1;
      }
      if (r > 1) {
        addShield((int) otherVector.x, (int) otherVector.y, r - 1);
      }
    }
  }

  private void removeFogOfWar(int x, int y, int r, bool start = true) {
    foreach (Vector2 otherVector in getPatch(x, y)) {
      _visibleMap[(int) otherVector.x, (int) otherVector.y] = true;

      if (start == false) {
        Tile tile = _map[(int) otherVector.x, (int) otherVector.y];
        GameObject tileObj = _tileToGameObjects[tile];
        var spriteRenderer = tileObj.GetComponent<SpriteRenderer>();
        tileObj.tag = "Untagged";
        spriteRenderer.sprite = BaseTiles[_map[(int) otherVector.x, (int) otherVector.y].BaseType];
      }
      if (r > 1) {
        removeFogOfWar((int) otherVector.x, (int) otherVector.y, r - 1, start);
      }
    }
  }

  private Vector3 tilePos(float x, float y) {
    return new Vector3(getHexPositionX((int) x, (int) y), (int) y * _widthOffset, 0);
  }

  private void DrawMap() {
    for (var x = 0; x < Width; x++) {
      for (var y = 0; y < Height; y++) {
        var pos = new Vector3(getHexPositionX(x, y), y * _widthOffset, 0);

        var tileObj = Instantiate(TilePrefab, pos, Quaternion.identity, this.transform) as GameObject;

        tileObj.GetComponentInChildren<TextMesh>().text = string.Format("{0},{1}", x, y);
        tileObj.name = string.Format("Hex: {0},{1}", x, y);
        tileObj.layer = LayerMask.NameToLayer("BaseTiles");

        var spriteRenderer = tileObj.GetComponent<SpriteRenderer>();
        if (_visibleMap[x, y] || showFog == false) {
          spriteRenderer.sprite = BaseTiles[_map[x, y].BaseType];
        } else {
          spriteRenderer.sprite = BaseTiles[5];
          tileObj.tag = "fog";
        }

        _tileToGameObjects[_map[x, y]] = tileObj;
        _gameObjectToTile[tileObj] = _map[x, y];

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