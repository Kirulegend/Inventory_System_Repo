using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
using static UnityEngine.InputManagerEntry;

public class UI_Item : MonoBehaviour
{
    public string _itemName;
    public GameObject _itemPrefab;
    public GameObject _itemDes;
    public Player _player;
    public Inventory _inv;

    void Start()
    {
        _itemDes = transform.Find("Description").gameObject;
        _itemDes.GetComponent<MaskableGraphic>().maskable = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (_player == null || _inv == null)
        {
            _player = FindAnyObjectByType<Player>();
            _inv = FindAnyObjectByType<Inventory>();
        }
    }

    public void InstantiateObj()
    {
        Item _item = _itemPrefab.GetComponent<Item>();
        if (_item._itemAttribute == "Item")
        {
            if(!_player._obj)
            {
                Vector3 pos = _player._hitPos;
                Instantiate(_itemPrefab, new Vector3(pos.x, pos.y + 1f, pos.z), Quaternion.identity);
                _itemPrefab.name = _itemName;
                Inventory._invInstance.RemoveItem(_itemPrefab);
            }
        }
        else if (_item._itemAttribute == "Heal")
        {
            if (Player._healthPower < 10)
            {
                Player._healthPower = Mathf.Clamp(Player._healthPower + 2, 0, 10);
                Inventory._invInstance.RemoveItem(_itemPrefab);
            }
            else
            {
                Debug.Log("Full Health");
            }
        }
        else if (_item._itemAttribute == "Power")
        {
            if (Player._attackPower < 10)
            {
                Player._attackPower = Mathf.Clamp(Player._attackPower + 2, 0, 10);
                Inventory._invInstance.RemoveItem(_itemPrefab);
            }
            else
            {
                Debug.Log("Full Attack");
            }
        }
        else if( _item._itemAttribute == "Key")
        {
            if (GameManager._nearDoor)
            {
                Debug.Log("klhnsadifhn");
                Inventory._invInstance.RemoveItem(_itemPrefab);
                GameManager._hasKey = true;
            }
        }
    }
}
