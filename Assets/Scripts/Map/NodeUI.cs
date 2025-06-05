using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using System;
using Unity.Behavior;
public class NodeUI : MonoBehaviour {
    public MapNode NodeData;
    [SerializeField] Button Button;
    public Image Image;
    [SerializeField] Image IconImage;
    [SerializeField] List<Sprite> Sprites;
    public Color DefaultColor;
    public  Color ActiveColor;
    public  Color CompletedColor;
    public  Color HighlightedColor;
    [SerializeField] NodeType type;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        type = NodeData.Type;
        switch (NodeData.Type) {
            case NodeType.Currency:
                IconImage.sprite = Sprites[0];
                break;
            case NodeType.Healing:
                IconImage.sprite = Sprites[1];
                break;
            case NodeType.Upgrade:
                IconImage.sprite = Sprites[2];
                break;
            case NodeType.Talisman:
                IconImage.sprite = Sprites[3];
                break;
            case NodeType.Boss:
                IconImage.sprite = Sprites[4];
                break;
        }
    }
    void Update() {
        UpdateUI();
    }
    // void start() {
    //     UpdateUI(NodeData);
    // }
    // void OnEnable() {
    //     Map.CompleteNode += UpdateUI;
    // }
    // void OnDisable() {
    //     Map.CompleteNode -= UpdateUI;
    // }

    void UpdateUI() {
        if (NodeData.Completed) {
            Image.color = CompletedColor;
            Button.enabled = false;
            return;
        }
        if (NodeData.SelectedNode){
            Image.color = HighlightedColor;
            Button.enabled = true;
            return;
        }
        if (NodeData.ActiveNode)
        {
            Image.color = ActiveColor;
            Button.enabled = true;
            return;
        }
        Image.color = DefaultColor;
        Button.enabled = false;
    }
    public void OnClick()
    {
        Map.OnCompleteNode(NodeData);
        WarpToNextFloor.OnWarpToFloor((NodeType)NodeData.Floor);
        ToggleMap.OnToggleMapUI(false);
    }
}
