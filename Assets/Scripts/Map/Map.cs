using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using Unity.Behavior;
using System;
public class Map : MonoBehaviour
{
    public MapNode LastCompletedNode;
    public MapNode StartNode;
    public GameObject LineContainer;
    [SerializeField] public int Floors;
    [SerializeField] int StartingBranches;
    [SerializeField] int BranchChance;
    [SerializeField] MapUI MapUI;
    public MapNode EndNode;
    public MapNode HealingNode;
    public static event Action<MapNode> CompleteNode;
    public static void OnCompleteNode(MapNode value) => CompleteNode?.Invoke(value);

    void OnEnable()
    {
        CompleteNode += ChangeActiveNodes;
    }
    void OnDisable()
    {
        CompleteNode -= ChangeActiveNodes;
    }
    void Awake()
    {
        MapUI = GetComponent<MapUI>();
        GenerateMap();
    }
    void ChangeActiveNodes(MapNode node)
    {
        if (node.Type == NodeType.Boss)
        {
            CreateNewMap.OnCreateMap();
            return;
        }
        foreach (var previousNodes in LastCompletedNode.ConnectedNodes)
        {
            previousNodes.ActiveNode = false;
        }
        node.Completed = true;
        LastCompletedNode = node;
        foreach (var nextNodes in LastCompletedNode.ConnectedNodes)
        {
            nextNodes.ActiveNode = true;
        }
        if (node != StartNode)
            MapUI.OnUpdateActiveNodes(node);
    }

    void GenerateMap()
    {
        MapUI = GetComponent<MapUI>();
        MapUI.MapData = this;
        EndNode = new MapNode(NodeType.Boss, Floors);
        StartNode = new MapNode(NodeType.Currency, 1);
        HealingNode = new MapNode(NodeType.Healing, Floors - 1);
        HealingNode.ConnectedNodes = new List<MapNode>() { EndNode };
        List<NodeType> weightedList = new List<NodeType>();
        weightedList.AddRange(Enumerable.Repeat(NodeType.Currency, (int)NodeType.Currency - 1));
        weightedList.AddRange(Enumerable.Repeat(NodeType.Healing, (int)NodeType.Healing - 1));
        weightedList.AddRange(Enumerable.Repeat(NodeType.Upgrade, (int)NodeType.Upgrade));
        weightedList.AddRange(Enumerable.Repeat(NodeType.Talisman, (int)NodeType.Talisman));
        StartNode.ConnectedNodes = Generatebranches(weightedList, 2, StartingBranches);
        LastCompletedNode = StartNode;
        ChangeActiveNodes(LastCompletedNode);
    }

    List<MapNode> Generatebranches(List<NodeType> weightedList, int floor, int branches)
    {
        var connections = new List<MapNode>();

        if (floor < Floors - 1)
        {
            for (int i = 0; i < branches; i++)
            {
                connections.Add(GeneratoNode(weightedList, floor));
                connections[i].ConnectedNodes = Generatebranches(weightedList, floor + 1, UnityEngine.Random.Range(0, 100) < BranchChance ? 2 : 1);
            }
        }
        else
        {
            connections.Add(HealingNode);
        }
        return connections;
    }
    MapNode GeneratoNode(List<NodeType> weightedList, int floor)
    {
        NodeType type;
        if (floor == 2)
        {
            type = NodeType.Upgrade;
            weightedList.RemoveAt(weightedList.IndexOf(NodeType.Upgrade));
        }
        else
        {
            int selected = UnityEngine.Random.Range(0, weightedList.Count);
            type = weightedList[selected];
            weightedList.RemoveAt(selected);
        }
        return new MapNode(type, floor);
    }
    void DestroyMap()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in LineContainer.transform)
        {
            Destroy(child.gameObject);
        }
        StartNode = null;
        EndNode = null;
        HealingNode = null;
        LastCompletedNode = null;
    }
}
