using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool _hasKey = false;
    public static bool _nearDoor = false;
    public Transform _objDoor;
    private bool _doorOpened = false;
    public Sprite _red;
    public Sprite _green;
    public Sprite _black;
    public Sprite _white;
    public Image _crossHair;
    public static bool _redB;
    public static bool _greenB;
    public static bool _blackB;
    public Vector2 CamInputRotation;
    public Rigidbody _rb3D;
    public Player _player;
    public int Count = 0;
    public Vector3 targetPosition;
    private bool _pressedE = false;
    public static int _bulletCount = 30;
    public TextMeshProUGUI _bulletCountText;
    public Camera _camera;
    public GameObject _canvas;
    public GameObject _scope;
    public UI_Inventory _uiInv;
    public static bool _isScope = false;
    public Image _weapon;
    public Sprite _gun;
    public Sprite _hand;
    public GameObject _unlimited;
    public static bool _unlimitedAmmo = false;
    public static bool _isHand = false;
    void Start()
    {
        _rb3D = _player.GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.None;
    }
    void Update()
    {
        _bulletCountText.text = Player._tempBulletCount.ToString();
        Door();
        CameraRot();
        Crosshair();
        Scope();
        Weapon();
    }
    public void Door()
    {
        if (_hasKey && !_doorOpened && Input.GetKeyDown(KeyCode.E))
        {
            _pressedE = true;
        }
        if(_pressedE)
        {
            _objDoor.position = Vector3.Lerp(_objDoor.position, targetPosition, 2 * Time.deltaTime);
        }
        if (_objDoor.position.y >= 4.4f && !_doorOpened)
        {
            _pressedE = false;
            _doorOpened = true;
            _hasKey = false;
        }
    }
    public void Crosshair()
    {
        if (_blackB)
        {
            _crossHair.sprite = _black;
        }
        else if (_redB)
        {
            _crossHair.sprite = _red;
        }
        else if (_greenB)
        {
            _crossHair.sprite = _green;
        }
        else if(!_blackB && !_greenB && !_redB)
        {
            _crossHair.sprite = _white;
        }
    }
    void CameraRot()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (Count == 0)
            {
                Count++;
                Cursor.lockState = CursorLockMode.Locked;
            }
            else if (Count == 1)
            {
                Count--;
                Cursor.lockState = CursorLockMode.None;
            }
        }
        if (Count == 1)
        {
            CamInputRotation.x += Input.GetAxis("Mouse X");
            CamInputRotation.y += Input.GetAxis("Mouse Y");
            CamInputRotation.y = Mathf.Clamp(CamInputRotation.y, -30f, 30f);
            _rb3D.MoveRotation(Quaternion.Euler(Mathf.Clamp(-CamInputRotation.y, -30, 30), CamInputRotation.x, 0));
        }
    }
    void Scope()
    {
        if (!_isHand)
        {
            if (Input.GetMouseButtonDown(1) && _uiInv._aniIndex != 2)
            {
                _isScope = true;
            }
            if (Input.GetMouseButtonUp(1) && _uiInv._aniIndex != 2)
            {
                _isScope = false;
            }
            if (!_isScope && _camera.fieldOfView != 60)
            {
                _scope.SetActive(false);
                _camera.fieldOfView = Mathf.SmoothStep(25, 60, 2f);
                _canvas.transform.localScale = Vector3.Lerp(new Vector3(0.00061556f, 0.00061556f, 0.00061556f), new Vector3(0.001603751f, 0.001603751f, 0.001603751f), 2f);
            }
            else if (_isScope && _camera.fieldOfView != 25)
            {
                _scope.SetActive(true);
                _camera.fieldOfView = Mathf.SmoothStep(60, 25, 2f);
                _canvas.transform.localScale = Vector3.Lerp(new Vector3(0.001603751f, 0.001603751f, 0.001603751f), new Vector3(0.00061556f, 0.00061556f, 0.00061556f), 2f);
            }
        }
    }
    void Weapon()
    {
        if (_uiInv._aniIndex != 2)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1) && _weapon.sprite != _hand)
            {
                _weapon.sprite = _hand;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && _weapon.sprite != _gun)
            {
                _bulletCountText.enabled = true;
                _unlimited.SetActive(false);
                _weapon.sprite = _gun;
            }
            if (_weapon.sprite == _hand)
            {
                _isHand = true;
                _bulletCountText.enabled = false;
                _unlimited.SetActive(true);
            }
            if (_weapon.sprite == _gun)
            {
                _isHand = false;
                if (Input.GetKeyDown(KeyCode.U))
                {
                    if (_unlimitedAmmo)
                    {
                        _bulletCountText.enabled = true;
                        _unlimited.SetActive(false);
                        _unlimitedAmmo = false;
                    }
                    else
                    {
                        _bulletCountText.enabled = false;
                        _unlimited.SetActive(true);
                        _unlimitedAmmo = true;
                    }
                }
            }
        }
    }
}
