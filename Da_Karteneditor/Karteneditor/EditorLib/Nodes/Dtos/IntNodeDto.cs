using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorLib.Nodes.Dtos;
[Serializable]
public class IntNodeDto : NodeDto
{
  public List<int> ConnectedNodes { get; set; } = [];
}
