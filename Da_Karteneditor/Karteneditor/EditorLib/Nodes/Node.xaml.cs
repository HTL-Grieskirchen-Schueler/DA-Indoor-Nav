using EditorLib.Nodes.EditNode;

using EditorUserControll;

using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EditorLib.Nodes;
/// <summary>
/// Interaction logic for Node.xaml
/// </summary>
public abstract partial class Node : UserControl
{
  public int Id { get; set; }

  public double Left { get; set; }
  public double Top { get; set; }


  public int NodeWidth { get; set; }

  public string NodeName { get => lblNodeName.Content.ToString() ?? ""; set => lblNodeName.Content = value; }

  protected List<string> _propsToEdit = [nameof(NodeName)];

  public Brush DefaultColor { get; init; } = Brushes.Black;

  public Brush FillColor { get => Ellipse.Fill; set => Ellipse.Fill = value; }

  public record class NodeEventArgs(Node EventNode);

  public EventHandler<NodeEventArgs>? NodeClicked;

  public Node()
  {
    InitializeComponent();
    InitializeDraggable();
  }

  private void UserControl_Loaded(object sender, RoutedEventArgs e)
  {
    Width = NodeWidth;
    Height = NodeWidth + spDescriptions.Height;

    Ellipse.Width = NodeWidth;
    Ellipse.Height = NodeWidth;
  }

  public virtual void OnNodeBtnClicked(object sender, RoutedEventArgs e) => NodeClicked?.Invoke(this, new NodeEventArgs(this));

  public virtual void OnNodeBtnDblClick(object sender, MouseButtonEventArgs e)
  {
    if (ImageControl.Mode != EditModes.EditNodes) return;
    EditNode();
  }

  public void EditNode()
  {
    var editNodeWindow = new EditNodeWindow(_propsToEdit.ToDictionary(prop => prop, prop => GetType().GetProperties().Single(x => x.Name == prop).GetValue(this)?.ToString()));
    editNodeWindow.ShowDialog();

    if (!editNodeWindow.ApplyChanges) return;

    editNodeWindow.PropValues.ToList().ForEach(prop =>
    {
      GetType().InvokeMember(prop.Key,
        BindingFlags.Instance | BindingFlags.Public | BindingFlags.SetProperty,
        Type.DefaultBinder, this, [prop.Value]);
    });
  }

  public override string ToString() => $"{Id}: {NodeName}";
}
