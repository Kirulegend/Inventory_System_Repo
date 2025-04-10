using UnityEngine;
using UnityEngine.UI;

public class UI_Item : MonoBehaviour
{
    GameObject _itemDes;
    Player _player;
    Inventory _inv;
    
    void Start()
    {
        _itemDes = transform.Find("Description").gameObject;
        _itemDes.GetComponent<MaskableGraphic>().maskable = false;
    }

    void Update()
    {
        if (_player == null || _inv == null)
        {
            _player = FindAnyObjectByType<Player>();
            _inv = FindAnyObjectByType<Inventory>();
        }
        Preview();
    }

    [Header("Preview")]
    [Tooltip("Add Green Material")]
    public Material _previewMatG;
    [Tooltip("Add Red Material")]
    public Material _previewMatR;
    Material _defaultMat;
    Rigidbody _tempRigi;
    Renderer _tempRend;
    BoxCollider _tempColB;
    GameObject _tempObj;
    Vector3 pos;
    public static bool _isItem = false;
    public static bool _itemPreview = false;

    void Preview()
    {
        if (_player != null)
        {
            pos = _player._hitPos;
        }
        if (_itemPreview && _tempObj != null)
        {
            _tempObj.transform.position = new Vector3(pos.x, pos.y + .5f, pos.z);
            if (_isItem)
            {
                _tempRend.material = _previewMatR;
            }
            else
            {
                _tempRend.material = _previewMatG;
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                _tempObj.transform.Rotate(0, 45, 0);
            }
            if (Input.GetMouseButtonDown(0) && !_isItem)
            {
                _tempColB.enabled = true;
                _tempRend.material = _defaultMat;
                _tempRigi.isKinematic = false;
                Inventory._invInstance.RemoveItem(_itemPrefab);
                _tempObj = null;
                _itemPreview = false;
            }
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(_tempObj);
                _itemPreview = false;
            }
        }
    }

    [HideInInspector] public string _itemName;
    [HideInInspector] public GameObject _itemPrefab;

    public void InstantiateObj()
    {
        Item _item = _itemPrefab.GetComponent<Item>();
        if (_item._itemAttribute == "Item")
        {
            if(!_player._obj && _tempObj == null)
            {
                Debug.Log("Hello");
                _tempObj = Instantiate(_itemPrefab, new Vector3(pos.x, pos.y + .5f, pos.z), Quaternion.identity);
                _tempRigi = _tempObj.GetComponent<Rigidbody>();
                _tempRend = _tempObj.GetComponent<Renderer>();
                _tempColB = _tempObj.GetComponent<BoxCollider>();
                _tempColB.enabled = false;
                _defaultMat = _tempRend.material;
                _tempRigi.isKinematic = true;
                _tempObj.name = _itemName;
                _itemPreview = true;
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
            if (Player._shieldPower < 10)
            {
                Player._shieldPower = Mathf.Clamp(Player._shieldPower + 2, 0, 10);
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
                Inventory._invInstance.RemoveItem(_itemPrefab);
            }
        }
    }
}
