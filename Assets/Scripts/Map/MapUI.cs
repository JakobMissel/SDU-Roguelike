using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapUI : MonoBehaviour {
    [SerializeField] public Map MapData;
    [SerializeField] GameObject FloorPrefab;
    [SerializeField] GameObject NodePrefab;
    [SerializeField] GameObject LinePrefab;
    [SerializeField] GameObject LineContainer;
    [SerializeField] RectTransform CanvasRoot;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        CanvasRoot = canvas.GetComponent<RectTransform>();
        UpdateMapUI();
    }

    public void UpdateMapUI() {
        Canvas.ForceUpdateCanvases();
        for (int i = 0; i < MapData.Floors; i++)
        {
            Instantiate(FloorPrefab, transform);
        }
        GenerateNodesUI(MapData.StartNode);
        // MapNode currentNode = MapData.StartNode;
        GenerateNodesUI(MapData.HealingNode);
        GenerateNodesUI(MapData.EndNode);
        GenerateLinesUI();
    }

    void GenerateNodesUI(MapNode node) {
        if (node == null)
            return;
        var nodeGameObject = Instantiate(NodePrefab, transform.GetChild(node.Floor-1));
        nodeGameObject.GetComponent<NodeUI>().NodeData = node;
        if (node.Floor == MapData.Floors-1 || node.Floor == MapData.Floors-2)
            return;
        foreach (MapNode connectedNode in node.ConnectedNodes) {
            GenerateNodesUI(connectedNode);
        }
        Canvas.ForceUpdateCanvases();
    }
    void GenerateLinesUI() {
        for (int i = 0; i < transform.childCount-1; i++) {
            Transform child = transform.GetChild(i);
            GameObject childGameObject = child.gameObject;
            for (int j = 0 ; j < child.childCount; j++){
                var node = child.GetChild(j).GetComponent<NodeUI>();
                for (int k = 0 ; k < transform.GetChild(i+1).childCount; k++){
                    var nextNode = transform.GetChild(i+1).GetChild(k).GetComponent<NodeUI>();
                    if (node.NodeData.ConnectedNodes.Contains(nextNode.NodeData)){
                        GenerateLine(node.transform.GetChild(0).GetComponent<RectTransform>(),nextNode.transform.GetChild(0).GetComponent<RectTransform>());
                    }
                }
            }
        }
    }
    void GenerateLine(RectTransform start, RectTransform end)
    {
        var lineGO = Instantiate(LinePrefab, LineContainer.transform);
        UILine line = lineGO.AddComponent<UILine>();
        line.StartObject = start;
        line.EndObject = end;
        line.Initialize();
    }

}
