using EditorLib;
using EditorLib.ExtensionMethods;
using EditorLib.Nodes.Dtos;

using System.IO;
using System.Text.Json;

namespace Karteneditor.Services;
public class SaveService(string basePath, ImageControl imageView)
{
  private string BasePath { get; init; } = basePath;
  public ImageControl ImageView { get; init; } = imageView;

  public void LoadImage() => ImageView.Image = File.ReadAllText(@$"{BasePath}\base64").ToWritableImage();

  public void SaveContent()
  {
    var intNodeDtos = ImageView.IntNodes.ToNodeDtoList(ImageView);
    File.WriteAllText(@$"{BasePath}\intnodes.json", JsonSerializer.Serialize(intNodeDtos));

    var accessPointDtos = ImageView.ApNodes.ToNodeDtoList(ImageView);
    File.WriteAllText(@$"{BasePath}\apnodes.json", JsonSerializer.Serialize(accessPointDtos));
  }

  public void LoadContent()
  {
    var intNodes = JsonSerializer.Deserialize<List<IntNodeDto>>(File.ReadAllText(@$"{BasePath}\intnodes.json"))!;
    var apNodes = JsonSerializer.Deserialize<List<AccessPointDto>>(File.ReadAllText(@$"{BasePath}\apnodes.json"))!;

    intNodes.ToNodeList(ImageView).ForEach(ImageView.AddNode);
    apNodes.ToNodeList(ImageView).ForEach(ImageView.AddNode);

    intNodes.ForEach(node => node.ConnectedNodes.ForEach(x => ImageView.AddConnection(node.Id, x)));
  }
}
