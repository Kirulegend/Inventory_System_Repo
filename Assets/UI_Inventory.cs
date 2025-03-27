using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.Rendering.LookDev;
public class UI_Inventory : MonoBehaviour
{
    Animator _ani;
    public float _aniIndex = 0;
    public Inventory _inv;
    public GameObject _itemPrefab;
    public RectTransform _itemContainer;
    public List<GameObject> slots = new List<GameObject>();
    public Slider _sliderHealth;
    public Slider _sliderPower;
    public int _invIndex = 0;
    public bool _invNumPressed = false;
    private int _tempIndex = 0;
    private int _tempIndex1 = -1;
    private int _tempIndex2 = 0;
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
        //Debug.Log(slots.Count);
        _sliderHealth.value = Player._healthPower;
        _sliderPower.value = Player._shieldPower;
        _ani.SetFloat("Index", _aniIndex);
        if (Input.GetKeyUp(KeyCode.Q))
        {
            OnClickEvent();
        }
        NumInvKeypad();
        NumInvScroll();
    }

    public void UpdateUI()
    {
        while (slots.Count < _inv._invItems.Count)
        {
            GameObject slot = Instantiate(_itemPrefab, _itemContainer);
            slots.Add(slot);
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
    void NumInvScroll()
    {
        Debug.Log(_tempIndex);
        float scrollInput = -Input.GetAxisRaw("Mouse ScrollWheel") * 10;
        if (_tempIndex1 == -1 && slots.Count > 0)
        {
            Image _tempImag0 = slots[0].GetComponent<Image>();
            _tempImag0.color = new Color(_tempImag0.color.r / 2, _tempImag0.color.g / 2, _tempImag0.color.b / 2, _tempImag0.color.a);
            _tempIndex1 = 0;
            _tempIndex2 = 0;
        }
        if (slots.Count > 0 && _aniIndex == 2 && !UI_Item._itemPreview)
        {
            if (scrollInput != 0)
            {
                _tempIndex = Mathf.Clamp(_tempIndex2 + Mathf.RoundToInt(scrollInput), 0, slots.Count - 1);

                if (_tempIndex != _tempIndex2)
                {
                    if (_tempIndex1 >= 0 && _tempIndex1 < slots.Count)
                    {
                        Image _tempImag1 = slots[_tempIndex1].GetComponent<Image>();
                        _tempImag1.color = new Color(_tempImag1.color.r * 2, _tempImag1.color.g * 2, _tempImag1.color.b * 2, _tempImag1.color.a);
                    }

                    _tempIndex2 = _tempIndex;
                    Image _tempImag2 = slots[_tempIndex2].GetComponent<Image>();
                    _tempImag2.color = new Color(_tempImag2.color.r / 2, _tempImag2.color.g / 2, _tempImag2.color.b / 2, _tempImag2.color.a);
                    _tempIndex1 = _tempIndex2;
                }
            }
        }
        else if (_aniIndex != 2)
        {
            if (_tempIndex2 >= 0 && _tempIndex2 < slots.Count)
            {
                Image _tempImag3 = slots[_tempIndex2].GetComponent<Image>();
                _tempImag3.color = new Color(_tempImag3.color.r * 2, _tempImag3.color.g * 2, _tempImag3.color.b * 2, _tempImag3.color.a);
            }

            _tempIndex1 = -1;
            _tempIndex2 = 0;
        }
        if (Input.GetKeyDown(KeyCode.E) && slots.Count > 0 && _aniIndex == 2)
        {
            Button TempButtom = slots[_tempIndex].GetComponent<Button>();
            TempButtom.onClick.Invoke();
        }
    }
    void NumInvKeypad()
    {
        if (Input.anyKeyDown && slots.Count > 0 && _aniIndex == 2)
        {
            string _tempIndex = Input.inputString;
            if (int.TryParse(_tempIndex, out _invIndex))
            {
                _invNumPressed = true;
            }
        }
        if (_invNumPressed && _invIndex <= slots.Count && _invIndex > 0 && !UI_Item._itemPreview)
        {
            Button TempButtom = slots[_invIndex - 1].GetComponent<Button>();
            TempButtom.onClick.Invoke();
            _invNumPressed = false;
        }
    }
    public void OnClickEvent()
    {
        if (_aniIndex == 0)
        {
            _aniIndex = 1;
            StartCoroutine(SetIndex(2));
            _tempIndex1 = -1;
            _tempIndex2 = 0;
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
