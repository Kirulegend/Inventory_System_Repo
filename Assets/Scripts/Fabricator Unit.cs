using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements.Experimental;

public class FabricatorUnit : MonoBehaviour
{
    public GameObject _nutri_Algae;
    public GameObject _bio_Luminary;

    public Image _energyBar;

    Canvas _canvas;

    float _timer = 25;
    float _tempTimer = 0;

    public bool _start = false;
    public bool _ready = false;



    void Awake()
    {
        _canvas = GetComponentInChildren<Canvas>();
    }
    public void Check(int Num)
    {
        switch (Num)
        {
            case 0:
                _nutri_Algae.SetActive(true);
                _bio_Luminary.SetActive(false);
                break;
            case 1:
                _bio_Luminary.SetActive(true);
                _nutri_Algae.SetActive(false);
                break;
        }
    }
    public void FabricatorClose()
    {
        if (!_start)
        {
            _nutri_Algae.SetActive(false);
            _bio_Luminary.SetActive(false);
        }
        _canvas.enabled = false;
    }
    void OnMouseUp()
    {
        if(!Placement._buildCheck) _canvas.enabled = true;
    }
    public void FabricatorStart()
    {
        if(_nutri_Algae.activeInHierarchy && !_start && !_ready && GameData._crop_Nutri_Algae >= 2)
        {
            GameData._crop_Nutri_Algae -= 2;
            _start = true;
            GameData._qc -= 25;
        }
        else if (_bio_Luminary.activeInHierarchy && !_start && !_ready && GameData._crop_Bio_Luminary >= 1)
        {
            GameData._crop_Bio_Luminary--;
            _start = true;
            GameData._qc -= 50;
            _timer *= 2;
        }
        else if (_ready)
        {
            GameData._xp += 10;
            GameData._energyBarCount++;
            _ready = false;
            _nutri_Algae.SetActive(false);
            _bio_Luminary.SetActive(false);
        }
    }
    void FabricatorRunning()
    {
        if (Input.GetMouseButtonDown(0) && _canvas.enabled)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (!_start)
                {
                    _nutri_Algae.SetActive(false);
                    _bio_Luminary.SetActive(false);
                }
                _canvas.enabled = false;
            }
        }
        if (_start && _tempTimer < _timer && !_ready)
        {
            Debug.Log("Hello");
            _tempTimer += Time.deltaTime;
            _energyBar.fillAmount = _tempTimer / _timer;
            if(_tempTimer >= _timer)
            {
                _start = false;
                _tempTimer = 0;
                _ready = true;
            }
        }
    }
    void Update()
    {
        FabricatorRunning();
    }
}
