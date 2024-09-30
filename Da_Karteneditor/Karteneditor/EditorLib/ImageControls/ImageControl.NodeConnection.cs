using EditorLib.ExtensionMethods;
using EditorLib.Nodes;

using EditorUserControll;

namespace EditorLib;
public partial class ImageControl
{
  public void AddConnection(int id1, int id2)
  {
    var node1 = IntNodes.Find(x => x.Id == id1)!;
    var node2 = IntNodes.Find(x => x.Id == id2)!;
    AddConnection(node1, node2);
  }

  private void AddConnection(IntNode node1, IntNode node2)
  {
    if (node1.ConnectedNodes.Contains(node2)) return;
    if (node2.ConnectedNodes.Contains(node1)) return;

    node1.ConnectedNodes.Add(node2);
    node2.ConnectedNodes.Add(node1);
    var connection = new NodeConnection { ConnectionNode1 = node1, ConnectionNode2 = node2 };

    node1.AttatchedConnections.Add(connection);
    node2.AttatchedConnections.Add(connection);

    connection.ConnectionClicked += OnConnectionClicked;

    NodeCanvas.DrawConnectionNode(connection, _nodeWidth);
  }

  private void OnConnectionClicked(object? sender, NodeConnection.NodeConnectionEventArgs e)
  {
    if (Mode == EditModes.Remove)
    {
      RemoveNodeConnection(e.EventNodeConnection);
    }
  }

  private void RemoveNodeConnection(NodeConnection e)
  {
    var node1 = (e.ConnectionNode1 as IntNode)!;
    var node2 = (e.ConnectionNode2 as IntNode)!;

    node1.ConnectedNodes.Remove(node2);
    node2.ConnectedNodes.Remove(node1);

    node1.AttatchedConnections.Remove(e);
    node2.AttatchedConnections.Remove(e);

    NodeCanvas.Children.Remove(e);
  }
}
