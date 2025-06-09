using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Playables;

public class FarmPanel : MonoBehaviour
{
    public enum Crop
    {
        NutriAlgae,
        BioLuminary
    }
    public Crop _crop;
    string _name;
    int _avaliableQuantity = 0;
    int _time;
    public static int _creatingQuantity = 3;
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
        _avaliableQuantity = _gameData._invI.Find(item => item.name == _crop.ToString()).quantity;
        _price = _gameData._invI.Find(item => item.name == _crop.ToString()).price;
        _time = _gameData._invI.Find(item => item.name == _crop.ToString()).time;
        _Time.text = _time + " SEC";
        _AvaliableQuantity.text = _avaliableQuantity + " Avaliable";
        _Price.text = _price.ToString();
    }
}
