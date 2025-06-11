using TMPro;
using UnityEngine;

public class InventoryPanel : MonoBehaviour
{
    public string _name;
    public TextMeshProUGUI _sellDes;
    public TextMeshProUGUI _sellPrice;
    public int _price;
    public int _quantity;
    void Start()
    {
        _name = transform.Find("Inventory Panel").Find("Name").GetComponent<TextMeshProUGUI>().text;
        _price = GameData._instance._invI.Find(item => item.name == _name).price;
        _sellDes = transform.Find("Inventory Panel").Find("SellDes").GetComponent<TextMeshProUGUI>();
        _sellPrice = transform.Find("Inventory Panel").Find("Sell").Find("Price").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        _sellDes.text = $"Sell {_quantity} For";
        _sellPrice.text = (_quantity * _price).ToString();
    }
    public void Add()
    {
        Debug.Log("Add");
        if(_quantity < GameData._instance._invI.Find(item => item.name == _name).quantity)
        {
            _quantity++;
        }
    }
    public void Remove()
    {
        Debug.Log("Remove");
        if (_quantity > 0)
        {
            _quantity--;
        }
    }
    public void Sell()
    {
        TS_Inventory._inv.RemoveItem(_name, _quantity);
        GameData._instance._qc += _price * _quantity;
        _quantity = 0;
    }
}
