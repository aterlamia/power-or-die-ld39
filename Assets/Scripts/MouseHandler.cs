using UnityEngine;
using Debug = UnityEngine.Debug;

public class MouseHandler : MonoBehaviour {
  public LayerMask LayerIDForTiles;
  public LayerMask LayerIDForUi;

  public Canvas ScienceCanvas;

  private MapCreator _mapCreator;
  private bool _isDragging = false;
  private Vector3 _lastMousePos;

  private GameObject _followTemplate;
  private Building _followBuilding;

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

      SpriteRenderer renderer = _followTemplate.GetComponent<SpriteRenderer>();
      if (IsValidBaseTile(obj)) {
        renderer.color = Color.green;
      } else {
        renderer.color = Color.red;
      }

      Vector3 pos = obj.transform.position;
      pos.z = 0f;
      _followTemplate.transform.position = pos;
    }

    _lastMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    if (Input.GetMouseButtonDown(0)) { } else if (Input.GetMouseButtonUp(0)) {
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
    GameObject tileobj = getClickedTileObj();
    Tile tile = _mapCreator.GetTileForGameObject(tileobj);

    if (tile.Building != null) {
      if (tile.Building.Type == BuildType.ScienceLab) {
        ScienceCanvas.enabled = true;
      }
    }

    // No tile then no click bail out.
    if (tileobj == null || _followTemplate == null || tileobj.CompareTag("fog")) {
      return false;
    }

    if (IsValidBaseTile(tileobj)) {
      _mapCreator.FinalizeTemplate(_followTemplate, tileobj);
      _followTemplate = null;
    }
    return true;
  }

  private bool IsValidBaseTile(GameObject tileObj) {
    Tile tile = _mapCreator.GetTileForGameObject(tileObj);

    if (tile.HasBuilding) {
      return false;
    }
    if (_followBuilding.Type == BuildType.Mine) {
      return tile.HasCoal;
    } else {
      return !tile.HasCoal;
    }
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

    if (MenuCollapse(hitInfo)) return true;

    Building building;

    ResourcesManager resourcesManager = _mapCreator.ResourceManager;
    // Move buildings creation to a factory when time.
    switch (hitInfo.collider.name) {
      case "btnPower":
        building = BuildingFactory.createBuilding(BuildType.PowerPlant, resourcesManager);
        _followTemplate = _mapCreator.getBuildingTemplate(building);
        break;
      case "btnMine":
        building = BuildingFactory.createBuilding(BuildType.Mine, resourcesManager);
        _followTemplate = _mapCreator.getBuildingTemplate(building);
        break;
      case "btnStore":
        building = BuildingFactory.createBuilding(BuildType.Storage, resourcesManager);
        _followTemplate = _mapCreator.getBuildingTemplate(building);
        break;
      case "btnHouse":
        building = BuildingFactory.createBuilding(BuildType.House, resourcesManager);
        _followTemplate = _mapCreator.getBuildingTemplate(building);
        break;
      case "btnScience":
        building = BuildingFactory.createBuilding(BuildType.ScienceLab, resourcesManager);
        _followTemplate = _mapCreator.getBuildingTemplate(building);
        break;
      case "btnShield":
      default:
        building = BuildingFactory.createBuilding(BuildType.ShieldGenerator, resourcesManager);
        _followTemplate = _mapCreator.getBuildingTemplate(building);
        break;
    }

    _followBuilding = building;
    return true;
  }

  private static bool MenuCollapse(RaycastHit2D hitInfo) {
    if (hitInfo.collider.name == "menuClosed") {
      hitInfo.collider.name = "menuOpen";
      GameObject menu = hitInfo.collider.gameObject;
      SpriteRenderer[] renderers = menu.GetComponentInChildren<Transform>().GetComponentsInChildren<SpriteRenderer>();
      foreach (SpriteRenderer spriteRenderer in renderers) {
        spriteRenderer.enabled = true;
      }
      Transform[] buttons = menu.GetComponentsInChildren<Transform>();

      foreach (Transform button in buttons) {
        if (button.name != "btnSciene" && button.name != "btnPower" && button.name != "btnMine" &&
            button.name != "btnHouse") {
          continue;
        }
        if (button.CompareTag("disabled")) {
          Debug.Log(1);
          continue;
        } else {
          BoxCollider2D collider = button.GetComponentInChildren<BoxCollider2D>();
          collider.enabled = true;
        }
      }
      return true;
    }

    if (hitInfo.collider.name == "menuOpen") {
      hitInfo.collider.name = "menuClosed";
      GameObject menu = hitInfo.collider.gameObject;
      SpriteRenderer[] renderers = menu.gameObject.GetComponentsInChildren<SpriteRenderer>();
      foreach (SpriteRenderer spriteRenderer in renderers) {
        spriteRenderer.enabled = false;
      }
      BoxCollider2D[] colliders = menu.GetComponentInChildren<Transform>().GetComponentsInChildren<BoxCollider2D>();
      foreach (BoxCollider2D collider in colliders) {
        collider.enabled = false;
      }
      menu.GetComponent<SpriteRenderer>().enabled = true;
      menu.GetComponent<BoxCollider2D>().enabled = true;
      return true;
    }
    return false;
  }
}