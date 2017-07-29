public class Tile {
  public int BaseType { get; private set; }
  public int X { get; private set; }
  public int Y { get; private set; }
  private Building _building;

  public Tile(int x, int y, int baseType) {
    BaseType = baseType;
    X = x;
    Y = y;
  }
}