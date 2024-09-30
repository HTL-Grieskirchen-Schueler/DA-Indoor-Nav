namespace DAServer.Dtos;

public class AccessPointDto : NodeDto
{
  public string MacAddress { get; set; } = "";
    public double distance { get; set; } = 0.0;
}
