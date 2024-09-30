using EditorLib.Nodes;

using EditorUserControll;

using System.Windows.Controls;

namespace EditorLib;
public partial class ImageControl
{
  private List<NodeConnection> _removedConnections = [];

  private void OnStartNodeMove(object? sender, Node.NodeEventArgs e)
  {
    if (e.EventNode is not IntNode intNode) return;
    var list = intNode.AttatchedConnections;

    while (list.Count != 0)
    {
      _removedConnections.Add(list.First());
      RemoveNodeConnection(list.First());
    }
  }

  private void OnEndNodeMove(object? sender, Node.NodeEventArgs e)
  {
    _removedConnections.ForEach(x => AddConnection(x.ConnectionNode1.Id, x.ConnectionNode2.Id));
    _removedConnections = [];
    e.EventNode.Left = Canvas.GetLeft(e.EventNode);
    e.EventNode.Top = Canvas.GetTop(e.EventNode);
  }
}
