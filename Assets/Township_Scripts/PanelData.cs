using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Data", menuName = "Scriptable Objects/PanelData")]
public class PanelData : ScriptableObject
{
    public string _name;
    public int _fabricatingQuantity;
    public Sprite _fabricatingImage;
    public Sprite _fabricatingImageBg;
    public int _fabricatingAvaliableQuantity;
    public int _fabricatingTime;
    public Sprite _NeedImage;
    public int[] _needQuantity;
    public int[] _needAvaliableQuantity;
    public Data _data;
    GameObject _button;
    private void OnEnable()
    {
        _button = _data._button;
    }
    public enum FabricationItem
    {
        EnergyBar,
        EnergyMeal
    }
    public FabricationItem _selection;
}