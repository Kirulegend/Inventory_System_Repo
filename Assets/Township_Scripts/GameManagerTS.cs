using System;
using System.Collections;
using TMPro;
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
    Placement _placement;
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
    GameData _gameData;

    
    void Awake()
    {
        _placement = GameObject.Find("Ground")?.GetComponent<Placement>();
        _gameData = GameObject.Find("GameData")?.GetComponent<GameData>();
        _camera = Camera.main;
        _campos = GameObject.Find("Cam").transform;
        //_campos.position = _directivePos[0].position;
        for (int i = 0; i < _directive.Length; i++)
        {
            _directive[i] = false;
        }
        for (int i = 0; i < _checks.Length; i++)
        {
            _checks[i] = false;
        }
        _fabricatingObj = _canvasFab.gameObject.transform.Find("Fabricating_Obj").GetComponent<Image>();
        _fabricatingObj.gameObject.SetActive(false);
        _start = _canvasFab.gameObject.transform.Find("Start").GetComponent<Button>();
        _canvasFab.enabled = false;
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
        Level();
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
                _gameData._xp += 20;
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
                if (Input.GetMouseButtonDown(0) && !Placement._placed)
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
    FabricatorUnit _temp;
    public Image _fabricatingObj;
    public GameObject _fabButtons;
    public Button _start;
    void FabCode()
    {
        if (_activeFab)
        {
            _fabricatingObj.fillAmount = _activeFab._currentTimer;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit Hit))
        {
            if (((1 << Hit.collider.gameObject.layer) & _fab) != 0)
            {
                if (Input.GetMouseButtonUp(0) && !_placement._editBuild && !Placement._placed)
                {
                    if (_activeFab) _temp = _activeFab;
                    _activeFab = Hit.collider.gameObject.GetComponent<FabricatorUnit>();
                    if (_temp && _temp != _activeFab)
                    {
                        _canvasFab.enabled = false;
                        FabricatorUnit.Click = false;
                    }
                    if (!FabricatorUnit.Click)
                    {
                        _fabricatingObj.sprite = _activeFab._fabricatingObjSprite;
                        _fabricatingObj.transform.Find("BG").GetComponent<Image>().sprite = _activeFab._fabricatingObjSprite;
                        _canvasFab.enabled = true;
                        _start.onClick.AddListener(_activeFab.FabricatorStart);
                        if (!_activeFab._start || !_activeFab._ready)
                        {
                            _fabricatingObj.gameObject.SetActive(false);
                            _fabButtons.SetActive(true);        
                        }
                        if (_activeFab._start || _activeFab._ready)
                        {
                            _fabricatingObj.gameObject.SetActive(true);
                            _fabButtons.SetActive(false);
                        }
                        FabricatorUnit.Click = true;
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0) && _canvasFab.enabled)
                {
                    if (!EventSystem.current.IsPointerOverGameObject())
                    {
                        FabricatorClose();
                    }
                }
            }
        }
    }
    public void FabricatorClose()
    {
        _canvasFab.enabled = false;
        FabricatorUnit.Click = false;
        if (!_activeFab._start && !_activeFab._ready) _fabricatingObj.gameObject.SetActive(false);
        _start.onClick.RemoveAllListeners();
        _activeFab = null;
    }
    IEnumerator Timer(bool Check)
    {
        yield return new WaitForSeconds(2);
        Check = true;
    }
    public void Farm(string crop)
    {
        Debug.Log(crop);
        if (_gameData._qc >= _gameData._invI.Find(item => item.name == crop).price)
        {
            _activePod._activeCrop = crop;
            _gameData._qc -= _gameData._invI.Find(item => item.name == crop).price;
            _canvasPod.enabled = false;
            _activePod.Check();
        }
        else Debug.Log("Insufficent Funds!");
    }
    void Level()
    {
        if(_gameData._level * 100 == _gameData._xp)
        {
            _gameData._level++;
        }
    }
}
