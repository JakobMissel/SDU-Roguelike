using System.Collections.Generic;
using UnityEngine;

public enum NodeType {
    Currency = 60,
    Healing = 15,
    Upgrade = 20,
    Talisman = 5,
    Boss = 0,
}
public class MapNode {

    [SerializeField] public NodeType Type;
    [SerializeField] public List<MapNode> ConnectedNodes;
    [SerializeField] public bool ActiveNode;
    [SerializeField] public bool Completed;
    public int Floor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        
    }

    public MapNode(NodeType type, int floor) {
        this.Type = type;
        this.Floor = floor;
        ActiveNode = false;
        ConnectedNodes = new List<MapNode>();    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
