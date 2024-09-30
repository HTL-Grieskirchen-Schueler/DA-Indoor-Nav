using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorLib.Nodes.Dtos;
public class NodeDto
{
  public int Id { get; set; }
  public string Name { get; set; } = "";
  public double Left { get; set; }
  public double Top { get; set; }
}
