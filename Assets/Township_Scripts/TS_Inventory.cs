using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class InventoryItem
{
    public string name;
    public int quantity;
    public float price;
    public Sprite icon;

    public InventoryItem(string name, int quantity, float price, Sprite icon)
    {
        this.name = name;
        this.quantity = quantity;
        this.price = price;
        this.icon = icon;
    }
}
public class TS_Inventory : MonoBehaviour
{
    public GameObject _invPanel;
    public Transform _invPanelParent;
    void Start()
    {
        _canvas.enabled = false;
    }
    List<InventoryItem> _items  = new List<InventoryItem>();
    [SerializeField] public List<InventoryItem> items = new List<InventoryItem>();
    Dictionary<string, GameObject> _uiPanel = new Dictionary<string, GameObject>();

    public void AddItem(string name, int quantity)
    {
        InventoryItem existingItem = _items.Find(item => item.name == name);
        InventoryItem _refItem = items.Find(item => item.name == name);
        if (existingItem != null)
        {
            existingItem.quantity += quantity;
            _uiPanel.TryGetValue(name, out GameObject UIPanel);
            UIPanel.transform.Find("Quantity").GetComponent<TextMeshProUGUI>().text = existingItem.quantity.ToString();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(name, quantity, _refItem.price, _refItem.icon);
            GameObject UIPanel = Instantiate(_invPanel, _invPanelParent);
            UIPanel.transform.Find("Quantity").GetComponent<TextMeshProUGUI>().text = quantity.ToString();
            UIPanel.transform.Find("Inventory Panel").Find("Name").GetComponent<TextMeshProUGUI>().text = name;
            UIPanel.transform.Find("Inventory Panel").Find("Item").GetComponent<Image>().sprite = _refItem.icon;
            _items.Add(newItem);
            _uiPanel.Add(name, UIPanel);
            
        }
    }
    void Update()
    {
        AutoClose();
    }
    void OnMouseDown()
    {
        if (!_canvas.enabled && !EventSystem.current.IsPointerOverGameObject())
        {
            _canvas.enabled = true;
            wasOpenedThisFrame = true;
        }
    }
    public Canvas _canvas;
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
