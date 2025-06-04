using System.Threading;
using Unity.Hierarchy;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class FabricatorUnit : MonoBehaviour
{
    public static FabricatorUnit Instance;
    public Image _fabricatingObj;
    public Sprite _fabricatingObjSprite;
    public string _name = null;    

    Canvas _canvas;
    public GameObject _buttonParent;

    [HideInInspector] public float _timer;
    [HideInInspector] public Transform _button;
    float _tempTimer = 0;

    public bool _start = false;
    public bool _ready = false;
    public static bool Click = false;

    //void Awake()
    //{
    //    _fabricatingObj = GameObject.FindGameObjectsWithTag("Fabricating_Obj_BG")<Image>();
    //    _fabricatingObj.gameObject.SetActive(false);
    //}
    //void OnMouseUp()
    //{
    //    if(!Placement._buildCheck) _canvas.enabled = true;
    //}
    //void OnMouseDown()
    //{
    //    if (!_canvas.enabled && !EventSystem.current.IsPointerOverGameObject())
    //    {
    //        _canvas.enabled = true;
    //        wasOpenedThisFrame = true;
    //    }
    //}
    bool wasOpenedThisFrame;
    //void AutoClose()
    //{
    //    if (_canvas.enabled)
    //    {
    //        if (wasOpenedThisFrame)
    //        {
    //            wasOpenedThisFrame = false;
    //            return;
    //        }
    //        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
    //        {
    //            if (!_start && !_ready)
    //            {
    //                _fabricatingObj.gameObject.SetActive(false);
    //            }
    //            _canvas.enabled = false;
    //        }
    //    }
    //}
    public void FabricatorStart()
    {
        if (!_start && _name != null && !_ready && _fabricatingObj.gameObject.activeInHierarchy)
        {
            _start = true;
            //_buttonParent.SetActive(false);
            _button.GetComponent<UnitPanel>().DataUpdate();
        }   
        else if (_ready)
        {
            _fabricatingObj.gameObject.SetActive(false);
            Debug.Log(_name);
            _button.GetComponent<UnitPanel>().DataUpdate();
            _name = null;
            //_buttonParent.SetActive(true);
            _ready = false;
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
                _fabricatingObj.fillAmount = _tempTimer / _timer;
                if (_tempTimer >= _timer)
                {
                    _ready = true;
                    _tempTimer = 0;
                    _start = false;
                }
            }
        }
    }
    void Update()
    {
        //AutoClose();
        FabricatorRunning();
    }
}
