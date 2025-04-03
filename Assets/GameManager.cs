using System.Collections;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
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
    Vector2 CamInputRotation;
    public Rigidbody _rb3D;
    public Player _player;
    int Count = 0;
    Vector3 targetPosition;
    bool _pressedE = false;
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
    public Image _inv;
    public Image _jump;
    public Image _tel;
    public static bool _isTel = false;
    public static bool _isJump = false;
    public static bool _isInv = false;
    public static bool _telPreview = false;
    public MeshRenderer _meshRenderer;
    public Transform _camPos;
    public Transform _playerPos;
    public Transform _TelTargetPos;
    public GameObject _Teleporter;
    public GameObject _jumpPad;
    public GameObject _JumpPad;
    public float _jumpPadforce;
    Vector3 rotationAxis;
    Quaternion rotation;
    Vector3 shootDirection;
    Vector3 spawnPosition;
    public Volume _vol;
    public Vignette _vig;
    public TextMeshProUGUI _invCount;
    public TextMeshProUGUI _jumpCount;
    public TextMeshProUGUI _telCount;
    int _invCountNum = 10;
    int _jumpCountNum = 10;
    int _telCountNum = 10;

    void Start()    
    {
        _rb3D = _player.GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.None;
    }
    void Update()
    {
        Door();
        CameraRot();
        Crosshair();
        Scope();
        Weapon();
        Abilities();
        Damage();
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
        if (Input.GetKeyDown(KeyCode.BackQuote))
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
            CamInputRotation.y = Mathf.Clamp(CamInputRotation.y, -40f, 40f);
            _rb3D.MoveRotation(Quaternion.Euler(Mathf.Clamp(-CamInputRotation.y, -40, 40), CamInputRotation.x, 0));
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
                _camera.fieldOfView = Mathf.SmoothStep(20, 60, 2f);
                _canvas.transform.localScale = Vector3.Lerp(new Vector3(0.00061556f, 0.00061556f, 0.00061556f), new Vector3(0.001603751f, 0.001603751f, 0.001603751f), 2f);
            }
            else if (_isScope && _camera.fieldOfView != 20)
            {
                _scope.SetActive(true);
                _camera.fieldOfView = Mathf.SmoothStep(60, 20, 2f);
                _canvas.transform.localScale = Vector3.Lerp(new Vector3(0.001603751f, 0.001603751f, 0.001603751f), new Vector3(0.00061556f, 0.00061556f, 0.00061556f), 2f);
            }
        }
    }
    void Weapon()
    {
        _bulletCountText.text = Player._tempBulletCount.ToString();
        if (_uiInv._aniIndex != 2)
        {
            if ((Input.GetKeyDown(KeyCode.Alpha1) || _telPreview) && _weapon.sprite != _hand)
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
    void Abilities()
    {
        _invCount.text = _invCountNum.ToString();
        _jumpCount.text = _jumpCountNum.ToString();
        _telCount.text = _telCountNum.ToString();
        if(_invCountNum == 0)
        {
            _inv.color = new Color32(45, 45, 45, 125);
        }
        if (_jumpCountNum == 0)
        {
            _jump.color = new Color32(45, 45, 45, 125);
        }
        if (_telCountNum == 0)
        {
            _tel.color = new Color32(45, 45, 45, 125);
        }
        if (Input.GetKeyDown(KeyCode.Q) && !_isJump && _jumpCountNum >= 1)
        {
            _isJump = true;
        }
        if (_isJump)
        {
            _jump.color = new Color32(255, 255, 255, 255);
            rotationAxis = _playerPos.forward;
            rotation = Quaternion.AngleAxis(_playerPos.localRotation.x, rotationAxis);
            shootDirection = rotation * _playerPos.forward;
            spawnPosition = new Vector3(_playerPos.position.x, _playerPos.position.y + 1f, _playerPos.position.z) + shootDirection * 1f;
            _JumpPad = Instantiate(_jumpPad, spawnPosition, Quaternion.LookRotation(shootDirection));
            Rigidbody _JumpPadRigi = _JumpPad.GetComponent<Rigidbody>();
            _JumpPadRigi.AddForce(_playerPos.forward * 250f, ForceMode.Force);
            _JumpPadRigi.AddForce(Physics.gravity * 2f, ForceMode.Acceleration);
            CapsuleCollider _tempCollider = _JumpPad.GetComponent<CapsuleCollider>();
            StartCoroutine(Jump(_tempCollider));
            _jumpCountNum--;
            Destroy(_JumpPad, 10);
            _isJump = false;
        }
        if (Input.GetKeyDown(KeyCode.E) && _isTel == false && _Teleporter == null && _telCountNum >= 1)
        {
            _tel.color = new Color32(255, 255, 255, 255);
            _telPreview = true;
            _isTel = true;
        }
        if (_isTel)
        {
            //rotationAxis = _camPos.forward;
            //rotation = Quaternion.AngleAxis(_camPos.localRotation.x, rotationAxis);
            //shootDirection = rotation * _playerPos.forward;
            //spawnPosition = new Vector3(_playerPos.position.x , _playerPos.position.y + 1f, _playerPos.position.z) + shootDirection * 1f;
            _Teleporter.transform.position = _TelTargetPos.position;
            //_Teleporter = Instantiate(_teleporter, spawnPosition, Quaternion.LookRotation(shootDirection), _playerPos);
            _isTel = false;
        }
        if(_Teleporter != null && _isTel == false)
        {
            Rigidbody _TeleporterRigi = _Teleporter.GetComponent<Rigidbody>();
            if (Input.GetMouseButtonDown(0) && !_TeleporterRigi.useGravity)
            {
                rotationAxis = _camPos.forward;
                rotation = Quaternion.AngleAxis(_camPos.localRotation.x, rotationAxis);
                shootDirection = rotation * _playerPos.forward;
                _TeleporterRigi.useGravity = true;
                _TeleporterRigi.isKinematic = false;
                _tel.color = new Color32(45, 45, 45, 125);
                _telCountNum--;
                Destroy(_Teleporter, 20f);
            }
            if (_TeleporterRigi.useGravity)
            {
                StartCoroutine(TeleporterPreview());
                if (_Teleporter.transform.parent != null)
                {
                    _Teleporter.transform.SetParent(null);
                }
                _TeleporterRigi.AddForce(shootDirection * 1.25f, ForceMode.Force);
            }
            if (Input.GetKeyDown(KeyCode.E) && _TeleporterRigi.useGravity && _Teleporter.transform.parent == null)
            {
                _TeleporterRigi.linearVelocity = Vector3.zero;
                _TeleporterRigi.angularVelocity = Vector3.zero;
                Vector3 _tempPos = _Teleporter.transform.position;
                _playerPos.position = _tempPos;
                _tel.color = new Color32(45, 45, 45, 255);
                Destroy(_Teleporter, 0.1f);
                if (_Teleporter != null)
                {
                    Destroy(_Teleporter, 0.1f);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.X) && !_isInv && _invCountNum >= 1)
        {
            _inv.color = new Color32(255, 255, 255, 255);
            _meshRenderer =  _playerPos.gameObject.GetComponent<MeshRenderer>();
            _meshRenderer.enabled = false;
            StartCoroutine(Inv());
        }
    }
    IEnumerator TeleporterPreview()
    {
        yield return new WaitForSeconds(2.5f);
        _telPreview = false;
    }
    IEnumerator Inv()
    {
        _inv.color = new Color32(255, 255, 255, 255);
        _isInv = true;
        _invCountNum--;
        yield return new WaitForSeconds(10);
        _meshRenderer.enabled = true;
        _isInv = false;
        _inv.color = new Color32(45, 45, 45, 255);
    }
    IEnumerator Jump(CapsuleCollider _tempCollider)
    {
        yield return new WaitForSeconds(.1f);
        _jump.color = new Color32(45, 45, 45, 255);
        _tempCollider.enabled = true;
    }
    void Damage()
    {
        if(Player._healthPower > 5)
        {
            _vol.profile.TryGet(out _vig);
            _vig.intensity.Override(0);
        }
        if(Player._healthPower == 5)
        {
            float _temp = Mathf.SmoothStep(0.0f, 0.1f, 2f);
            _vol.profile.TryGet(out _vig);
            _vig.intensity.Override(_temp);
        }
        if (Player._healthPower == 4)
        {
            float _temp = Mathf.SmoothStep(0.1f, 0.2f, 2f);
            _vol.profile.TryGet(out _vig);
            _vig.intensity.Override(_temp);
        }
        if (Player._healthPower == 3)
        {
            float _temp = Mathf.SmoothStep(0.2f, 0.3f, 2f);
            _vol.profile.TryGet(out _vig);
            _vig.intensity.Override(_temp);
        }
        if (Player._healthPower == 2)
        {
            float _temp = Mathf.SmoothStep(0.3f, 0.4f, 2f);
            _vol.profile.TryGet(out _vig);
            _vig.intensity.Override(_temp);
        }
        if (Player._healthPower == 1)
        {
            float _temp = Mathf.SmoothStep(0.4f, 0.5f, 2f);
            _vol.profile.TryGet(out _vig);
            _vig.intensity.Override(_temp);
        }
    }
}
