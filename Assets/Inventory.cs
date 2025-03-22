using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public string _itemName;
    public int _itemQuantity;
    public Sprite _iconTexture;

    public int _inventorySize;
    public int _currentInventorySize;

    public UI_Inventory _uiAni;
    public static Inventory _invInstance;
    public List<GameObject> _invItems = new List<GameObject>();
    void Start()
    {
        _uiAni = FindFirstObjectByType<UI_Inventory>();
        if (_uiAni == null)
        {
            Debug.LogError("UI_Animation not found in the scene!");
        }
    }
    public void AddItem(GameObject itemPrefab)
    {
        if (!_invItems.Contains(itemPrefab) && itemPrefab != null)
        {
            _invItems.RemoveAll(item => item == null);
            _invItems.Add(itemPrefab);
            Debug.Log($"Added {itemPrefab.GetComponent<Item>()._itemName} to inventory!");
            _uiAni.UpdateUI();
        }
        else if (_invItems.Contains(itemPrefab) && itemPrefab.GetComponent<Item>()._itemQuantity < itemPrefab.GetComponent<Item>()._itemMaxQuantity)
        {
            itemPrefab.GetComponent<Item>()._itemQuantity++;
        }
        else
        {
            Debug.Log("Item already in inventory.");
        }
    }
    public void RemoveItem(GameObject itemPrefab)
    {
        _invItems.Remove(itemPrefab);
        Debug.Log($"Removed {itemPrefab.GetComponent<Item>()._itemName} from inventory!");
        if (_uiAni != null)
        {
            _uiAni.UpdateUI();
        }
    }
    private void Update()
    {
        Debug.Log(string.Join(", ", _invItems));
    }
}
