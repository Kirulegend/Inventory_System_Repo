using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

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
    public int _time;
    public int _creatingQuantity;
    public int _price;
    TextMeshProUGUI _Name;
    TextMeshProUGUI _AvaliableQuantity;
    TextMeshProUGUI _Time;
    TextMeshProUGUI _Price;
    Transform _farmPanel;
    void Start()
    {
        _farmPanel = transform.Find("Farm Panel").GetComponent<Transform>();
        _name = gameObject.name;
        _Name = _farmPanel.Find("Name").GetComponent<TextMeshProUGUI>();
        _Name.text = _name + " X" + _creatingQuantity;
        _AvaliableQuantity = _farmPanel.Find("Available").GetComponent<TextMeshProUGUI>();
        _Time = _farmPanel.Find("Time").GetComponent<TextMeshProUGUI>();
        _Time.text = _time + " SEC";
        _Price = _farmPanel.Find("Price").GetComponent<TextMeshProUGUI>();
        _Price.text = _price.ToString();
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
            _avaliableQuantity = GameData._nutriAlgaeCrop;
        }
        else if (_crop == Crop._bioLuminary)
        {
            _avaliableQuantity = GameData._bioLuminaryCount;
        }
        _AvaliableQuantity.text = _avaliableQuantity + " Avaliable";
    }
}
