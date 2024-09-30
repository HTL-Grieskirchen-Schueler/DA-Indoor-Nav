using EditorLib.Nodes;

using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace EditorLib;
/// <summary>
/// Interaction logic for ImageControl.xaml
/// </summary>
public partial class ImageControl : UserControl
{
  public static EditModes Mode { get; set; } = EditModes.MoveWindow;

  private WriteableBitmap? _image;
  public WriteableBitmap Image
  {
    get => _image;
    set
    {
      _image = value;
      NodeCanvas.Width = Image.Width;
      NodeCanvas.Height = Image.Height;
      Img.Source = _image;
    }
  }

  private readonly int _nodeWidth = 50;

  private Node? _lastSelectedNode;
  public Node? LastSelectedNode
  {
    get => _lastSelectedNode;
    set
    {
      _lastSelectedNode = value;
      NodeCanvas.Children.OfType<Node>().ToList().ForEach(node => node.FillColor = node.DefaultColor);

      if (LastSelectedNode == null) return;
      LastSelectedNode.FillColor = Brushes.Green;
    }
  }

  public ImageControl()
  {
    InitializeComponent();
    InitializeDisplaceEvents();

    NodeCanvas.MouseUp += OnNodeCanvasMouseUp;
  }

  private void OnNodeCanvasMouseUp(object sender, MouseButtonEventArgs e)
  {
    if (e.Source is not Node && Mode != EditModes.MoveWindow)
      LastSelectedNode = null;

    if (Mode is not (EditModes.AddIntersection or EditModes.AddAccessPoints)) return;

    if (Mode is EditModes.AddIntersection)
    {
      if (e.Source is Node) return;
      AddNode(new IntNode(), e);
    }
    else if (Mode is EditModes.AddAccessPoints)
    {
      if (e.Source is Node) return;
      AddNode(new AccessPointNode(), e);
    }
  }
}
