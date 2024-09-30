using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace EditorLib.Nodes;
public class AccessPointNode : Node
{
  public string MacAddress { get; set; } = "";

  public AccessPointNode() : base()
  {
    _propsToEdit = [.. _propsToEdit, nameof(MacAddress)];
    DefaultColor = Brushes.Chartreuse;
    FillColor = DefaultColor;
  }
}
