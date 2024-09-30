
using DAServer.Dtos;

using System.Text.Json;

namespace DAServer.Services;

public class LoadContentBackgroundService(IServiceProvider serviceProvider) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var mapService = scope.ServiceProvider.GetRequiredService<MapService>();

        string contentPath = "Content\\";

        List<IntNodeDto> intNodeDtos = JsonSerializer.Deserialize<List<IntNodeDto>>(File.ReadAllText($"{contentPath}intnodes.json"))!;
        List<AccessPointDto> apDtos = JsonSerializer.Deserialize<List<AccessPointDto>>(File.ReadAllText($"{contentPath}apnodes.json"))!;
        string base64 = File.ReadAllText($"{contentPath}\\base64");

        mapService.ImgBase64 = base64;
        mapService.IntNodes = intNodeDtos;
        mapService.AccessPoints = apDtos;

        return Task.Run(() => { }, stoppingToken);
    }
}
