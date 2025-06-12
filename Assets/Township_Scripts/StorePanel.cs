using TMPro;
using UnityEngine;

public class StorePanel : MonoBehaviour
{
    string _name;
    public int _person;
    public int _price;
    Transform _storePanel;
    TextMeshProUGUI _Name;
    TextMeshProUGUI _Quantity;
    TextMeshProUGUI _Price;
    Placement _placement;

    void Start()
    {
        _placement = GameObject.Find("Ground")?.GetComponent<Placement>();
        _storePanel = transform.Find("Store Panel").GetComponent<Transform>();
        _name = gameObject.name;
        _Name = _storePanel.Find("Name").GetComponent<TextMeshProUGUI>();
        _Name.text = _name;
        _Quantity = _storePanel.Find("PersonQuantity").GetComponent<TextMeshProUGUI>();
        _Quantity.text = "X" + _person.ToString();
        _Price = _storePanel.Find("Price").GetComponent<TextMeshProUGUI>();
        _Price.text = _price.ToString();
    }
    
    public void Select()
    {
        if(GameData._instance._qc >= _price)
        {
            GameData._instance._qc -= _price;
            GameData._instance._population += _person;
            GameData._instance._xp += _person * 2;
            _placement.Build(gameObject.name);
        }
    }
}
