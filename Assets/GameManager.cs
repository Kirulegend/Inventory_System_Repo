using System.Collections;
using System.Drawing;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;

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
    public GameObject _wall;
    public GameObject _floor;
    public GameObject _ramp;
    public GameObject _tempObj;
    public GameObject _tempobj;
    Renderer _tempRend;
    BoxCollider _tempColB;
    public UI_Inventory _uiInv;
    public static bool _isScope = false;
    public Image _weapon;
    public Sprite _gun;
    public Sprite _hand;
    public Sprite _build;
    public GameObject _unlimited;
    public GameObject _buildBlock;
    public static bool _unlimitedAmmo = false;
    public static bool _isGun = false;
    public static bool _isBuild = false;
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
    int _invCountNum = 50;
    int _jumpCountNum = 50;
    int _telCountNum = 50;
    bool _preview = false;
    public Material _previewMatG;
    public Material _previewMatR;
    public Material _defaultMat;
    Vector3 pos;
    public static bool _isCol = false;

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
        Damage();
        Abilities();
        build();
    }
    void Door()
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
    void Crosshair()
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
            CamInputRotation.y = Mathf.Clamp(CamInputRotation.y, -40, 40f);
            _rb3D.MoveRotation(Quaternion.Euler(-CamInputRotation.y, CamInputRotation.x, 0));
        }
    }
    void Scope()
    {
        if (_isGun)
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
        if (_uiInv._aniIndex != 2 && !_preview)
        {
            if ((Input.GetKeyDown(KeyCode.Alpha1) || _telPreview) && _weapon.sprite != _hand)
            {
                _isBuild = false;
                _weapon.sprite = _hand;
                _isGun = false;
                _bulletCountText.enabled = false;
                _unlimited.SetActive(true);
                _buildBlock.SetActive(false);
            }
            if (Input.GetKeyDown(KeyCode.Alpha2) && _weapon.sprite != _gun)
            {
                _isBuild = false;
                _isGun = true;
                _bulletCountText.enabled = true;
                _unlimited.SetActive(false);
                _buildBlock.SetActive(false);
                _weapon.sprite = _gun;
            }
            if(Input.GetKeyDown(KeyCode.Alpha3) && _weapon.sprite != _build)
            {
                _isBuild = true;
                _isGun = false;
                _unlimited.SetActive(true);
                _buildBlock.SetActive(true);
                _bulletCountText.enabled = false;
                _weapon.sprite = _build;
            }
            if (Input.GetKeyDown(KeyCode.U) && _weapon.sprite == _gun)
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
    void build()
    {
        pos = _player._hitPos;
        if (_isBuild)
        {
            if (Input.GetKeyDown(KeyCode.X) && !_preview)
            {
                _tempobj = _wall;
            }
            if (Input.GetKeyDown(KeyCode.C) && !_preview)
            {
                _tempobj = _ramp;
            }
            if (Input.GetKeyDown(KeyCode.V) && !_preview)
            {
                _tempobj = _floor;
            }
            if (_preview)
            {
                if (_isCol)
                {
                    _tempRend.material = _previewMatR;
                }
                else
                {
                    _tempRend.material = _previewMatG;
                }

                if (Input.GetKeyDown(KeyCode.R))
                {
                    _tempObj.transform.Rotate(0, 45, 0);
                }
                if (Input.GetMouseButtonDown(0) && !_isCol)
                {
                    _tempRend.material = _defaultMat;
                    _tempObj.transform.Find("Cube").gameObject.layer = 0;
                    _tempColB.isTrigger = false;
                    _tempobj = null;
                    _preview = false;
                }
                if (Input.GetMouseButtonDown(1))
                {
                    Destroy(_tempObj);
                    _tempobj = null;
                    _preview = false;
                }
            }
            if (_tempobj != null)
            {
                Preview(_tempobj);
                _tempObj.transform.position = pos;
            }
        }
    }
    void Preview(GameObject Obj)
    {
        if (!_preview)
        {
            _tempObj = Instantiate(Obj, pos, Quaternion.identity);
            _tempObj.transform.Find("Cube").gameObject.layer = 6;
            _tempRend = _tempObj.transform.Find("Cube").GetComponent<Renderer>();
            _tempColB = _tempObj.transform.Find("Cube").GetComponent<BoxCollider>();
            _tempColB.isTrigger = true;
            _defaultMat = _tempRend.material;
            _preview = true;
        }
    }
    void Abilities()
    {
        //UI count Display
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

        //Jump-pad Logic
        if (Input.GetKeyDown(KeyCode.Q) && !_isJump && _jumpCountNum >= 1 && !_preview)
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

        //Teleporter Logic
        if (Input.GetKeyDown(KeyCode.E) && !_Teleporter.activeInHierarchy && _telCountNum >= 1 && !_preview && !_isScope)
        {
            _Teleporter.transform.SetParent(_playerPos);
            _Teleporter.transform.localRotation = Quaternion.identity;
            _tel.color = new Color32(255, 255, 255, 255);
            _telPreview = true;
            _Teleporter.SetActive(true);
            _Teleporter.transform.position = _TelTargetPos.position;
        }
        if (_Teleporter.activeInHierarchy)
        {
            Rigidbody _TeleporterRigi = _Teleporter.GetComponent<Rigidbody>();
            _Teleporter.transform.rotation = Quaternion.Euler(0, _Teleporter.transform.eulerAngles.y, 0);
            if (Input.GetMouseButtonDown(0) && _telPreview)
            {
                _TeleporterRigi.useGravity = true;
                _TeleporterRigi.isKinematic = false;
                _tel.color = new Color32(45, 45, 45, 125);
                _Teleporter.transform.SetParent(null);
                _telCountNum--;
                _telPreview = false;
            }
            if (!_telPreview)
            {
                _TeleporterRigi.AddForce(_Teleporter.transform.forward * .5f, ForceMode.Impulse);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    _TeleporterRigi.useGravity = false;
                    _TeleporterRigi.isKinematic = true;
                    _rb3D.MovePosition(_Teleporter.transform.position);
                    _tel.color = new Color32(45, 45, 45, 255);
                    _Teleporter.SetActive(false);
                }
            }
        }


        //// Phase 1: Enter Teleporter Preview Mode
        //if (Input.GetKeyDown(KeyCode.E) && !_telPreview && !_Teleporter.activeInHierarchy && _telCountNum >= 1 && !_preview)
        //{
        //    _Teleporter.transform.SetParent(_playerPos);
        //    _Teleporter.transform.localRotation = Quaternion.identity;
        //    _Teleporter.transform.localPosition = Vector3.zero; // Ensure it starts at the player's position
        //    _tel.material.color = new Color32(255, 255, 255, 255); // Access material.color for Renderer
        //    _Teleporter.SetActive(true);
        //    _telPreview = true;
        //    _isTel = false; // Reset launch state
        //}

        //// Phase 2: Position and Launch the Teleporter
        //if (_telPreview)
        //{
        //    _Teleporter.transform.position = _TelTargetPos.position; // Update position during preview

        //    Rigidbody _TeleporterRigi = _Teleporter.GetComponent<Rigidbody>();
        //    if (Input.GetMouseButtonDown(0))
        //    {
        //        _TeleporterRigi.useGravity = true;
        //        _TeleporterRigi.isKinematic = false;
        //        _tel.material.color = new Color32(45, 45, 45, 125); // Access material.color for Renderer
        //        _telCountNum--;
        //        _telPreview = false; // Exit preview mode
        //        _isTel = true; // Mark as launched
        //        _Teleporter.transform.SetParent(null); // Detach immediately on launch
        //        _TeleporterRigi.AddForce(_camPos.forward * 10f, ForceMode.Impulse); // Increased force for better visibility
        //    }

        //    // Cancel preview (optional - if you want to cancel placement)
        //    //if (Input.GetKeyDown(KeyCode.E))
        //    //{
        //    //    _Teleporter.SetActive(false);
        //    //    _telPreview = false;
        //    //}
        //}

        //// Phase 3: Teleport the Player
        //if (_isTel && Input.GetKeyDown(KeyCode.E) && _Teleporter.activeInHierarchy)
        //{
        //    Rigidbody _TeleporterRigi = _Teleporter.GetComponent<Rigidbody>();
        //    _TeleporterRigi.useGravity = false;
        //    _TeleporterRigi.isKinematic = true;
        //    Vector3 _tempPos = _Teleporter.transform.position;
        //    _playerPos.position = _tempPos;
        //    _tel.material.color = new Color32(45, 45, 45, 255); // Access material.color for Renderer
        //    _Teleporter.SetActive(false);
        //    _isTel = false; // Reset launch state
        //}

        //Invisiable Logic
        if (Input.GetKeyDown(KeyCode.X) && !_isInv && _invCountNum >= 1 && !_preview)
        {
            _inv.color = new Color32(255, 255, 255, 255);
            _meshRenderer =  _playerPos.gameObject.GetComponent<MeshRenderer>();
            _meshRenderer.enabled = false;
            StartCoroutine(Inv());
        }
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
