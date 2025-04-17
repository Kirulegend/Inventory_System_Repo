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

    GameObject _player;
    TextMeshPro _name;
    [HideInInspector] public bool _isRay;

    void Start()
    {
        _player = GameObject.FindWithTag("Player");
        _name = GetComponentInChildren<TextMeshPro>();
        _name.enabled = false;
        _name.text = _itemName;
    }
    void Update()
    {
        if (_isRay && Player._obj)
        {
            _name.enabled = true;
            Vector3 direction = _player.transform.position - _name.transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(-direction);
            _name.transform.rotation = targetRotation;
        }
        else
        {
            _name.enabled = false;
            _isRay = false;
        }
    }
}
