using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
using UnityEngine.UI;
using static FarmPanel;

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
    public Slider _timerslider;
    public Image _crop;
    public Canvas _cropCanvas;

    void Awake()
    {
        _cropCanvas = transform.Find("Canvas").GetComponent<Canvas>();
        _timerslider = transform.Find("Canvas").Find("Timer").GetComponent<Slider>();
        _crop = transform.Find("Canvas").Find("Timer").Find("Crop").GetComponent<Image>();
        _inv = GameObject.Find("Inventory")?.GetComponent<TS_Inventory>();
        _gameData = GameObject.Find("GameData")?.GetComponent<GameData>();
        _pod = transform.Find("Pod");
        Instance = this;
        _cropCanvas.enabled = false;
        _crop.sprite = _gameData._invI.Find(item => item.name == _activeCrop).iconBG;
    }
    public void Check()
    {
        if (Click)
        {
            Start = true;
            _cropCanvas.enabled = true;
            if (_activeCrop != null) 
            {
                _cropTimer = _gameData._invI.Find(item => item.name == _activeCrop).time;
                _timerslider.maxValue = _cropTimer;
            }
            Click = false;
        }
    }
    
    void Rot()
    {
        if (Start && Timer < _cropTimer)
        {
            _timerslider.value = Timer;
            Timer += Time.deltaTime;
            _pod.Rotate(0, 50 * Time.deltaTime, 0);
            if(Timer >= _cropTimer)
            {
                Start = false;
                Timer = 0;
                _cropReady = true;
                _gameData._xp += 5;
                Debug.Log("Crop Collected");
                _inv.AddItem(_activeCrop, FarmPanel._creatingQuantity);
                _activeCrop = string.Empty;
                _cropReady = false;
                _cropCanvas.enabled = false;
                _timerslider.value = 0;
            }
        }
    }
    void Update()
    {
        //Check();
        Rot();
    }
}
