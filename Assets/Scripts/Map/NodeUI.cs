using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using System;
public class NodeUI : MonoBehaviour {
    public MapNode NodeData;
    [SerializeField] Button Button;
    [SerializeField] Image Image;
    [SerializeField] Image IconImage;
    [SerializeField] List<Sprite> Sprites;
    [SerializeField] Color DefaultColor;
    [SerializeField] Color ActiveColor;
    [SerializeField] Color CompletedColor;
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

    void UpdateUI(){
        if (NodeData.Completed) {
            Image.color = CompletedColor;
            Button.enabled = false;
            return;
        }
        if (NodeData.ActiveNode){
            Image.color = ActiveColor;
            Button.enabled = true;
            return;
        }
        Image.color = DefaultColor;
        Button.enabled = false;
    }
    public void OnClick(){
        Map.OnCompleteNode(NodeData);
    }
}
