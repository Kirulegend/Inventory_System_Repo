using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PanelData : MonoBehaviour
{
    public enum Type
    {
        Store, Inventory, Farm, Unit, 
    }

    public Type _type;
    [HideInInspector]public GameObject _panel;
    public GameObject[] _panelPrefab;

    public void setvalue()
    {
        switch (_type)
        {
            case Type.Store:
                _panel = Instantiate(_panelPrefab[0]);
                break;
            case Type.Inventory:
                _panel = Instantiate(_panelPrefab[1]);
                break;
            case Type.Farm:
                _panel = Instantiate(_panelPrefab[2]);
                break;
            case Type.Unit:
                _panel = Instantiate(_panelPrefab[3]);
                break;
        }
    }
    void Awake()
    {
        Debug.Log("Hi");
    }
}