using EditorLib.Nodes.Dtos;

namespace Karteneditor;
public class MapDataDto
{
  public List<IntNodeDto> IntNodes { get; set; } = null!;
  public List<AccessPointDto> AccessPoints { get; set; } = null!;
  public string base64Img { get; set; } = null!;
}
