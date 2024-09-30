using DAServer.Dtos;

using ShortestPathLib;

using System.Drawing;
using System.Text.Json;

namespace DAServer.Services;

public class MapService
{
    private string ContentPath = "Content\\";

    private List<IntNodeDto> _intNodes = [];

    public List<IntNodeDto> IntNodes
    {
        get => _intNodes;
        set
        {
            _intNodes = value;
            File.WriteAllText(@$"{ContentPath}\intnodes.json", JsonSerializer.Serialize(IntNodes));
        }
    }
    public List<IntNodeDto> TestIntNodes { get; set; } = [];

    private List<AccessPointDto> _accessPoints = [];


    public List<AccessPointDto> AccessPoints
    {
        get => _accessPoints; set
        {
            _accessPoints = value;
            File.WriteAllText(@$"{ContentPath}\apnodes.json", JsonSerializer.Serialize(AccessPoints));
        }
    }

    private string _imgBase64 = "";
    public string ImgBase64
    {
        get => _imgBase64;
        set
        {
            _imgBase64 = value;
            File.WriteAllText(@$"{ContentPath}\base64", ImgBase64);
        }
    }
    public MapService()
    {
        InitalizeTestData();
    }

    private void InitalizeTestData()
    {
        var intNode1 = new IntNodeDto() { Id = 0, Top = 0.1, Left = 0.1, };
        var intNode2 = new IntNodeDto() { Id = 1, Top = 0.3, Left = 0.2, };
        var intNode3 = new IntNodeDto() { Id = 2, Top = 0.5, Left = 0.7, };
        var intNode4 = new IntNodeDto() { Id = 3, Top = 0.9, Left = 0.9, };
        var intNode5 = new IntNodeDto() { Id = 4, Top = 0.7, Left = 0.5, };
        var intNode6 = new IntNodeDto() { Id = 5, Top = 0.2, Left = 0.8, };
        var intNode7 = new IntNodeDto() { Id = 6, Top = 0.4, Left = 0.3, };
        var intNode8 = new IntNodeDto() { Id = 7, Top = 0.6, Left = 0.6, };
        var intNode9 = new IntNodeDto() { Id = 8, Top = 0.2, Left = 0.1, };
        var intNode10 = new IntNodeDto() { Id = 9, Top = 0.4, Left = 0.4, };
        var intNode11 = new IntNodeDto() { Id = 10, Top = 0.6, Left = 0.7, };


        intNode1.ConnectedNodes.Add(intNode2.Id);
        intNode2.ConnectedNodes.Add(intNode1.Id);
        intNode2.ConnectedNodes.Add(intNode3.Id);
        intNode2.ConnectedNodes.Add(intNode6.Id);
        intNode3.ConnectedNodes.Add(intNode2.Id);
        intNode3.ConnectedNodes.Add(intNode4.Id);
        intNode4.ConnectedNodes.Add(intNode3.Id);
        intNode4.ConnectedNodes.Add(intNode9.Id);
        intNode4.ConnectedNodes.Add(intNode10.Id);
        intNode5.ConnectedNodes.Add(intNode6.Id);
        intNode6.ConnectedNodes.Add(intNode5.Id);
        intNode6.ConnectedNodes.Add(intNode7.Id);
        intNode6.ConnectedNodes.Add(intNode2.Id);
        intNode7.ConnectedNodes.Add(intNode6.Id);
        intNode8.ConnectedNodes.Add(intNode10.Id);
        intNode9.ConnectedNodes.Add(intNode4.Id);
        intNode10.ConnectedNodes.Add(intNode8.Id);
        intNode10.ConnectedNodes.Add(intNode11.Id);
        intNode11.ConnectedNodes.Add(intNode10.Id);

        TestIntNodes = [
          intNode1,
            intNode2,
            intNode3,
            intNode4,
            intNode5,
            intNode6,
            intNode7,
            intNode8,
            intNode9,
            intNode10,
            intNode11,
        ];
    }

    public List<SpfNode> GetNodesAsSpfList(List<IntNodeDto> nodeList)
    {
        var spfNodes = nodeList.Select(x => new SpfNode
        {
            Id = x.Id,
            X = x.Left,
            Y = x.Top,
        }).ToList();

        spfNodes.ForEach(spfNode =>
        {
            var connectedNodes = spfNodes.Where(node => nodeList.Find(x => x.Id == spfNode.Id).ConnectedNodes.Contains(node.Id)).ToList();
            spfNode.Neighbors = connectedNodes;
        });
        return spfNodes;
    }

}
