using EditorUserControll;

using System.Windows.Media;

namespace EditorLib.Nodes;
public class IntNode : Node
{
  public List<NodeConnection> AttatchedConnections { get; set; } = [];

  public List<Node> ConnectedNodes { get; set; } = [];

  public IntNode() : base()
  {
    _propsToEdit = [.. _propsToEdit];
    DefaultColor = Brushes.Black;
    FillColor = DefaultColor;
  }
}
