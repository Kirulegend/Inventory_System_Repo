using System.Threading;
using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class FabricatorUnit : MonoBehaviour
{
    public static FabricatorUnit Instance;
    public Sprite _fabricatingObjSprite;
    public string _name = null;    

    public GameObject _buttonParent;

    [HideInInspector] public float _timer;
    public float _currentTimer = 0;
    public Transform _button;
    float _tempTimer = 0;

    public bool _start = false;
    public bool _ready = false;
    public static bool Click = false;

    GameManagerTS _gmTS;
    void Start()
    {
        _gmTS = GameObject.Find("GameManager")?.GetComponent<GameManagerTS>();
    }
    public void FabricatorStart()
    {
        if (!_start && !_ready && _gmTS._fabricatingObj.gameObject.activeInHierarchy)
        {
            _start = true;
            _gmTS._fabButtons.SetActive(false);
            _button.GetComponent<UnitPanel>().DataUpdate();
        }   
        else if (_ready)
        {
            _currentTimer = 0;
            _fabricatingObjSprite = null;
            _gmTS._fabricatingObj.gameObject.SetActive(false);
            _gmTS._fabButtons.SetActive(true);
            Debug.Log(_name);
            _button.GetComponent<UnitPanel>().DataUpdate();
            _name = null;
            _ready = false;
            _start = false;
            _button = null;
        }
    }
    void FabricatorRunning()
    {
        if (_start)
        {
            if (_tempTimer < _timer && !_ready)
            {
                _tempTimer += Time.deltaTime;
                _currentTimer = _tempTimer / _timer;
                if (_tempTimer >= _timer)
                {
                    _ready = true;
                    _tempTimer = 0;
                }
            }
        }
    }
    void Update()
    {
        FabricatorRunning();
    }
}
