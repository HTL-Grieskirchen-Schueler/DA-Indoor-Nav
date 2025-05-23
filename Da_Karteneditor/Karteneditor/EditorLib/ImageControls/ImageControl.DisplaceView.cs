﻿using System.Windows.Controls;
using System.Windows.Input;
using System.Windows;

namespace EditorLib;
public partial class ImageControl
{
  Point? lastCenterPositionOnTarget;
  Point? lastMousePositionOnTarget;
  Point? lastDragPoint;

  public void InitializeDisplaceEvents()
  {
    scrollViewer.ScrollChanged += OnScrollViewerScrollChanged;
    scrollViewer.MouseLeftButtonUp += OnMouseLeftButtonUp;
    scrollViewer.PreviewMouseLeftButtonUp += OnMouseLeftButtonUp;
    scrollViewer.PreviewMouseWheel += OnPreviewMouseWheel;

    scrollViewer.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
    scrollViewer.MouseMove += OnMouseMove;

    scrollViewer.PreviewMouseDown += OnMouseDown;
    scrollViewer.PreviewMouseUp += OnMouseUp;

    slider.ValueChanged += OnSliderValueChanged;
  }

  private void OnMouseDown(object sender, MouseButtonEventArgs e)
  {
    if (e.ChangedButton == MouseButton.Middle)
      StartMovingCanvas(e);
  }

  private void OnMouseUp(object sender, MouseButtonEventArgs e)
  {
    if (e.ChangedButton == MouseButton.Middle)
      EndMovingCanvas();
  }

  private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
  {
    if (Mode == EditModes.MoveWindow)
      StartMovingCanvas(e);
  }

  private void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
  {
    if (Mode == EditModes.MoveWindow)
      EndMovingCanvas();
  }

  private void StartMovingCanvas(MouseButtonEventArgs e)
  {
    var mousePos = e.GetPosition(scrollViewer);
    if (mousePos.X <= scrollViewer.ViewportWidth && mousePos.Y < scrollViewer.ViewportHeight)
    {
      scrollViewer.Cursor = Cursors.SizeAll;
      lastDragPoint = mousePos;
      Mouse.Capture(scrollViewer);
    }
  }

  private void EndMovingCanvas()
  {
    scrollViewer.Cursor = Cursors.Arrow;
    scrollViewer.ReleaseMouseCapture();
    lastDragPoint = null;
  }

  private void OnMouseMove(object sender, MouseEventArgs e)
  {
    if (lastDragPoint.HasValue)
    {
      Point posNow = e.GetPosition(scrollViewer);

      double dX = posNow.X - lastDragPoint.Value.X;
      double dY = posNow.Y - lastDragPoint.Value.Y;

      lastDragPoint = posNow;

      scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - dX);
      scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - dY);
    }
  }

  private void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
  {
    lastMousePositionOnTarget = Mouse.GetPosition(grid);

    if (e.Delta > 0)
      slider.Value += 1;
    if (e.Delta < 0)
      slider.Value -= 1;

    e.Handled = true;
  }

  private void OnSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
  {
    scaleTransform.ScaleX = e.NewValue;
    scaleTransform.ScaleY = e.NewValue;

    var centerOfViewport = new Point(scrollViewer.ViewportWidth / 2,
                                     scrollViewer.ViewportHeight / 2);
    lastCenterPositionOnTarget = scrollViewer.TranslatePoint(centerOfViewport, grid);
  }

  private void OnScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
  {
    if (e.ExtentHeightChange != 0 || e.ExtentWidthChange != 0)
    {
      Point? targetBefore = null;
      Point? targetNow = null;

      if (!lastMousePositionOnTarget.HasValue)
      {
        if (lastCenterPositionOnTarget.HasValue)
        {
          var centerOfViewport = new Point(scrollViewer.ViewportWidth / 2,
                                           scrollViewer.ViewportHeight / 2);
          Point centerOfTargetNow = scrollViewer.TranslatePoint(centerOfViewport, grid);

          targetBefore = lastCenterPositionOnTarget;
          targetNow = centerOfTargetNow;
        }
      }
      else
      {
        targetBefore = lastMousePositionOnTarget;
        targetNow = Mouse.GetPosition(grid);

        lastMousePositionOnTarget = null;
      }

      if (targetBefore.HasValue)
      {
        double dXInTargetPixels = targetNow.Value.X - targetBefore.Value.X;
        double dYInTargetPixels = targetNow.Value.Y - targetBefore.Value.Y;

        double multiplicatorX = e.ExtentWidth / grid.Width;
        double multiplicatorY = e.ExtentHeight / grid.Height;

        double newOffsetX = scrollViewer.HorizontalOffset - dXInTargetPixels * multiplicatorX;
        double newOffsetY = scrollViewer.VerticalOffset - dYInTargetPixels * multiplicatorY;

        if (double.IsNaN(newOffsetX) || double.IsNaN(newOffsetY)) return;

        scrollViewer.ScrollToHorizontalOffset(newOffsetX);
        scrollViewer.ScrollToVerticalOffset(newOffsetY);
      }
    }
  }
}
