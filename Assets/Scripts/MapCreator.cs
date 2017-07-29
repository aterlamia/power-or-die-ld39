﻿using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MapCreator : MonoBehaviour {
  private int _width = 20;
  private int _height = 20;

  public GameObject TilePrefab;
  public GameObject ShieldPrefab;
  public GameObject CityPrefab;

  private int[,] _map;
  private int[,] _buildings;
  private int[,] _shield;
  private float _sizeX;
  private float _sizeY;
  private float _widthOffset;
  public Vector2 StartPos;
  public EnergyManager EnergyManager;

  public City City1 { get; private set; }

  private Dictionary<BuildType, Sprite> _sprites;
  private Dictionary<Building, GameObject> _buildingToGameObjects;
  private Dictionary<GameObject, Building> _gameObjectToBuilding;

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
  }

  // Update is called once per frame
  void Update() {
    EnergyManager.PowerConsumption = City1.PowerConsumption;
  }

  private void InitMap() {
    City1 = new City(10);
    _map = new int[_width, _height];
    for (var x = _width; x < _width; x++) {
      for (var y = _height; y < _height; y++) {
        // For now all tiles are the same
        _map[x, y] = 1;
      }
    }
    Camera.main.transform.position = new Vector3(StartPos.x, StartPos.y, -10  );
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
    var buildingObj = Instantiate(TilePrefab, new Vector3(10, 10, -1), Quaternion.identity , transform);
    var renderer = buildingObj.GetComponent<SpriteRenderer>();
    renderer.sprite = _sprites[building.Type];  
    _buildingToGameObjects[building] = buildingObj;
    _gameObjectToBuilding[buildingObj] = building;
    return buildingObj;
  }
  
  public void FinalizeTemplate(GameObject tile) {
    City1.addBuilding(_gameObjectToBuilding[tile]);
  }
  
  public void CancelTemplate(GameObject tile) {
    _buildingToGameObjects.Remove(_gameObjectToBuilding[tile]);
    _gameObjectToBuilding.Remove(tile);
  }
  
  private void DrawMap() {
    for (var x = 0; x < _width; x++) {
      for (var y = 0; y < _height; y++) {
        var pos = new Vector3(getHexPositionX(x, y), y * _widthOffset, 0);
        var tileObj = Instantiate(TilePrefab, pos, Quaternion.identity, this.transform) as GameObject;
        tileObj.GetComponentInChildren<TextMesh>().text =
          string.Format("{0},{1}", x, y);

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