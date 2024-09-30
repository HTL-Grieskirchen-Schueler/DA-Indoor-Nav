using EditorLib.ExtensionMethods;
using EditorLib.Nodes;

using System.Windows.Controls;
using System.Windows.Input;

namespace EditorLib;
public partial class ImageControl
{
  public List<IntNode> IntNodes { get; set; } = [];
  public List<AccessPointNode> ApNodes { get; set; } = [];

  public void AddNode(Node node)
  {
    if (node is IntNode intNode)
    {
      intNode.Id = IntNodes.GetNextId();
      IntNodes.Add(intNode);
    }
    else if (node is AccessPointNode apNode)
    {
      apNode.Id = IntNodes.GetNextId();
      ApNodes.Add(apNode);
    }

    node.NodeClicked += NodeClicked;
    node.StartMoving += OnStartNodeMove;
    node.EndMoving += OnEndNodeMove;

    Canvas.SetLeft(node, node.Left - _nodeWidth / 2);
    Canvas.SetTop(node, node.Top - _nodeWidth / 2);

    node.NodeWidth = _nodeWidth;

    NodeCanvas.Children.Add(node);
  }

  private void AddNode(Node node, MouseButtonEventArgs e)
  {
    node.Left = e.GetPosition(NodeCanvas).X;
    node.Top = e.GetPosition(NodeCanvas).Y;

    AddNode(node);
  }

  private void RemoveNode(Node node)
  {
    NodeCanvas.Children.Remove(node);

    if (node is IntNode intNode)
    {
      NodeCanvas.Children.RemoveConnectionsWithNode(node);
      IntNodes.Remove(intNode);
      intNode.ConnectedNodes.OfType<IntNode>().ToList().ForEach(x => x.ConnectedNodes.Remove(intNode));
    }
    else if (node is AccessPointNode apNode)
    {
      ApNodes.Remove(apNode);
    }
  }

  private void NodeClicked(object sender, Node.NodeEventArgs e)
  {
    if (Mode == EditModes.Remove)
    {
      RemoveNode(e.EventNode);
      if (LastSelectedNode == e.EventNode) LastSelectedNode = null;
      return;
    }

    if (LastSelectedNode == null)
    {
      LastSelectedNode = e.EventNode;
      return;
    }

    if (LastSelectedNode is IntNode lsNode
      && e.EventNode is IntNode sNode
      && Mode == EditModes.AddConnection)
    {
      AddConnection(lsNode, sNode);
    }

    LastSelectedNode = e.EventNode;
  }
}
