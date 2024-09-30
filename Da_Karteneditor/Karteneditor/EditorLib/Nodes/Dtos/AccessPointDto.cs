using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EditorLib.Nodes.Dtos;
[Serializable]
public class AccessPointDto : NodeDto
{
  public string MacAddress { get; set; } = "";
}
