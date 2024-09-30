namespace DAServer.Dtos;

public class MapDataDto
{
  public List<IntNodeDto> IntNodes { get; set; }
  public List<AccessPointDto> AccessPoints { get; set; }
  public string base64 { get; set; }
}
