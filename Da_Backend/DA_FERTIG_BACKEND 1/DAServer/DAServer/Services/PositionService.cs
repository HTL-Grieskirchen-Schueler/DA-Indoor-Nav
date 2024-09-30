
using DAServer.Dtos;

using ShortestPathLib;

namespace DAServer.Services;

public class PositionService(MapService mapService)
{
  private readonly MapService _mapService = mapService;

  AStarAlgorythm AStarAlgorythm { get; set; } = new();

  public List<int> GetShortestPath(int id1, int id2)
  {
        if (id2<id1)
        {
            var temp = id1;
            id1 = id2;
            id2 = temp;
        }
    var list = _mapService.GetNodesAsSpfList(_mapService.TestIntNodes);

    var node1 = list
      .Find(x => x.Id == id1);
    var node2 = list
      .Find(x => x.Id == id2);


    var spf = AStarAlgorythm.FindShortestPath(node1, node2);

    return spf.Select(x => x.Id).ToList();
  }
}
