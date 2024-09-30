
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EditorLib.Nodes;
public partial class Node
{
  protected bool _isDragging;
  private Point _clickPosition;

  public EventHandler<NodeEventArgs>? StartMoving;
  public EventHandler<NodeEventArgs>? EndMoving;

  public void InitializeDraggable()
  {
    MouseDoubleClick += StartDragging;
    PreviewMouseLeftButtonDown += StartDragging;
    PreviewMouseLeftButtonUp += EndDragging;
    MouseMove += MoveDragging;
  }

  private void StartDragging(object sender, MouseButtonEventArgs e)
  {
    if (ImageControl.Mode != EditModes.MoveNodes) return;

    _isDragging = true;
    var draggableControl = (sender as UserControl)!;
    _clickPosition = e.GetPosition(this);
    draggableControl.CaptureMouse();

    StartMoving?.Invoke(this, new NodeEventArgs(this));
  }

  private void EndDragging(object sender, MouseButtonEventArgs e)
  {
    if (ImageControl.Mode != EditModes.MoveNodes) return;

    _isDragging = false;
    var draggable = (sender as UserControl)!;
    draggable.ReleaseMouseCapture();

    EndMoving?.Invoke(this, new NodeEventArgs(this));
  }

  private void MoveDragging(object sender, MouseEventArgs e)
  {
    if (ImageControl.Mode != EditModes.MoveNodes || !_isDragging || sender is not UserControl) return;
    
    Point currentPosition = e.GetPosition(Parent as UIElement);

    Canvas.SetLeft(this, currentPosition.X - NodeWidth / 2);
    Canvas.SetTop(this, currentPosition.Y - NodeWidth / 2);
  }
}
