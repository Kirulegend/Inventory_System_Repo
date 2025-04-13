using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Item / Create a Item")]
public class Item_Data : ScriptableObject
{
    public string _itemName;
    public int _itemQuantity;
    public int _itemMaxQuantity;
    public Sprite _iconTexture;
    public string _itemDescription;
    public GameObject _itemPrefab;
}
