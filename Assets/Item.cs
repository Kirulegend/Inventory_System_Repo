using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public string _itemName;
    public int _itemQuantity = 1;
    public int _itemMaxQuantity;
    public Sprite _iconTexture;
    public string _itemDescription;
    public GameObject _itemPrefab;
}
