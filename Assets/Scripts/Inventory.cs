using NUnit.Framework.Interfaces;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    UI_Inventory _uiAni;
    public static Inventory _invInstance;
    [Header("Inventory Objects")]
    [Tooltip("Add all the Pickable Objects")]
    [SerializeField] List<GameObject> _itemData = new List<GameObject>();
    [HideInInspector] public List<GameObject> _invItems = new List<GameObject>();

    void Start()
    {
        _invInstance = this;
        _uiAni = FindFirstObjectByType<UI_Inventory>();
        foreach (GameObject itemObj in _itemData)
        {
            Item item = itemObj.GetComponent<Item>();
            item._itemQuantity = 0;
        }
    }

    public void AddItem(GameObject itemPrefab)
    {
        if (itemPrefab)
        {
            _invItems.RemoveAll(item => !item);
            foreach (GameObject Obj in _itemData)
            {
                Item _item1 = Obj.GetComponent<Item>();
                Item _item2 = itemPrefab.GetComponent<Item>();
                if (_item1._itemName == _item2._itemName && _item1._itemQuantity == 0)
                {
                    if(_item2._itemAttribute == "Key")
                    {
                        GameManager._hasKey = true;
                    }
                    _invItems.Add(Obj);
                    _item1._itemQuantity++;
                    _uiAni.UpdateUI();
                    break;
                }
                else if (_item1._itemName == _item2._itemName && _item1._itemQuantity < _item1._itemMaxQuantity)
                {
                    _item1._itemQuantity++;
                    _uiAni.UpdateUI();
                }
                else if (_item1._itemName == _item2._itemName && _item1._itemQuantity >= _item1._itemMaxQuantity)
                {
                    Debug.Log("Item already in inventory.");
                }
            }
        }
    }

    public void RemoveItem(GameObject itemPrefab)
    {
        Item _item = itemPrefab.GetComponent<Item>();
        if (_item._itemQuantity == 1)
        {
            _item._itemQuantity--;
            _invItems.Remove(itemPrefab);
            Debug.Log($"Removed {itemPrefab.GetComponent<Item>()._itemName} from inventory!");
        }
        else if(_item._itemQuantity > 1)
        {
            _item._itemQuantity--;
            Debug.Log($"Removed {itemPrefab.GetComponent<Item>()._itemName} from inventory!");
        }
        if (_uiAni)
        {
            _uiAni.UpdateUI();
        }
    }
}
