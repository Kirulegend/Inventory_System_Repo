using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Security.Cryptography;

public class GameData : MonoBehaviour
{
    public static int _level = 1;
    public static int _qc = 500;
    public static float _xp = 0;
    public static int _roadCount = 0;
    public static int _buildCount = 0;
    public static int _nutriAlgaeCrop = 10;
    public static int _bioLuminaryCount = 10;
    public static int _energyBarCount = 10;
    public static int _energyMealCount = 10;
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
