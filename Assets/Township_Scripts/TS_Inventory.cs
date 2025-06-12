using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TS_Inventory : MonoBehaviour
{
    public static TS_Inventory _inv;
    GameObject _invPanel;
    Transform _invPanelParent;
    Placement _placement;
    void Awake()
    {
        _invPanelParent = GameObject.Find("UI/Inventory/Panel")?.GetComponent<Transform>();
        _canvas = GameObject.Find("UI/Inventory")?.GetComponent<Canvas>();
        _invPanel = Resources.Load<GameObject>("Inventory Item");
    }

    void Start()
    {
        _inv = this;
        _placement = GameObject.Find("Ground")?.GetComponent<Placement>();
        _canvas.enabled = false;
    }
    Dictionary<string, GameObject> _uiPanel = new Dictionary<string, GameObject>();

    public void AddItem(string name, int quantity)
    {
        InventoryItem existingItem = GameData._instance._invI.Find(item => item.name == name);
        if (_uiPanel.ContainsKey(name))
        {
            existingItem.quantity += quantity;
            _uiPanel[name].transform.Find("Quantity").GetComponent<TextMeshProUGUI>().text = existingItem.quantity.ToString();
        }
        else
        {
            GameObject UIPanel = Instantiate(_invPanel, _invPanelParent);
            UIPanel.transform.Find("Quantity").GetComponent<TextMeshProUGUI>().text = quantity.ToString();
            UIPanel.transform.Find("Inventory Panel").Find("Name").GetComponent<TextMeshProUGUI>().text = name;
            UIPanel.transform.Find("Inventory Panel").Find("Item").GetComponent<Image>().sprite = existingItem.icon;
            UIPanel.GetComponent<Image>().sprite = existingItem.icon;
            _uiPanel.Add(name, UIPanel);  
        }
    }
    public void RemoveItem(string name, int quantity)
    {
        InventoryItem existingItem = GameData._instance._invI.Find(item => item.name == name);
        if (_uiPanel.ContainsKey(name))
        {
            if (existingItem.quantity <= 0) _uiPanel.Remove(name);
            else
            {
                existingItem.quantity -= quantity;
                _uiPanel[name].transform.Find("Quantity").GetComponent<TextMeshProUGUI>().text = existingItem.quantity.ToString();
            }
        }
    }
    void Update()
    {
        AutoClose();
    }
    void OnMouseUp()
    {
        if (!_canvas.enabled && !EventSystem.current.IsPointerOverGameObject() && !_placement._editBuild)
        {
            _canvas.enabled = true;
            wasOpenedThisFrame = true;
        }
    }
    Canvas _canvas;
    bool wasOpenedThisFrame;
    void AutoClose()
    {
        if (_canvas.enabled)
        {
            if (wasOpenedThisFrame)
            {
                wasOpenedThisFrame = false;
                return;
            }
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                _canvas.enabled = false;
            }
        }
    }
    public void ManualClose()
    {
        _canvas.enabled = false;
    }
    void Plus()
    {

    }
    void Minus()
    {

    }
}
