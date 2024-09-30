using EditorUserControll;

using System.Windows.Controls;
using System.Windows.Media;

namespace EditorLib.ExtensionMethods;
public static class CanvasExtensionMethods
{
  public static void DrawConnectionNode(this Canvas nodeCanvas, NodeConnection connection, int nodeWith = 50)
    => DrawConnectionNode(nodeCanvas, 
      connection, 
      Canvas.GetLeft(connection.ConnectionNode1), 
      Canvas.GetTop(connection.ConnectionNode1), 
      Canvas.GetLeft(connection.ConnectionNode2), 
      Canvas.GetTop(connection.ConnectionNode2), 
      nodeWith);

  public static void DrawConnectionNode(this Canvas nodeCanvas, NodeConnection connection, double x1, double y1, double x2, double y2, int nodeWidth = 50)
  {
    connection.X1 = x1;
    connection.Y1 = y1;
    connection.X2 = x2;
    connection.Y2 = y2;

    connection.Stroke = Brushes.Blue;

    connection.SetValue(Canvas.LeftProperty, x1 + nodeWidth / 2);
    connection.SetValue(Canvas.TopProperty, y1 + nodeWidth / 2);

    nodeCanvas.Children.Insert(0, connection);
  }
}
