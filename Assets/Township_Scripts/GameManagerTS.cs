using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum Directive
{
    _directive1, _directive2, _directive3, _directive4, _directive5
}
public class GameManagerTS : MonoBehaviour
{
    public Canvas _canvas;
    
    public static bool _nexusClick = false;
    public DText _dialogue;
    public GameObject _arrow;
    public static bool[] _checks = new bool[10];
    public bool[] _directive = new bool[4];
    public Transform[] _directivePos;
    Transform _campos;
    Camera _camera;
    
    public static Directive _currentDirective;
    public static GameManagerTS _gm;

    
    void Awake()
    {
        _camera = Camera.main;
        _campos = GameObject.Find("Cam").transform;
        _campos.position = _directivePos[0].position;
        for (int i = 0; i < _directive.Length; i++)
        {
            _directive[i] = false;
        }
        for (int i = 0; i < _checks.Length; i++)
        {
            _checks[i] = false;
        }
    }
    float _size = 20;
    public bool start = false;
    void Update()
    {
        PodCode();
        FabCode();
        if (start)
        {
            Directive();
        }
    }

    

    void Directive()
    {
        // 1
        if (!_directive[0] && _size >= 15f)
        {
            _size -= Time.deltaTime * 1.5f;
            _camera.orthographicSize = _size;
            if (_size <= 15f)
            {
                _directive[0] = true;
                ChangeDirective(global::Directive._directive1);
            }
        }
        if (!_nexusClick && _directive[0])
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit Hit))
            {
                if (Hit.collider.gameObject.CompareTag("Nexus"))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        _arrow.SetActive(false);
                        _dialogue.StartDialogue();
                        _nexusClick = true;
                    }
                }
            }
        }
        if (_checks[0])
        {
            //Isocamera._moveCam = true;
           _campos.position = Vector3.MoveTowards(_campos.position, _directivePos[1].position, Time.deltaTime * 10);
            if (_campos.position == _directivePos[1].position)
            {
                GameData._instance._xp += 20;
                ChangeDirective(global::Directive._directive2);
                _dialogue.StartDialogue();
                _directive[1] = true;
                _checks[0] = false;
            }
        }
        if (_directive[1])
        {
            _canvas.enabled = true;
        }
    }
    public static event Action<Directive> OnDirectiveChanged;
    public static void ChangeDirective(Directive newDirective)
    {
        _currentDirective = newDirective;
        OnDirectiveChanged?.Invoke(_currentDirective);
    }
    public LayerMask _pod;
    public Canvas _canvasPod;
    Pod _activePod;
    void PodCode()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit Hit))
        {
            if (((1 << Hit.collider.gameObject.layer) & _pod) != 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _activePod = Hit.collider.gameObject.GetComponent<Pod>();
                    if (!Pod.Click && !_activePod._cropReady && !_activePod.Start)
                    {
                        _canvasPod.enabled = true;
                        Pod.Click = true;
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0) && _canvasPod.enabled)
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        Pod.Click = false;
                        _canvasPod.enabled = false;
                    }
                }
            }
        }
    }
    public LayerMask _fab;
    public Canvas _canvasFab;
    public FabricatorUnit _activeFab;
    void FabCode()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit Hit))
        {
            if (((1 << Hit.collider.gameObject.layer) & _fab) != 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _activeFab = Hit.collider.gameObject.GetComponent<FabricatorUnit>();
                    if (!FabricatorUnit.Click && !_activeFab._start && !_activeFab._ready)
                    {
                        _canvasFab.enabled = true;
                        FabricatorUnit.Click = true;
                    }
                    //if (_activeFab._energyReady)
                    //{
                    //    GameData._instance._xp += 5;
                    //    Debug.Log("Crop Collected");
                    //    if (_activePod._activeCrop == "Nutri-Algae")
                    //    {
                    //        GameData._instance._nutriAlgaeCrop++;
                    //    }
                    //    if (_activePod._activeCrop == "Bio_Luminary")
                    //    {
                    //        GameData._instance._bioLuminaryCount++;
                    //    }
                    //    _activePod._activeCrop = string.Empty;
                    //    _activePod._cropReady = false;
                    //}
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0) && _canvasFab.enabled)
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        FabricatorUnit.Click = false;
                        _canvasFab.enabled = false;
                        if (!_activeFab._start && !_activeFab._ready)
                        {
                            _activeFab._fabricatingObj.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
    }
    IEnumerator Timer(bool Check)
    {
        yield return new WaitForSeconds(2);
        Check = true;
    }
    public void Farm(int crop)
    {
        switch (crop)
        {
            case 0:
                if(GameData._instance._qc >= GameData._instance._nutriAlgaePrice)
                {
                    _activePod.Nutri_Algae = true;
                    GameData._instance._qc -= GameData._instance._nutriAlgaePrice;
                    _canvasPod.enabled = false;
                }
                else
                {
                    Debug.Log("Insufficent Funds!");
                }
                break;
            case 1:
                if (GameData._instance._qc >= GameData._instance._bioLuminaryPrice)
                {
                    _activePod.Bio_Luminary = true;
                    GameData._instance._qc -= GameData._instance._bioLuminaryPrice;
                    _canvasPod.enabled = false; 
                }
                else
                {
                    Debug.Log("Insufficent Funds!");
                }
                break;
        }
    }
}
