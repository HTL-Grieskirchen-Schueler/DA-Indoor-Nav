using EditorLib;
using EditorLib.Nodes;

using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace EditorUserControll;
/// <summary>
/// Interaction logic for NodeConnection.xaml
/// </summary>
public partial class NodeConnection : UserControl
{
  public Node ConnectionNode1 { get; set; }
  public Node ConnectionNode2 { get; set; }

  private double _x1;
  private double _y1;
  private double _x2;
  private double _y2;

  public double X1
  {
    get => _x1;
    set
    {
      _x1 = value;
      UpdateLineDisplay();
    }
  }
  public double Y1
  {
    get => _y1; set
    {
      _y1 = value;
      UpdateLineDisplay();
    }
  }
  public double X2
  {
    get => _x2; set
    {
      _x2 = value;
      UpdateLineDisplay();
    }
  }
  public double Y2
  {
    get => _y2; set
    {
      _y2 = value;
      UpdateLineDisplay();
    }
  }

  public record class NodeConnectionEventArgs(NodeConnection EventNodeConnection);

  public EventHandler<NodeConnectionEventArgs> ConnectionClicked;

  public Brush Stroke { get => ConnectionLine.Stroke; set => ConnectionLine.Stroke = value; }

  public NodeConnection() => InitializeComponent();

  private void UpdateLineDisplay()
  {
    RenderTransformOrigin = new Point(0, 0);
    var flipTrans = new ScaleTransform();

    double tempWidht = X2 - X1;
    double tempHeight = Y2 - Y1;

    if (tempWidht < 0)
    {
      flipTrans.ScaleX = -1;
    }
    if (tempHeight < 0)
    {
      flipTrans.ScaleY = -1;
    }

    RenderTransform = flipTrans;

    Width = Math.Abs(tempWidht);
    Height = Math.Abs(tempHeight);
  }

  private void ConnectionLine_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
    => ConnectionClicked.Invoke(this, new NodeConnectionEventArgs(this));
}
