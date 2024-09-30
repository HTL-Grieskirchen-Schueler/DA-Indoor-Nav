namespace ShortestPathLib;
public class AStarAlgorythm
{
  public List<SpfNode> FindShortestPath(SpfNode startNode, SpfNode targetNode)
  {
    var openSet = new List<SpfNode> { startNode };
    var closedSet = new HashSet<SpfNode>();

    startNode.GScore = 0;
    startNode.FScore = Heuristic(startNode, targetNode);

    while (openSet.Count > 0)
    {
      var currentNode = GetLowestFScoreNode(openSet);
      if (currentNode == targetNode)
      {
        return ReconstructPath(currentNode);
      }

      openSet.Remove(currentNode);
      closedSet.Add(currentNode);

      foreach (var neighbor in currentNode.Neighbors)
      {
        if (closedSet.Contains(neighbor))
        {
          continue;
        }

        var tentativeGScore = currentNode.GScore + 1;

        if (!openSet.Contains(neighbor))
        {
          openSet.Add(neighbor);
        }
        else if (tentativeGScore >= neighbor.GScore)
        {
          continue;
        }

        neighbor.CameFrom = currentNode;
        neighbor.GScore = tentativeGScore;
        neighbor.FScore = neighbor.GScore + Heuristic(neighbor, targetNode);
      }
    }
    return null;
  }

  private double Heuristic(SpfNode node1, SpfNode node2)
  {
    return Math.Sqrt(Math.Pow(node2.X - node1.X, 2) + Math.Pow(node2.Y - node1.Y, 2));
  }

  private SpfNode GetLowestFScoreNode(List<SpfNode> openSet)
  {
    var lowestNode = openSet[0];
    foreach (var node in openSet)
    {
      if (node.FScore < lowestNode.FScore)
      {
        lowestNode = node;
      }
    }
    return lowestNode;
  }

  private List<SpfNode> ReconstructPath(SpfNode currentNode)
  {
    var path = new List<SpfNode> { currentNode };
    while (currentNode.CameFrom != null)
    {
      currentNode = currentNode.CameFrom;
      path.Insert(0, currentNode);
    }
    return path;
  }
}
