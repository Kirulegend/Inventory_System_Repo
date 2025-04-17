using System.Collections;
using System.Drawing;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    void Start()    
    {
        _player = FindFirstObjectByType<Player>();
        _uiInv = FindFirstObjectByType<UI_Inventory>();
        _playerPos = _player.transform;
        //_sj = _playerPos.GetComponent<SpringJoint>();
        //_lineRen  = GetComponent<LineRenderer>();
        //_lineRen.enabled = false;
        _rb3D = _player.GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        Door();
        //CameraRot();
        Crosshair();
        Scope();
        Weapon();
        Damage();
        Abilities();
        build();
        Slowmo();
        //Rope();
    }

    //[Header("Grapple Mech")]
    //public SpringJoint _sj;
    //public static bool _isRope = false;
    //public GameObject _ropePoint;
    //public Renderer _tempRopeRen;
    //public Material _defaultRopeMat;
    //public LineRenderer _lineRen;
    //void Rope()
    //{
    //    //_sj.anchor = transform.localPosition;
    //    if(_ropePoint != null)
    //    {   
    //        if (_isRope)
    //        {
    //            _tempRopeRen.material = _previewMatG;
    //            if (Input.GetMouseButtonDown(0))
    //            {
    //                _lineRen.enabled = true;
    //                if (_sj != null)
    //                {
    //                    Destroy(_sj);
    //                }
    //                _sj = _playerPos.gameObject.AddComponent<SpringJoint>();
    //                //_sj.autoConfigureConnectedAnchor = false;
    //                _sj.connectedBody = _ropePoint.GetComponent<Rigidbody>();
    //                _sj.connectedAnchor = _ropePoint.transform.position;

    //            }
    //        }
    //        if (Input.GetMouseButtonDown(1))
    //        {
    //            _lineRen.enabled = false;
    //            if (_sj != null)
    //            {
    //                Destroy(_sj);
    //            }
    //        }
    //        _lineRen.positionCount = 2;
    //        _lineRen.SetPosition(0, _playerPos.position);
    //        _lineRen.SetPosition(1, _ropePoint.transform.position);
    //    }
    //    if (_lineRen.enabled)
    //    {
    //        _ropePoint.layer = 6;
    //        float distanceToHook = Vector3.Distance(_playerPos.position, _ropePoint.transform.position);
    //        if (distanceToHook < 1f)
    //        {
    //            _lineRen.enabled = false;
    //            _ropePoint.layer = 0;
    //            if (_sj != null)
    //            {
    //                Destroy(_sj);
    //            }
    //        }
    //        //Vector3 _tempPos = new Vector3(_ropePoint.transform.position.x, _ropePoint.transform.position.y + 1, _ropePoint.transform.position.z);
    //        //_playerPos.position = Vector3.MoveTowards(_playerPos.position, _tempPos, .1f);
    //        //if(_playerPos.position == new Vector3(_ropePoint.transform.position.x, _ropePoint.transform.position.y + 1, _ropePoint.transform.position.z))
    //        //{
    //        //    _lineRen.enabled = false;
    //        //    _ropePoint.layer = 0;
    //        //}
    //    }
    //}

    [Header("Door")]
    [Tooltip("The door which need key")]
    public Transform _objDoor;
    public static bool _hasKey = false;
    public static bool _nearDoor = false;
    bool _doorOpened = false;
    Vector3 _targetPosition;
    bool _pressedE = false;

    void Door()
    {
        if (_hasKey && !_doorOpened && Input.GetKeyDown(KeyCode.E))
        {
            _pressedE = true;
        }
        if(_pressedE)
        {
            _objDoor.position = Vector3.Lerp(_objDoor.position, _targetPosition, 2 * Time.deltaTime);
        }
        if (_objDoor.position.y >= 4.4f && !_doorOpened)
        {
            _pressedE = false;
            _doorOpened = true;
            _hasKey = false;
        }
    }

    [Header("Crosshair Mech")]
    [Tooltip("Add the Red Crosshair")]
    public Sprite _red;
    [Tooltip("Add the Green Crosshair")]
    public Sprite _green;
    [Tooltip("Add the Black Crosshair")]
    public Sprite _black;
    [Tooltip("Add the White Crosshair")]
    public Sprite _white;
    [Tooltip("Add the Crosshair Component")]
    public Image _crossHair;
    public static bool _redB;
    public static bool _greenB;
    public static bool _blackB;

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

    [Header("CameraRot Mech")]
    Transform _playerPos;
    Player _player;
    Rigidbody _rb3D;
    Vector2 CamInputRotation;
    int Count = 0;

    void CameraRot()
    {
        if (Input.GetKeyDown(KeyCode.BackQuote) || Input.GetKeyDown(KeyCode.Escape))
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
        if (Count == 1 && Time.timeScale == 1)
        {
            CamInputRotation.x += Input.GetAxis("Mouse X");
            CamInputRotation.y += Input.GetAxis("Mouse Y");
            CamInputRotation.y = Mathf.Clamp(CamInputRotation.y, -60, 60f);
            _rb3D.MoveRotation(Quaternion.Euler(-CamInputRotation.y, CamInputRotation.x, 0));
            //_camera.transform.localRotation = Quaternion.Euler(-CamInputRotation.y, CamInputRotation.x, 0);
            //_playerPos.rotation = Quaternion.Euler(0, CamInputRotation.x, 0) * transform.rotation;
        }
    }

    [Header("Scope Mech")]
    [Tooltip("Add the MainCamera")]
    public Camera _camera;
    [Tooltip("Add the Canvas")]
    public GameObject _canvas;
    public static bool _isScope = false;
    UI_Inventory _uiInv;

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

    [Header("Weapon Mech")]
    [Tooltip("Add the Text Component for Bullet Count")]
    public TextMeshProUGUI _bulletCountText;
    public static int _bulletCount = 30;
    [Tooltip("Add the Image for Weapon UI")]
    public Image _weapon;
    [Tooltip("Add the Gun Sprite")]
    public Sprite _gun;
    [Tooltip("Add the Hand Sprite")]
    public Sprite _hand;
    [Tooltip("Add the Build Sprite")]
    public Sprite _build;
    [Tooltip("Add the Infinity Sprite")]
    public GameObject _unlimited;
    [Tooltip("Add the BuildEnable Sprite")]
    public GameObject _buildBlock;
    public static bool _unlimitedAmmo = false;
    public static bool _isGun = false;
    public static bool _isBuild = false;

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
                _unlimitedAmmo = false;
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

    [Header("Build Mech")]
    [Tooltip("Add the Scope Image GameObject")]
    public GameObject _scope;
    [Tooltip("Add the Wall GameObject")]
    public GameObject _wall;
    [Tooltip("Add the Floor GameObject")]
    public GameObject _floor;
    [Tooltip("Add the Ramp GameObject")]
    public GameObject _ramp;
    [Tooltip("Add the Cone GameObject")]
    public GameObject _cone;
    GameObject _tempObj;
    GameObject _tempobj;
    Renderer _tempRend;
    MeshCollider _tempCol;
    bool _preview = false;
    [Tooltip("Add the Green Material")]
    public Material _previewMatG;
    [Tooltip("Add the red Material")]
    public Material _previewMatR;
    Material _defaultMat;
    Vector3 _pos;
    public static bool _isCol = false;
    [Tooltip("Add the Grid Space (5 is Recomended)")]
    public int _gridSize;

    void build()
    {   
        if (_isBuild)
        {
            _pos = _player._hitPos;
            // Debug.Log(pos);
            Vector3 snappedPosition = new Vector3(
            Mathf.Round(_pos.x / _gridSize) * _gridSize,
            Mathf.Round(_pos.y / _gridSize) * _gridSize,
            Mathf.Round(_pos.z / _gridSize) * _gridSize
        );
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
            if (Input.GetKeyDown(KeyCode.B) && !_preview)
            {
                _tempobj = _cone;
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
                    _tempObj.layer = 0;
                    _tempCol.isTrigger = false;
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
            if (_tempobj)
            {
                Preview(_tempobj);
                _tempObj.transform.position = snappedPosition;
            }
        }
    }

    void Preview(GameObject Obj)
    {
        if (!_preview)
        {
            _tempObj = Instantiate(Obj, _pos, Quaternion.identity);
            _tempObj.layer = 6;
            _tempRend = _tempObj.transform.GetComponent<Renderer>();
            _tempCol = _tempObj.transform.GetComponent<MeshCollider>();
            _tempCol.isTrigger = true;
            _defaultMat = _tempRend.material;
            _preview = true;
        }
    }

    void Slowmo()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            if(Time.timeScale == 0)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
        }
    } 

    [Header("Abilities Mech")]
    [Tooltip("Add Invisible Image Component")]
    public Image _inv;
    [Tooltip("Add Jump Image Component")]
    public Image _jump;
    [Tooltip("Add Teleporter Image Component")]
    public Image _tel;
    public static bool _isTel = false;
    public static bool _isJump = false;
    public static bool _isInv = false;
    public static bool _telPreview = false;
    MeshRenderer _meshRenderer;
    [Tooltip("Add Teleporter Spawn Position")]
    public Transform _TelTargetPos;
    [Tooltip("Add Teleporter Prefab")]
    public GameObject _Teleporter;
    [Tooltip("Add Satchel Prefab")]
    public GameObject _jumpPad;
    GameObject _JumpPad;
    [Tooltip("Add Satchel Jump Force")]
    public float _jumpPadforce;
    Vector3 _rotationAxis;
    Quaternion _rotation;
    Vector3 _shootDirection;
    Vector3 _spawnPosition;
    [Tooltip("Add Text Component for Inv Count")]
    public TextMeshProUGUI _invCount;
    [Tooltip("Add Text Component for Jump Count")]
    public TextMeshProUGUI _jumpCount;
    [Tooltip("Add Text Component for Telporter Count")]
    public TextMeshProUGUI _telCount;
    int _invCountNum = 50;
    int _jumpCountNum = 50;
    int _telCountNum = 50;

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
            _rotationAxis = _playerPos.forward;
            _rotation = Quaternion.AngleAxis(_playerPos.localRotation.x, _rotationAxis);
            _shootDirection = _rotation * _playerPos.forward;
            _spawnPosition = new Vector3(_playerPos.position.x, _playerPos.position.y + 1f, _playerPos.position.z) + _shootDirection * 1f;
            _JumpPad = Instantiate(_jumpPad, _spawnPosition, Quaternion.LookRotation(_shootDirection));
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

        //Invisiable Logic
        if (Input.GetKeyDown(KeyCode.Z) && !_isInv && _invCountNum >= 1 && !_preview)
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

    [Header("Damage Mech")]
    [Tooltip("Add Volume Component from Global Volume GameObj")]
    public Volume _vol;
    Vignette _vig;

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
