using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using static FarmPanel;

[System.Serializable]
public class IntSpriteSlot
{
    public enum Item
    {
        NutriAlgae,
        BioLuminary
    }
    public Item _item;
    public int _neededQuantity;
    [HideInInspector]public int _avaliableQuantity;
}
public class UnitPanel : MonoBehaviour
{
    public enum Item
    {
        EnergyBar,
        EnergyMeal
    }
    public Item _item;
    Sprite _unit;
    string _name;
    int _avaliableQuantity = 0;
    int _time;
    public int _creatingQuantity;
    TextMeshProUGUI _Name;
    TextMeshProUGUI _AvaliableQuantity;
    TextMeshProUGUI _Time;
    Transform[] _NeedPanel;
    public Transform _unitPanel;
    RectTransform _need;
    public Transform _needPanel;
    GameData _gameData;
    TS_Inventory _inv;
    void Start()
    {
        _inv = GameObject.Find("Inventory")?.GetComponent<TS_Inventory>();
        _gameData = GameObject.Find("GameData")?.GetComponent<GameData>();
        _gmTS = GameObject.Find("GameManager")?.GetComponent<GameManagerTS>();
        _NeedPanel = new Transform[slots.Length];
        _unitPanel = transform.Find("Unit Panel").GetComponent<Transform>();
        _name = gameObject.name;
        _Name = _unitPanel.Find("Name").GetComponent<TextMeshProUGUI>();
        _Name.text = _name + " X" + _creatingQuantity;
        _AvaliableQuantity = _unitPanel.Find("Available").GetComponent<TextMeshProUGUI>();
        _Time = _unitPanel.Find("Time").GetComponent<TextMeshProUGUI>();
        _Time.text = _time + " SEC";
        _need = _unitPanel.Find("Need").GetComponent<RectTransform>();
        _avaliableQuantity = _gameData._invI.Find(item => item.name == _item.ToString()).quantity;
        _AvaliableQuantity.text = _avaliableQuantity + " Avaliable";
        for (int i = 0; i < slots.Length; i++)
        {
            _NeedPanel[i] = Instantiate(_needPanel, _need);
            slots[i]._avaliableQuantity = _gameData._invI.Find(item => item.name == slots[i]._item.ToString()).quantity;
            _NeedPanel[i].GetComponent<Image>().sprite = _gameData._invI.Find(item => item.name == slots[i]._item.ToString()).icon;
            _NeedPanel[i].Find("Item_Quantity").GetComponent<TextMeshProUGUI>().text = slots[i]._neededQuantity + "/" + slots[i]._avaliableQuantity;
        }
        _unit = _gameData._invI.Find(item => item.name == _item.ToString()).iconBG;
        _time = _gameData._invI.Find(item => item.name == _item.ToString()).time;
    }
    void Update()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            _NeedPanel[i].Find("Item_Quantity").GetComponent<TextMeshProUGUI>().text = slots[i]._neededQuantity + "/" + _gameData._invI.Find(item => item.name == slots[i]._item.ToString()).quantity;
        }
        _AvaliableQuantity.text = _gameData._invI.Find(item => item.name == _item.ToString()).quantity.ToString() + " Avaliable";
    }
    bool _dataUpdate = false;
    public void DataUpdate()
    {
        if (!_dataUpdate)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                _inv.RemoveItem(slots[i]._item.ToString(), slots[i]._neededQuantity);
            }
            _dataUpdate = true;
        }
        else
        {
            _inv.AddItem(_item.ToString(), _creatingQuantity);
            _gameData._xp += 10;
            _dataUpdate = false;
        }
    }
    GameManagerTS _gmTS;
    public void FabricatorData()
    {
        if (Check())
        {
            _gmTS._fabricatingObj.gameObject.SetActive(true);
            _gmTS._activeFab._fabricatingObjSprite = _unit;
            _gmTS._fabricatingObj.sprite = _unit;
            _gmTS._fabricatingObj.transform.Find("BG").GetComponent<Image>().sprite = _unit;
            _gmTS._activeFab._timer = _time;
            _gmTS._activeFab._name = _name;
            _gmTS._activeFab._button = transform;
        }
    }

    bool Check()
    {
        if (slots.Length > 1)
        {
            return slots[0]._avaliableQuantity >= slots[0]._neededQuantity && slots[1]._avaliableQuantity >= slots[1]._neededQuantity;
        }
        else return slots[0]._avaliableQuantity >= slots[0]._neededQuantity;
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
