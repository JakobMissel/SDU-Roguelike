using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using Unity.Behavior;
using System;
public class Map : MonoBehaviour {
    public MapNode LastCompletedNode;
    public MapNode StartNode;
    [SerializeField] List<MapNode> Nodes;
    [SerializeField] public int Floors;
    [SerializeField] int StartingBranches;
    [SerializeField] int BranchChance;
    public MapNode EndNode;
    public MapNode HealingNode;
    public static event Action<MapNode> CompleteNode;
    public static void OnCompleteNode(MapNode value) => CompleteNode?.Invoke(value);

    void OnEnable() {
        CompleteNode += ChangeActiveNodes;
    }
    void OnDisable() {
        CompleteNode -= ChangeActiveNodes;
    }
    void Start() {
        EndNode = new MapNode(NodeType.Boss, Floors);
        StartNode = new MapNode(NodeType.Currency, 1);
        HealingNode = new MapNode(NodeType.Healing, Floors-1);
        HealingNode.ConnectedNodes = new List<MapNode>() {EndNode};
        GenerateMap();
    }
    void ChangeActiveNodes(MapNode node){
        
        foreach (var previousNodes in LastCompletedNode.ConnectedNodes) {
            previousNodes.ActiveNode = false;
        }
        node.Completed = true;
        LastCompletedNode = node;
        foreach (var nextNodes in LastCompletedNode.ConnectedNodes) {
            nextNodes.ActiveNode = true;
        }
    }

    void GenerateMap(){
        List<NodeType> weightedList = new List<NodeType>();
        //make a list of each
        weightedList.AddRange(Enumerable.Repeat(NodeType.Currency, (int)NodeType.Currency-1));
        weightedList.AddRange(Enumerable.Repeat(NodeType.Healing, (int)NodeType.Healing-1));
        weightedList.AddRange(Enumerable.Repeat(NodeType.Upgrade, (int)NodeType.Upgrade));
        weightedList.AddRange(Enumerable.Repeat(NodeType.Talisman, (int)NodeType.Talisman));
        StartNode.ConnectedNodes = Generatebranches(weightedList, 2, 3);
        LastCompletedNode = StartNode;
        ChangeActiveNodes(LastCompletedNode);
    }

    List<MapNode> Generatebranches(List<NodeType> weightedList, int floor, int branches) {
        var connections = new List<MapNode>();

        if (floor < Floors-1){
            for (int i = 0; i < branches; i++) {
                connections.Add(GeneratoNode(weightedList, floor));
                connections[i].ConnectedNodes = Generatebranches(weightedList, floor+1, UnityEngine.Random.Range(0, 100) < BranchChance ? 2 : 1);
            }
        } else {
            connections.Add(HealingNode);
        }
        return connections;
    }
    MapNode GeneratoNode(List<NodeType> weightedList, int floor){
        NodeType type;
        if (floor == 2) {
            type = NodeType.Upgrade;
            weightedList.RemoveAt(weightedList.IndexOf(NodeType.Upgrade));
        } else {
            int selected = UnityEngine.Random.Range(0, weightedList.Count);
            type = weightedList[selected];
            weightedList.RemoveAt(selected);
        }
        return new MapNode(type, floor);
    }
}
