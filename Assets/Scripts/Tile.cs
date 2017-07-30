public class Tile {
  public int BaseType { get; set; }
  public int X { get; private set; }
  public int Y { get; private set; }

  private bool hasBuilding = false;
  public Building Building { get; set; }
  private bool hasCoal = false;
  private int coalAmount = 0;
  
  public Tile(int x, int y, int baseType) {
    BaseType = baseType;
    X = x;
    Y = y;
  }
  
  public bool HasCoal {
    get { return hasCoal; }
    set { hasCoal = value; }
  }

  public int CoalAmount {
    get { return coalAmount; }
    set { coalAmount = value; }
  }

  public bool HasBuilding {
    get { return hasBuilding; }
    set { hasBuilding = value; }
  }

}