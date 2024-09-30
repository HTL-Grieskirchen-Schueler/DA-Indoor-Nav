namespace ShortestPathLib;
public class SpfNode
{
  public int Id { get; set; }
  public double X { get; set; }
  public double Y { get; set; }
  public List<SpfNode> Neighbors { get; set; }
  public SpfNode? CameFrom { get; set; }
  public double GScore { get; set; }
  public double FScore { get; set; }
  public SpfNode()
  {
    Neighbors = new List<SpfNode>();
    GScore = double.MaxValue;
    FScore = double.MaxValue;
  }

  public override string ToString() => $"{Id}";
}
