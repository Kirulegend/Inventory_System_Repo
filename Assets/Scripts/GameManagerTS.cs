using JetBrains.Annotations;
using RPG;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.LookDev;
using UnityEngine.UI;
using static UnityEditor.VersionControl.Asset;

public class GameManagerTS : MonoBehaviour
{
    public Canvas _canvas;
    public Canvas _canvasPod;
    public static bool _nexusClick = false;
    public DText _dialogue;
    public TextMeshProUGUI _qcText;
    public Slider _xpSlider;
    public static int _qc = 500;
    public static float _xp = 0;
    public GameObject _arrow;
    public static bool[] _checks = new bool[10];
    public bool[] _directive = new bool[4];
    public Transform[] _directivePos;
    public Transform _campos;
    Camera _camera;
    public static int _level = 1;
    public LayerMask _pod;
    Pod _activePod;
    public TextMeshProUGUI _road;
    public TextMeshProUGUI _build;
    public TextMeshProUGUI _podCrop;

    void Awake()
    {
        _camera = Camera.main;
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
    void Update()
    {
        if (!_directive[0] && _size >= 15f)
        {
            _size -= Time.deltaTime * 1.5f;
            _camera.orthographicSize = _size;
            if(_size <= 15f)
            {
                _directive[0] = true;
            }
        }
        if (!_nexusClick && _directive[0])
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit Hit))
            {
                Debug.Log(Hit.collider.gameObject.name);
                if (Hit.collider.gameObject.CompareTag("Nexus"))
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        _arrow.SetActive(false);
                        _dialogue.StartDialogue();
                        _nexusClick = true;
                    }
                }
                if (((1 << Hit.collider.gameObject.layer) & _pod) != 0)
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        _activePod = Hit.collider.gameObject.GetComponent<Pod>();
                        if(!Pod.Click && !_activePod._cropReady && !_activePod.Start)
                        {
                            _canvasPod.gameObject.SetActive(true);
                            Pod.Click = true;
                        }
                        if (_activePod._cropReady)
                        {
                            _xp += 5;
                            Debug.Log("Crop Collected");
                            Pod._cropCount++;
                            _activePod._cropReady = false;
                        }
                    }
                }
                else
                {
                    if (Input.GetMouseButtonDown(0) && _canvasPod.gameObject.activeInHierarchy)
                    {
                        if (!EventSystem.current.IsPointerOverGameObject())
                        {
                            Pod.Click = false;
                            _canvasPod.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }
        if (_checks[0])
        {
            //Isocamera._moveCam = true;
            //_canvas.enabled = true;
            _campos.position = Vector3.MoveTowards(_campos.position, _directivePos[1].position, Time.deltaTime * 10);
            _xp += 20;
            if (_campos.position == _directivePos[1].position)
            {
                _directive[1] = true;
                _checks[0] = false;
            }
        }
        if(_directive[1])
        {
            
        }
        UI();
    }
    IEnumerator Timer(bool Check)
    {
        yield return new WaitForSeconds(2);
        Check = true;
    }
    public int Nutri_Algae = 25;
    public int Bio_Luminary = 50;
    public void Farm(int crop)
    {
        switch (crop)
        {
            case 0:
                if(_qc >= Nutri_Algae)
                {
                    _activePod.Nutri_Algae = true;
                    _qc -= Nutri_Algae;
                    _canvasPod.gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("Insufficent Funds!");
                }
                break;
            case 1:
                if (_qc >= Bio_Luminary)
                {
                    _activePod.Bio_Luminary = true;
                    _qc -= Bio_Luminary;
                    _canvasPod.gameObject.SetActive(false);
                }
                else
                {
                    Debug.Log("Insufficent Funds!");
                }
                break;
        }
    }
    void UI()
    {
        _road.text = "ROADS : " + Placement._roadCount.ToString();
        _build.text = "BUILD : " + Placement._buildCount.ToString();
        _podCrop.text = "CROPS : " + Pod._cropCount.ToString();

        _qcText.text = _qc.ToString();

        _xpSlider.maxValue = _level;
        _xpSlider.value = _xp / 100;
    }
}
