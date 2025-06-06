using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Playables;

public class FarmPanel : MonoBehaviour
{
    public enum Crop
    {
        _nutriAlgae,
        _bioLuminary
    }
    public Crop _crop;
    string _name;
    int _avaliableQuantity = 0;
    int _time;
    public int _creatingQuantity;
    int _price;
    TextMeshProUGUI _Name;
    TextMeshProUGUI _AvaliableQuantity;
    TextMeshProUGUI _Time;
    TextMeshProUGUI _Price;
    Transform _farmPanel;
    GameData _gameData;
    void Start()
    {
        _gameData = GameObject.Find("GameData")?.GetComponent<GameData>();
        _farmPanel = transform.Find("Farm Panel").GetComponent<Transform>();
        _name = gameObject.name;
        _Name = _farmPanel.Find("Name").GetComponent<TextMeshProUGUI>();
        _Name.text = _name + " X" + _creatingQuantity;
        _AvaliableQuantity = _farmPanel.Find("Available").GetComponent<TextMeshProUGUI>();
        _Time = _farmPanel.Find("Time").GetComponent<TextMeshProUGUI>();
        _Price = _farmPanel.Find("Price").GetComponent<TextMeshProUGUI>();
        DynamicData();
    }
    void Update()
    {
        DynamicData();
    }
    void DynamicData()
    {
        if (_crop == Crop._nutriAlgae)
        {
            _avaliableQuantity = _gameData._nutriAlgaeCrop;
            _price = _gameData._nutriAlgaePrice;
            _time = _gameData._nutriAlgaeTime;
        }
        if (_crop == Crop._bioLuminary)
        {
            _avaliableQuantity = _gameData._bioLuminaryCount;
            _price = _gameData._bioLuminaryPrice;
            _time = _gameData._bioLuminaryTime;
        }
        _Time.text = _time + " SEC";
        _AvaliableQuantity.text = _avaliableQuantity + " Avaliable";
        _Price.text = _price.ToString();
    }
}
