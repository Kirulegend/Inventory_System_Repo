using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public string _itemName;
    public int _itemQuantity;
    public int _itemMaxQuantity = 10;
    public Sprite _iconTexture;
    public string _itemDescription;
    public string _itemAttribute;
    public GameObject _itemPrefab;
}
