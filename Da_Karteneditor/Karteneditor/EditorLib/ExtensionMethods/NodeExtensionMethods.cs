using EditorLib.Nodes;
using EditorLib.Nodes.Dtos;

namespace EditorLib.ExtensionMethods;
public static class NodeExtensionMethods
{
  public static IntNodeDto ToIntNodeDto(this IntNode intNode, ImageControl window)
    => new()
    {
      Id = intNode.Id,
      Name = intNode.NodeName,
      Left = intNode.Left / window.Image.Width,
      Top = intNode.Top / window.Image.Height,
      ConnectedNodes = intNode.ConnectedNodes.Select(x => x.Id).ToList(),
    };

  public static AccessPointDto ToAccessPointDto(this AccessPointNode apNode, ImageControl window)
    => new()
    {
      Id = apNode.Id,
      Name = apNode.NodeName,
      Left = apNode.Left / window.Image.Width,
      Top = apNode.Top / window.Image.Height,
      MacAddress = apNode.MacAddress,
    };

  public static List<IntNodeDto> ToNodeDtoList(this IEnumerable<IntNode> intNodes, ImageControl window)
    => intNodes.Select(x => x.ToIntNodeDto(window)).ToList();

  public static List<AccessPointDto> ToNodeDtoList(this IEnumerable<AccessPointNode> apNodes, ImageControl window)
    => apNodes.Select(x => x.ToAccessPointDto(window)).ToList();

  public static IntNode ToIntNode(this IntNodeDto intNodeDto, ImageControl window)
  => new()
  {
    Id = intNodeDto.Id,
    NodeName = intNodeDto.Name,
    Left = intNodeDto.Left * window.Image.Width,
    Top = intNodeDto.Top * window.Image.Height,
    ConnectedNodes = [],
  };

  public static AccessPointNode ToApNode(this AccessPointDto apNodeDto, ImageControl window)
  => new()
  {
    Id = apNodeDto.Id,
    NodeName = apNodeDto.Name,
    Left = apNodeDto.Left * window.Image.Width,
    Top = apNodeDto.Top * window.Image.Height,
    MacAddress = apNodeDto.MacAddress,
  };

  public static List<IntNode> ToNodeList(this IEnumerable<IntNodeDto> intNodes, ImageControl window)
    => intNodes.Select(x => x.ToIntNode(window)).ToList();

  public static List<AccessPointNode> ToNodeList(this IEnumerable<AccessPointDto> intNodes, ImageControl window)
    => intNodes.Select(x => x.ToApNode(window)).ToList();
}
