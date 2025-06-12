using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class FarmPanel : MonoBehaviour
{
    public enum Crop
    {
        NutriAlgae,
        BioLuminary,
        Phycophyta
    }
    [Tooltip("Select the Crop for this panel")]
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
    Image _cropImg;
    void Start()
    {
        _gameData = GameObject.Find("GameData")?.GetComponent<GameData>();
        _cropImg = transform.GetComponent<Image>();
        _cropImg.sprite = _gameData._invI.Find(item => item.name == _crop.ToString()).icon;
        _farmPanel = Instantiate(Resources.Load<Transform>("Farm Panel"), transform);
        gameObject.GetComponent<UI_Hover>()._hoverData = _farmPanel.gameObject;
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
