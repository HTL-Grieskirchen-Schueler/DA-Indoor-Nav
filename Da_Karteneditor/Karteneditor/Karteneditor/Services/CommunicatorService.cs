using System.Net.Http;

namespace Karteneditor.Services;
internal class CommunicatorService
{
  public HttpClient HttpClient { get; set; }

  public CommunicatorService(string apiPath)
  {
    HttpClient = new()
    {
      BaseAddress = new Uri(apiPath)
    };
  }

  public void PostMapData(MapDataDto mapData)
  {
    //string json = JsonSerializer.Serialize(mapData);
    //var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

    //using var response = HttpClient.PostAsync($"/ImageCrl/mapData", stringContent).Result;
    //response.EnsureSuccessStatusCode();
  }
}
