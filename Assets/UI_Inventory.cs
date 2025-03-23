using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
public class UI_Inventory : MonoBehaviour
{
    Animator _ani;
    float _aniIndex = 0;
    public Inventory _inv;
    public GameObject _itemPrefab;
    public RectTransform _itemContainer;
    private List<GameObject> slots = new List<GameObject>();

    void Start()
    {
        if (_inv != null && _inv._invItems.Count > 0)
        {
            UpdateUI();
        }
        _ani = GetComponent<Animator>();
    }

    void Update()
    {
        _ani.SetFloat("Index", _aniIndex);
        if (Input.GetKeyUp(KeyCode.E))
        {
            OnClickEvent();
        }
    }

    public void UpdateUI()
    {
        while (slots.Count < _inv._invItems.Count)
        {
            //Debug.Log(slots.Count + " " + _inv._invItems.Count);
            GameObject slot = Instantiate(_itemPrefab, _itemContainer);
            slots.Add(slot);
            //_itemContainer.offsetMax += new Vector2(0, 69f);
        }
        while (slots.Count > _inv._invItems.Count)
        {
            GameObject slotToRemove = slots[slots.Count - 1];
            slots.RemoveAt(slots.Count - 1);
            Destroy(slotToRemove);
        }
        for (int i = 0; i < slots.Count; i++)
        {
            GameObject slot = slots[i];
            GameObject itemInstance = _inv._invItems[i];

            Item item = itemInstance.GetComponent<Item>();
            Image slotImage = slot.transform.Find("Image").GetComponent<Image>();
            slotImage.sprite = item._iconTexture;
            TextMeshProUGUI slotText = slot.transform.Find("Name").GetComponent<TextMeshProUGUI>();
            slotText.text = item._itemName;
            TextMeshProUGUI slotDescription = slot.transform.Find("Description").GetComponent<TextMeshProUGUI>();
            slotDescription.text = item._itemDescription;
            TextMeshProUGUI slotQuantity = slot.transform.Find("Quantity").GetComponent<TextMeshProUGUI>();
            slotQuantity.text = item._itemQuantity.ToString();

            UI_Item ui_Item = slot.GetComponent<UI_Item>();
            ui_Item._itemPrefab = itemInstance;
            ui_Item._itemName = item._itemName;
        }
    }
    public void OnClickEvent()
    {
        if (_aniIndex == 0)
        {
            _aniIndex = 1;
            StartCoroutine(SetIndex(2));
        }
        if (_aniIndex == 2)
        {
            _aniIndex = 3;
            StartCoroutine(SetIndex(0));
        }
    }
    IEnumerator SetIndex(float Index)
    {
        yield return new WaitForSeconds(.25f);
        _aniIndex = Index;
    }
}
