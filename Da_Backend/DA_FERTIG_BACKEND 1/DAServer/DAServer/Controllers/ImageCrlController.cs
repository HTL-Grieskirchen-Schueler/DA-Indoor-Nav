using DAServer.Dtos;
using DAServer.Services;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using NuGet.Protocol;

using System.Drawing;
using System.Text.Json;

namespace DAServer.Controllers;
[Route("[controller]")]
[ApiController]
public class ImageCrlController(MapService mapService) : ControllerBase
{
    private readonly MapService _mapService = mapService;

    [HttpGet("nodes")]
    public List<IntNodeDto> GetNodes() => _mapService.IntNodes.ToList();

    [HttpPost("mapData")]
    public IActionResult PostMapData([FromBody] string jsonMapData)
    {
        this.Log();
        var mapData = (MapDataDto)JsonSerializer.Deserialize(jsonMapData, typeof(MapDataDto))!;

        this.Log();
        Console.Write(mapData.ToJson());
        _mapService.IntNodes = mapData.IntNodes;
        _mapService.AccessPoints = mapData.AccessPoints;
        _mapService.ImgBase64 = mapData.base64;

        return Ok(jsonMapData);
    }

    [HttpGet("img")]
    public string GetImg() => _mapService.ImgBase64;

}
