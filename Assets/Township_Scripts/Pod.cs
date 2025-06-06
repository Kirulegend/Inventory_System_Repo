using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class Pod : MonoBehaviour
{
    public static Pod Instance;
    public static bool Click = false;
    public bool Nutri_Algae = false;
    public bool Bio_Luminary = false;
    public string _activeCrop;
    Transform _pod;
    public bool Start = false;
    float Timer = 0;
    public bool _cropReady = false;
    GameData _gameData;
    int _cropTimer;

    void Awake()
    {
        _gameData = GameObject.Find("GameData")?.GetComponent<GameData>();
        _pod = transform.Find("Pod");
        Instance = this;
    }
    void Check()
    {
        if (Click)
        {
            if (Nutri_Algae)
            {
                _activeCrop = "Nutri-Algae";
                Start = true;
                Click = false;
                Nutri_Algae = false;
                _cropTimer = _gameData._nutriAlgaeTime;
            }
            if (Bio_Luminary)
            {
                _activeCrop = "Bio_Luminary";
                _cropTimer = _gameData._bioLuminaryTime;
                Start = true;
                Click = false;
                Bio_Luminary = false;
            }
        }
    }
    
    void Rot()
    {
        if (Start && Timer < _cropTimer)
        {
            Timer += Time.deltaTime;
            _pod.Rotate(0, 50 * Time.deltaTime, 0);
            if(Timer >= _cropTimer)
            {
                Start = false;
                Timer = 0;
                _cropReady = true;
                _gameData._xp += 5;
                Debug.Log("Crop Collected");
                if (_activeCrop == "Nutri-Algae")
                {
                    _gameData._nutriAlgaeCrop++;
                }
                if (_activeCrop == "Bio_Luminary")
                {
                    _gameData._bioLuminaryCount++;
                }
                _activeCrop = string.Empty;
                _cropReady = false;
            }
        }
    }
    void Update()
    {
        Check();
        Rot();
    }
}
