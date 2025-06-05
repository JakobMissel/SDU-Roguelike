using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using UnityEngine.InputSystem;
using System;
public class MapUI : MonoBehaviour
{
    [SerializeField] public Map MapData;
    [SerializeField] GameObject FloorPrefab;
    [SerializeField] GameObject NodePrefab;
    [SerializeField] GameObject LinePrefab;
    [SerializeField] GameObject LineContainer;
    [SerializeField] RectTransform CanvasRoot;
    PlayerInput playerInput;
    public static event Action<MapNode> UpdateActiveNodes;
    public static void OnUpdateActiveNodes(MapNode value) => UpdateActiveNodes?.Invoke(value);

    List<NodeUI> activeNodeUIs = new List<NodeUI>();
    int selectedNodeIndex;
    void Awake()
    {
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();
        activeNodeUIs = new List<NodeUI>();
    }
    void OnEnable()
    {
        if (playerInput == null)
            playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();

        UpdateActiveNodes += ChangeActiveNodes;
        playerInput.actions["MapSwitch"].performed += ctx => SwitchActiveNodeUI(ctx);
        playerInput.actions["Select"].performed += ctx => TriggerActiveNode();
        
    }
    void OnDisable()
    {
        UpdateActiveNodes -= ChangeActiveNodes;
        playerInput.actions["MapSwitch"].performed -= ctx => SwitchActiveNodeUI(ctx);
        playerInput.actions["Select"].performed -= ctx => TriggerActiveNode();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Canvas canvas = FindFirstObjectByType<Canvas>();
        CanvasRoot = canvas.GetComponent<RectTransform>();
        UpdateMapUI();
        selectedNodeIndex = 0;
        activeNodeUIs.Clear();
        ChangeActiveNodes(MapData.StartNode);
        ChangeHighlightedNode(0);
        
    }
    void SwitchActiveNodeUI(InputAction.CallbackContext context) {
        if (activeNodeUIs.Count == 0 || selectedNodeIndex >= activeNodeUIs.Count || activeNodeUIs[selectedNodeIndex] == null)
            return;
        float input = context.ReadValue<float>();
        if (activeNodeUIs.Count == 0) return;
        int index;

        // Cycle right
        if (input > 0) {
            index = (selectedNodeIndex + 1) % activeNodeUIs.Count;
            ChangeHighlightedNode(index);
        }
        // Cycle left
        else if (input < 0) {
            index = (selectedNodeIndex - 1 + activeNodeUIs.Count) % activeNodeUIs.Count;
            ChangeHighlightedNode(index);
        }
    }
    void TriggerActiveNode() {
        if (activeNodeUIs.Count == 0 || selectedNodeIndex >= activeNodeUIs.Count || activeNodeUIs[selectedNodeIndex] == null)
            return;
        var nodeUI = activeNodeUIs[selectedNodeIndex];
        if (nodeUI != null)
            nodeUI.OnClick();
    }

    void ChangeActiveNodes(MapNode node) {
        if (activeNodeUIs.Count != 0) {
            activeNodeUIs[selectedNodeIndex].GetComponent<NodeUI>().NodeData.SelectedNode = false;
        }
        activeNodeUIs.Clear();
        foreach (MapNode connectedNode in node.ConnectedNodes) {
            activeNodeUIs.Add(connectedNode.NodeUI);
        }
        selectedNodeIndex = 0;
        activeNodeUIs[selectedNodeIndex].GetComponent<NodeUI>().NodeData.SelectedNode = true;
    }
    void ChangeHighlightedNode(int index) {
        if (index < 0 || index >= activeNodeUIs.Count)
            return;

        activeNodeUIs = activeNodeUIs.Where(n => n != null).ToList();
        if (selectedNodeIndex >= 0 && selectedNodeIndex < activeNodeUIs.Count && activeNodeUIs[selectedNodeIndex] != null){
            activeNodeUIs[selectedNodeIndex].NodeData.SelectedNode = false;
        }

        selectedNodeIndex = index;

        if (selectedNodeIndex >= 0 && selectedNodeIndex < activeNodeUIs.Count && activeNodeUIs[selectedNodeIndex] != null) {
            activeNodeUIs[selectedNodeIndex].NodeData.SelectedNode = true;
        }
        // activeNodeUIs[selectedNodeIndex].GetComponent<NodeUI>().NodeData.SelectedNode = false;
        // selectedNodeIndex = index;
        // activeNodeUIs[selectedNodeIndex].GetComponent<NodeUI>().NodeData.SelectedNode = true;
        // activeNodeUIs[selectedNodeIndex].GetComponent<NodeUI>().Image.color = activeNodeUIs[selectedNodeIndex].ActiveColor;
    }

    public void UpdateMapUI()
    {
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
    void GenerateNodesUI(MapNode node)
    {
        if (node == null)
            return;
        var nodeGameObject = Instantiate(NodePrefab, transform.GetChild(node.Floor - 1));
        nodeGameObject.GetComponent<NodeUI>().NodeData = node;
        node.NodeUI = nodeGameObject.GetComponent<NodeUI>();
        if (node.Floor == MapData.Floors - 1 || node.Floor == MapData.Floors - 2)
            return;
        foreach (MapNode connectedNode in node.ConnectedNodes) {
            GenerateNodesUI(connectedNode);
        }
        Canvas.ForceUpdateCanvases();
    }
    void GenerateLinesUI()
    {
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Transform child = transform.GetChild(i);
            GameObject childGameObject = child.gameObject;
            for (int j = 0; j < child.childCount; j++)
            {
                var node = child.GetChild(j).GetComponent<NodeUI>();
                for (int k = 0; k < transform.GetChild(i + 1).childCount; k++)
                {
                    var nextNode = transform.GetChild(i + 1).GetChild(k).GetComponent<NodeUI>();
                    if (node.NodeData.ConnectedNodes.Contains(nextNode.NodeData))
                    {
                        GenerateLine(node.transform.GetChild(0).GetComponent<RectTransform>(), nextNode.transform.GetChild(0).GetComponent<RectTransform>());
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
