namespace DAServer.Dtos;

public class IntNodeDto : NodeDto
{
  public List<int> ConnectedNodes { get; set; } = [];
}
