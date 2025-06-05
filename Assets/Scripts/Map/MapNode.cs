using System.Collections.Generic;
using UnityEngine;

public enum NodeType {
    Currency = 60,
    Healing = 11,
    Upgrade = 20,
    Talisman = 9,
    Boss = 0,
}
public class MapNode {

    [SerializeField] public NodeType Type;
    [SerializeField] public List<MapNode> ConnectedNodes;
    [SerializeField] public bool ActiveNode;
    [SerializeField] public bool Completed;
    [SerializeField] public bool SelectedNode;
    [SerializeField] MapUI MapUI;
    public NodeUI NodeUI;
    public int Floor;
    public int ID;
    public static int CURRENTID;

    public MapNode(NodeType type, int floor) {
        this.Type = type;
        this.Floor = floor;
        if (floor == 2) {
            ActiveNode = true;
        } else {
            ActiveNode = false;
        }
        ConnectedNodes = new List<MapNode>();
        CURRENTID += 1;
        ID = CURRENTID;
    }
}
