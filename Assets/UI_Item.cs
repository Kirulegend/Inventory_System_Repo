using UnityEngine;
using static UnityEngine.InputManagerEntry;

public class UI_Item : MonoBehaviour
{
    public string _itemName;
    public GameObject _itemPrefab;
    public Player _player;
    public Inventory _inv;

    void Start()
    {

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
        if (!_player._obj)
        {
            Vector3 pos = _player._hitPos;
            Instantiate(_itemPrefab, new Vector3(pos.x, pos.y + 1f, pos.z), Quaternion.identity);
            _itemPrefab.name = _itemName;
            Inventory._invInstance.RemoveItem(_itemPrefab);
        }
    }
}
