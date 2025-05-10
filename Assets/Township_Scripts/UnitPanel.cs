using TMPro;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class IntSpriteSlot
{
    public enum Item
    {
        _nutriAlgae,
        _bioLuminary
    }
    public Item _item;
    public int _neededQuantity;
    public int _avaliableQuantity;
    public Sprite _neededItem;
}
public class UnitPanel : MonoBehaviour
{
    public enum Item
    {
        _energyBar,
        _energyMeal
    }
    public Item _item;
    string _name;
    int _avaliableQuantity = 0;
    public int _time;
    public int _creatingQuantity;
    TextMeshProUGUI _Name;
    TextMeshProUGUI _AvaliableQuantity;
    TextMeshProUGUI _Time;
    TextMeshProUGUI[] _NeedQuantity;
    Transform[] _NeedPanel;
    public Transform _unitPanel;
    RectTransform _need;
    public Transform _needPanel;
    void Start()
    {
        _NeedPanel = new Transform[slots.Length];
        _unitPanel = transform.Find("Unit Panel").GetComponent<Transform>();
        _name = gameObject.name;
        _Name = _unitPanel.Find("Name").GetComponent<TextMeshProUGUI>();
        _Name.text = _name + " X" + _creatingQuantity;
        _AvaliableQuantity = _unitPanel.Find("Available").GetComponent<TextMeshProUGUI>();
        if(_item == Item._energyBar)
        {
            _avaliableQuantity = GameData._energyBarCount;
        }
        else if(_item == Item._energyMeal)
        {
            _avaliableQuantity = GameData._energyMealCount;
        }
        _AvaliableQuantity.text = _avaliableQuantity + " Avaliable";
        _Time = _unitPanel.Find("Time").GetComponent<TextMeshProUGUI>();
        _Time.text = _time + " SEC";
        _need = _unitPanel.Find("Need").GetComponent<RectTransform>();
        for (int i = 0; i < slots.Length; i++)
        {
            _NeedPanel[i] = Instantiate(_needPanel, _need);
            if (slots[i]._item == IntSpriteSlot.Item._nutriAlgae)
            {
                slots[i]._avaliableQuantity = GameData._nutriAlgaeCrop;
            }
            else if (slots[i]._item == IntSpriteSlot.Item._bioLuminary)
            {
                slots[i]._avaliableQuantity = GameData._bioLuminaryCount;
            }
            _NeedPanel[i].GetComponent<Image>().sprite = slots[i]._neededItem;
            _NeedPanel[i].Find("Item_Quantity").GetComponent<TextMeshProUGUI>().text = slots[i]._neededQuantity + "/" + slots[i]._avaliableQuantity;
        }
    }

    [SerializeField]
    int slotCount = 0;
    [SerializeField]
    private IntSpriteSlot[] slots;

    private void OnValidate()
    {
        if (slots == null || slots.Length != slotCount)
        {
            IntSpriteSlot[] newSlots = new IntSpriteSlot[slotCount];
            if (slots != null)
            {
                for (int i = 0; i < Mathf.Min(slots.Length, slotCount); i++)
                {
                    newSlots[i] = slots[i];
                }
            }
            for (int i = 0; i < slotCount; i++)
            {
                if (newSlots[i] == null)
                {
                    newSlots[i] = new IntSpriteSlot();
                }
            }
            slots = newSlots;
        }
    }
}
