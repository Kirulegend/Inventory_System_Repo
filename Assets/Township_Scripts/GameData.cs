using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;

[System.Serializable]
public class InventoryItem
{
    public string name;
    public int quantity;
    public int price;
    public int time;
    public Sprite icon;
    public Sprite iconBG;

    public InventoryItem(string name, int quantity, int price, int time, Sprite icon, Sprite iconBG)
    {
        this.name = name;
        this.quantity = quantity;
        this.price = price;
        this.time = time;
        this.icon = icon;
        this.iconBG = iconBG;
    }
}
public class GameData : MonoBehaviour
{
    public List<InventoryItem> _invI = new List<InventoryItem>();
    public static GameData _instance;
    public int _level = 1;
    public int _qc = 500;
    public float _xp = 0;
    public int _roadCount = 0;
    public int _buildCount = 0;
    TS_Inventory _inv;
    private void Awake()
    {
        _instance = this;
        _inv = GameObject.Find("Inventory")?.GetComponent<TS_Inventory>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _roadCount = 0;
        for (int i = 0; i < _invI.Count; i++)
        {
            //Debug.Log(_invI[i].name);
            if (_invI[i].quantity > 0)
            {
                _inv.AddItem(_invI[i].name, _invI[i].quantity);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
