using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using Unity.Behavior;
public class Map : MonoBehaviour {
    [SerializeField] MapNode CurrentNode;
    [SerializeField] MapNode StartNode;
    
    [SerializeField] List<MapNode> Nodes;
    [SerializeField] int Floors;
    [SerializeField] int StartingBranches;
    [SerializeField] int BranchChance;
    MapNode EndNode;
    void Start() {
        EndNode = new MapNode(NodeType.Boss, Floors);
        StartNode = new MapNode(NodeType.Currency, 1);
        GenerateMap();
    }

    void GenerateMap(){
        List<NodeType> weightedList = new List<NodeType>();
        //make a list of each
        weightedList.AddRange(Enumerable.Repeat(NodeType.Currency, (int)NodeType.Currency-1));
        weightedList.AddRange(Enumerable.Repeat(NodeType.Healing, (int)NodeType.Healing));
        weightedList.AddRange(Enumerable.Repeat(NodeType.Upgrade, (int)NodeType.Upgrade));
        weightedList.AddRange(Enumerable.Repeat(NodeType.Talisman, (int)NodeType.Talisman));
        StartNode.ConnectedNodes = Generatebranches(weightedList, 2, 3);
    }

    List<MapNode> Generatebranches(List<NodeType> weightedList, int floor, int branches) {
        var connections = new List<MapNode>();

        if (floor < Floors){
            for (int i = 0; i < branches; i++) {
                connections.Add(GeneratoNode(weightedList, floor));
                connections[i].ConnectedNodes = Generatebranches(weightedList, floor+1, Random.Range(0, 100) < BranchChance ? 2 : 1);
            }
        } else {
            connections.Add(EndNode);
        }
        return connections;
    }
    MapNode GeneratoNode(List<NodeType> weightedList, int floor){
        NodeType type;
        if (floor == 2) {
            type = NodeType.Upgrade;
            weightedList.RemoveAt(weightedList.IndexOf(NodeType.Upgrade));
        } else if (floor == Floors-2) {
            type = NodeType.Healing;
            weightedList.RemoveAt(weightedList.IndexOf(NodeType.Healing));
        } else {
            int selected = UnityEngine.Random.Range(0, weightedList.Count);
            type = weightedList[selected];
            weightedList.RemoveAt(selected);
        }
        return new MapNode(type, floor);
    }
    // bool _endDrawn = false;

    // void OnDrawGizmos() {
    //     if (StartNode == null) return;
    //     _endDrawn = false;
    //     DrawNode(StartNode, 0f, 1);
    // }

    // void DrawNode(MapNode node, float x, int floor, float spread = 4f) {
    //     if (node == null) return;

    //     // 1) Compute this node's world‐space position:
    //     float y = (floor - 1) * 2f;        // floor=1 -> y=0, floor=2->y=2, etc.
    //     Vector3 pos = new Vector3(x, y, 0);

    //     // 2) Draw it:
    //     Gizmos.color = Color.yellow;
    //     Gizmos.DrawSphere(pos, 0.25f);

    //     // 3) Lay out children horizontally around 'x':
    //     if (node.ConnectedNodes == null || node.ConnectedNodes.Count == 0)
    //         return;
    //     int count = node.ConnectedNodes.Count;
    //     float startX = x - (count - 1) * spread * 0.5f;

    //     for (int i = 0; i < count; i++) {
    //         var child = node.ConnectedNodes[i];

    //         // 3a) Special case: if this child is your singular EndNode:
    //         if (child == EndNode) {
    //             // draw EndNode once at the top:
    //             float yEnd = (Floors - 1) * 2f;
    //             Vector3 endPos = new Vector3(0, yEnd, 0);
    //             if (!_endDrawn) {
    //                 Gizmos.color = Color.red;
    //                 Gizmos.DrawSphere(endPos, 0.3f);
    //                 _endDrawn = true;
    //             }
    //             // draw line from this node → EndNode
    //             Gizmos.color = Color.yellow;
    //             Gizmos.DrawLine(pos, endPos);
    //             continue;
    //         }

    //         // 3b) Normal node: compute its x and y:
    //         float childX = startX + i * spread;
    //         float childY = (floor) * 2f; // floor+1 -1 = floor
    //         Vector3 childPos = new Vector3(childX, childY, 0);

    //         // draw line and recurse:
    //         Gizmos.color = Color.yellow;
    //         Gizmos.DrawLine(pos, childPos);
    //         DrawNode(child, childX, floor + 1, spread * 0.8f);
    //     }
    // }
}
