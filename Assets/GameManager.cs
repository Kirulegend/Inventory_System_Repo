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
        if (_objDoor.position.y >= 4.5f && !_doorOpened)
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
}
