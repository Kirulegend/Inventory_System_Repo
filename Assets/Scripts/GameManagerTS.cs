using RPG;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.VersionControl.Asset;

public class GameManagerTS : MonoBehaviour
{
    public Canvas _canvas;
    public static bool _nexusClick = false;
    public DText _dialogue;
    public TextMeshProUGUI _qc;
    public Slider _xp;
    public static int _qcText = 500;
    public static float _xpText = 0;
    public GameObject _arrow;
    public static bool[] _checks = new bool[10];
    public bool[] _directive = new bool[4];
    Camera _camera;
    public static int _level = 1;

    void Awake()
    {
        _camera = Camera.main;
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
        if (_checks[0] && _xp.value == 0)
        {
            Isocamera._moveCam = true;
            _canvas.enabled = true;
            _xpText += 20f;
        }
        XP();
        QC();
    }

    void QC()
    {
        _qc.text = _qcText.ToString();
    }
    void XP()
    {
        _xp.maxValue = _level;
        _xp.value = _xpText/ 100;
    }
}
