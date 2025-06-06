using System.Runtime.InteropServices.WindowsRuntime;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;
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
    [HideInInspector]public int _avaliableQuantity;
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
    public Sprite _unit;
    string _name;
    int _avaliableQuantity = 0;
    public int _time;
    public int _creatingQuantity;
    TextMeshProUGUI _Name;
    TextMeshProUGUI _AvaliableQuantity;
    TextMeshProUGUI _Time;
    Transform[] _NeedPanel;
    public Transform _unitPanel;
    RectTransform _need;
    public Transform _needPanel;
    GameData _gameData;
    void Start()
    {
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
        if (_item == Item._energyBar)
        {
            _avaliableQuantity = _gameData._energyBarCount;
        }
        else if (_item == Item._energyMeal)
        {
            _avaliableQuantity = _gameData._energyMealCount;
        }
        _AvaliableQuantity.text = _avaliableQuantity + " Avaliable";
        for (int i = 0; i < slots.Length; i++)
        {
            _NeedPanel[i] = Instantiate(_needPanel, _need);
            if (slots[i]._item == IntSpriteSlot.Item._nutriAlgae)
            {
                slots[i]._avaliableQuantity = _gameData._nutriAlgaeCrop;
            }
            else if (slots[i]._item == IntSpriteSlot.Item._bioLuminary)
            {
                slots[i]._avaliableQuantity = _gameData._bioLuminaryCount;
            }
            _NeedPanel[i].GetComponent<Image>().sprite = slots[i]._neededItem;
            _NeedPanel[i].Find("Item_Quantity").GetComponent<TextMeshProUGUI>().text = slots[i]._neededQuantity + "/" + slots[i]._avaliableQuantity;
        }
    }
    void Update()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i]._item == IntSpriteSlot.Item._nutriAlgae)
            {
                slots[i]._avaliableQuantity = _gameData._nutriAlgaeCrop;
            }
            else if (slots[i]._item == IntSpriteSlot.Item._bioLuminary)
            {
                slots[i]._avaliableQuantity = _gameData._bioLuminaryCount;
            }
            _NeedPanel[i].Find("Item_Quantity").GetComponent<TextMeshProUGUI>().text = slots[i]._neededQuantity + "/" + slots[i]._avaliableQuantity;
        }
        if (_item == Item._energyBar)
        {
            _avaliableQuantity = _gameData._energyBarCount;
        }
        else if (_item == Item._energyMeal)
        {
            _avaliableQuantity = _gameData._energyMealCount;
        }
        _AvaliableQuantity.text = _avaliableQuantity + " Avaliable";
    }
    bool _dataUpdate = false;
    public void DataUpdate()
    {
        if (!_dataUpdate)
        {
            for (int i = 0; i < slots.Length; i++)
            {
                if (slots[i]._item == IntSpriteSlot.Item._nutriAlgae)
                {
                    _gameData._nutriAlgaeCrop -= slots[i]._neededQuantity;
                }
                else if (slots[i]._item == IntSpriteSlot.Item._bioLuminary)
                {
                    _gameData._bioLuminaryCount -= slots[i]._neededQuantity;
                }
            }
            _dataUpdate = true;
        }
        else
        {
            if (_item == Item._energyBar)
            {
                _gameData._energyBarCount += _creatingQuantity;
            }
            else if (_item == Item._energyMeal)
            {
                _gameData._energyMealCount += _creatingQuantity;
            }
            _dataUpdate = false;
        }
    }
    public GameManagerTS _gmTS;
    public void FabricatorData()
    {
        if (Check())
        {
            _gmTS._fabricatingObj.gameObject.SetActive(true);
            Debug.Log(gameObject.name);
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
