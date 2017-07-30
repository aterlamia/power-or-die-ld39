using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour {
  public LayerMask LayerIDForTiles;
  public LayerMask LayerIDForUi;
  private MapCreator _mapCreator;
  private bool _isDragging = false;
  private Vector3 _lastMousePos;

  private GameObject _followTemplate;

  // Use this for initialization
  void Start() {
    _mapCreator = GameObject.FindObjectOfType<MapCreator>();
  }

  // Update is called once per frame
  void Update() {
    if (Input.GetKeyDown(KeyCode.Escape)) {
      if (_followTemplate != null) {
        _mapCreator.CancelTemplate(_followTemplate);
        Destroy(_followTemplate);
      }
      _followTemplate = null;
    }

    if (Input.GetMouseButtonDown(2)) {
      _isDragging = true;
    }
    if (Input.GetMouseButtonUp(2)) {
      _isDragging = false;
    }

    if (_isDragging) {
      Vector3 currentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
      Vector3 diff = _lastMousePos - currentPos;

      Camera.main.transform.Translate(diff, Space.World);
    }

    if (_followTemplate != null) {
      var obj = getClickedTileObj();
      if (obj == null) {
        return;
      }
      Vector3 pos = obj.transform.position;
      pos.z = -6f;
      _followTemplate.transform.position = pos;
    }

    _lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    if (Input.GetMouseButtonDown(0)) {
    } else if (Input.GetMouseButtonUp(0)) {
      GameObject tile = MouseToTile();

      if (tile == null) {
        return;
      }
      tile.GetComponentInChildren<TextMesh>().text =
        string.Format("{0},{1}", 'a', 'b');
    }
  }

  GameObject MouseToTile() {
    // Do not check menu when building.
    if (_followTemplate == null) {
      if (handleMenuClicks()) return null;
    }
    if (HandleTileClicks()) return null;

    return null;
  }

  private GameObject getClickedTileObj() {
    RaycastHit2D hitInfo = Physics2D.Raycast(
      new Vector2(
        Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
        Camera.main.ScreenToWorldPoint(Input.mousePosition).y
      ),
      Vector2.zero,
      0f,
      LayerIDForTiles.value
    );
    if (hitInfo.collider != null) {
      // Something got hit
      // The collider is a child of the "correct" game object that we want.
      GameObject tile = hitInfo.collider.gameObject;
      return tile;
    }
    return null;
  }

  private bool HandleTileClicks() {
    GameObject tile = getClickedTileObj();

    // No tile then no click bail out.
    if (tile == null || _followTemplate == null || tile.CompareTag("fog")) {
      return false;
    }

    _mapCreator.FinalizeTemplate(_followTemplate, tile);
    _followTemplate = null;
    return true;
  }

  private bool handleMenuClicks() {
    RaycastHit2D hitInfo = Physics2D.Raycast(
      new Vector2(
        Camera.main.ScreenToWorldPoint(Input.mousePosition).x,
        Camera.main.ScreenToWorldPoint(Input.mousePosition).y
      ),
      Vector2.zero,
      0f,
      LayerIDForUi.value
    );

    if (hitInfo.collider == null) {
      return false;
    }

    Building building;
    
    // Move buildings creation to a factory when time.
    switch (hitInfo.collider.name) {
      case "btnPower":
        building = new Building("Plant1", BuildType.PowerPlant, 0.05f, 0f, 200f, 0);
        _followTemplate = _mapCreator.getBuildingTemplate(building);
        break;
      case "btnMine":
        building = new Building("Mine1", BuildType.Mine, 0f, 0.04f, 200f, 0);
        _followTemplate = _mapCreator.getBuildingTemplate(building);
        break;
      case "btnStore":
        building = new Building("Storage1", BuildType.Storage, 0f, 0.06f, 100f, 0);
        _followTemplate = _mapCreator.getBuildingTemplate(building);
        break;
      case "btnShield":
        building = new Building("Shield1", BuildType.ShieldGenerator, 0f, 0.15f, 100f, 0);
        _followTemplate = _mapCreator.getBuildingTemplate(building);
        break;
    }
    GameObject menu = hitInfo.collider.gameObject;
    SpriteRenderer[] renderers = menu.GetComponentInChildren<Transform>().GetComponentsInChildren<SpriteRenderer>();
    foreach (SpriteRenderer renderer in renderers) {
      renderer.enabled = true;
    }
    return true;
  }
}