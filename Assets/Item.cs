using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    [Header("Item Data")]
    [Tooltip("Enter the Item Name")]
    public string _itemName;
    [HideInInspector] public int _itemQuantity;
    [Tooltip("Enter the Item Max Quantity")]
    public int _itemMaxQuantity = 10;
    [Tooltip("Add the Item Icon")]
    public Sprite _iconTexture;
    [Tooltip("Enter the Item Description")]
    public string _itemDescription;
    [Tooltip("Enter the Item Type/Attribute")]
    public string _itemAttribute;
    [Tooltip("Add the Specific GameObj Prefab")]
    public GameObject _itemPrefab;
}
