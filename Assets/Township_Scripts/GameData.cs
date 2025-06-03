using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Security.Cryptography;

public class GameData : MonoBehaviour
{
    public static GameData _instance;
    public int _level = 1;
    public int _qc = 500;
    public float _xp = 0;
    public int _roadCount = 0;
    public int _buildCount = 0;
    public int _nutriAlgaeCrop = 10;
    public int _nutriAlgaePrice = 10;
    public int _bioLuminaryCount = 10;
    public int _bioLuminaryPrice = 20;
    public int _energyBarCount = 10;
    public int _energyMealCount = 10;
    private void Awake()
    {
        _instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _roadCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
