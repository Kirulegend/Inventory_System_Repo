using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;

public class Pod : MonoBehaviour
{
    public static Pod Instance;
    public static bool Click = false;
    public string _activeCrop;
    Transform _pod;
    public bool Start = false;
    float Timer = 0;
    public bool _cropReady = false;
    GameData _gameData;
    int _cropTimer;
    TS_Inventory _inv;

    void Awake()
    {
        _inv = GameObject.Find("Inventory")?.GetComponent<TS_Inventory>();
        _gameData = GameObject.Find("GameData")?.GetComponent<GameData>();
        _pod = transform.Find("Pod");
        Instance = this;
    }
    public void Check()
    {
        if (Click)
        {
            Start = true;
            if(_activeCrop != null) _cropTimer = _gameData._invI.Find(item => item.name == _activeCrop).time;
            Click = false;
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
                //_gameData._invI.Find(item => item.name == _activeCrop).quantity += FarmPanel._creatingQuantity;
                _inv.AddItem(_activeCrop, FarmPanel._creatingQuantity);
                _activeCrop = string.Empty;
                _cropReady = false;
            }
        }
    }
    void Update()
    {
        //Check();
        Rot();
    }
}
