using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static Unity.VisualScripting.Member;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    void Awake()
    {
        instance = this;
        _bullet = Resources.Load<GameObject>("Bullet");
        SpawnBullet();
    }

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
        _CamBotRigi = _camBot.GetComponent<Rigidbody>();
    }

    public static bool _isGame = false;

    void Update()
    {
        //Door();
        CameraRot();
        Crosshair();
        Scope();
        Weapon();
        Damage();
        Abilities();
        build();
        Slowmo();
        MiniMap();
        ZipLine();
        Icon();
        Spawn();
        Text();
        //Rope();
    }

    [Header("Key")]
    [Tooltip("Attach The Text Component Obj")]
    public TextMeshProUGUI _boxText;

    void Text()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    _boxText.text = key.ToString();
                }
            }
        }
    }

    [Header("Spawn")]
    [Tooltip("Attach The Player Spawn Transform")]
    public Transform _spawnPos;
    bool _spawned = false;

    void Spawn()
    {
        if (_isGame && _rb3D.isKinematic && !_spawned)
        {
            _playerPos.position = _spawnPos.position;
            _rb3D.isKinematic = false;
            _spawned = true;
        }
    }

    [Header("Icons")]
    [Tooltip("Attach The Player Icon GameObject")]
    public Transform _playerIcon;
    public Transform _camBotIcon;
    public Transform _TeleporterIcon;

    void Icon()
    {
        _playerIcon.position = _playerPos.position;
        _playerIcon.rotation = Quaternion.Euler(_playerIcon.rotation.eulerAngles.x, _playerPos.rotation.eulerAngles.y, _playerIcon.rotation.eulerAngles.z);

        if (_camBot.activeInHierarchy)
        {
            _camBotIcon.gameObject.SetActive(true);
            _camBotIcon.position = _camBot.transform.position;
            _camBotIcon.rotation = Quaternion.Euler(_playerIcon.rotation.eulerAngles.x, _camBot.transform.rotation.eulerAngles.y, _playerIcon.rotation.eulerAngles.z);
            _camBotIcon.position = ClampToMinimap(_camBotIcon.position);
        }
        else
        {
            _camBotIcon.gameObject.SetActive(false);
        }

        if (_Teleporter.activeInHierarchy)
        {
            _TeleporterIcon.gameObject.SetActive(true);
            _TeleporterIcon.position = _Teleporter.transform.position;
            _TeleporterIcon.rotation = Quaternion.Euler(_TeleporterIcon.rotation.eulerAngles.x, _Teleporter.transform.rotation.eulerAngles.y, _TeleporterIcon.rotation.eulerAngles.z);
            _TeleporterIcon.position = ClampToMinimap(_TeleporterIcon.position);
        }
        else
        {
            _TeleporterIcon.gameObject.SetActive(false);
        }
    }

    [Header("Distance Clamp")]
    [Tooltip("Enter the Clamp distance")]
    public float MinimapSize;

    Vector3 ClampToMinimap(Vector3 position)
    {
        return new Vector3(
            Mathf.Clamp(position.x, _camMiniMap.transform.position.x - MinimapSize, _camMiniMap.transform.position.x + MinimapSize),
            position.y,
            Mathf.Clamp(position.z, _camMiniMap.transform.position.z - MinimapSize, _camMiniMap.transform.position.z + MinimapSize)
        );
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

    GameObject _bullet;
    Queue<GameObject> _bulletPool = new Queue<GameObject>();
    Transform _bulletSpawner;

    void SpawnBullet()
    {
        _bulletSpawner = transform.Find("BulletSpawner");
        for (int i = 0; i < 30; i++)
        {
            GameObject pObj = Instantiate(_bullet, _bulletSpawner);
            pObj.name = $"Bullet {i}";
            pObj.SetActive(false);
            _bulletPool.Enqueue(pObj);
        }
    }
    public GameObject GetBullet()
    {
        GameObject bullet = _bulletPool.Dequeue();
        Debug.Log(bullet.name);
        bullet.SetActive(true);
        _bulletPool.Enqueue(bullet);
        return bullet;
    }

    [Header("MiniMap")]
    [Tooltip("Attack the Mini_Map Camera")]
    public Camera _camMiniMap;
    [SerializeField] Vector2 _clampArea;
    [SerializeField] bool _clamp = false;

    void MiniMap()
    {
        if (_isCam)
        {
            if( _clamp)
            {
                _camMiniMap.transform.position = new Vector3(Mathf.Clamp(_camBot.transform.position.x, _clampArea.x, -_clampArea.x), _camBot.transform.position.y + 20f, Mathf.Clamp(_camBot.transform.position.z, -_clampArea.y, _clampArea.y));
                _camMiniMap.transform.rotation = Quaternion.Euler(_camMiniMap.transform.rotation.eulerAngles.x, 0, _camMiniMap.transform.rotation.eulerAngles.z);
            }
            else
            {
                _camMiniMap.transform.position = new Vector3(_camBot.transform.position.x, _camBot.transform.position.y + 20f, _camBot.transform.position.z);
                _camMiniMap.transform.rotation = Quaternion.Euler(_camMiniMap.transform.rotation.eulerAngles.x, _camBot.transform.rotation.eulerAngles.y, _camMiniMap.transform.rotation.eulerAngles.z);
            }
        }
        else
        {
            if (_clamp)
            {
                _camMiniMap.transform.position = new Vector3(Mathf.Clamp(_playerPos.transform.position.x, _clampArea.x, -_clampArea.x), _playerPos.transform.position.y + 20f, Mathf.Clamp(_playerPos.transform.position.z, -_clampArea.y, _clampArea.y));
                _camMiniMap.transform.rotation = Quaternion.Euler(_camMiniMap.transform.rotation.eulerAngles.x, 0, _camMiniMap.transform.rotation.eulerAngles.z);
            }
            else
            {
                _camMiniMap.transform.position = new Vector3(_playerPos.transform.position.x, _playerPos.transform.position.y + 20f, _playerPos.transform.position.z);
                _camMiniMap.transform.rotation = Quaternion.Euler(_camMiniMap.transform.rotation.eulerAngles.x, _playerPos.transform.rotation.eulerAngles.y, _camMiniMap.transform.rotation.eulerAngles.z);
            }   
        }
    }

    //[Header("Door")]
    //[Tooltip("The door which need key")]
    //public Transform _objDoor;
    //public static bool _hasKey = false;
    //public static bool _nearDoor = false;
    //bool _doorOpened = false;
    //Vector3 _targetPosition;
    //bool _pressedE = false;

    //void Door()
    //{
    //    if (_hasKey && !_doorOpened && Input.GetKeyDown(KeyCode.E))
    //    {
    //        _pressedE = true;
    //    }
    //    if(_pressedE)
    //    {
    //        _objDoor.position = Vector3.Lerp(_objDoor.position, _targetPosition, 2 * Time.deltaTime);
    //    }
    //    if (_objDoor.position.y >= 4.4f && !_doorOpened)
    //    {
    //        _pressedE = false;
    //        _doorOpened = true;
    //        _hasKey = false;
    //    }
    //}

    public static Transform _zipLineStart;
    public static Transform _zipLineEnd;
    public static Transform _zipLine;
    public static bool _isZip = false;
    public static bool _zipTrig = false;
    float _moveDuration = 2f;
    float _elapsedTime = 0f;

    void ZipLine()
    {
        if (_zipTrig && Input.GetKeyDown(KeyCode.CapsLock))
        {
            _isZip = true;
            _playerPos.SetParent(_zipLine);
            _playerPos.position = new Vector3(_zipLine.position.x, _zipLine.position.y - 1.5f, _zipLine.position.z);
            _rb3D.isKinematic = true;
        }
        if (_elapsedTime < _moveDuration && _isZip)
        {
            _elapsedTime += Time.deltaTime;
            float t = _elapsedTime / _moveDuration;
            Vector3 position = Vector3.Lerp(_zipLineStart.position, _zipLineEnd.position, t);
            position.y += -1 * 3 * t * (1 - t);
            _zipLine.position = position;
        }
        if(_elapsedTime >= _moveDuration && Input.GetKeyDown(KeyCode.CapsLock))
        {
            _elapsedTime = 0f;
            _playerPos.SetParent(null);
            _playerPos.position += _zipLineEnd.right * 2f;
            _rb3D.isKinematic = false;
            _isZip = false;
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
        if ((Input.GetKeyDown(KeyCode.BackQuote) || Input.GetKeyDown(KeyCode.Escape)) && _isGame)
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
            if (!_isZip)
            {
                CamInputRotation.y += Input.GetAxis("Mouse Y");
                CamInputRotation.y = Mathf.Clamp(CamInputRotation.y, -60, 60f);
            }
            else
            {
                CamInputRotation.y = 0;
            }
            if (!_isCam)
            {
                if (Time.timeScale == 0 || _isZip)
                {
                    _playerPos.transform.rotation = Quaternion.Euler(-CamInputRotation.y, CamInputRotation.x, 0);
                }
                if (Time.timeScale == 1 || !_isZip)
                {
                    _rb3D.MoveRotation(Quaternion.Euler(-CamInputRotation.y, CamInputRotation.x, 0));
                }
            }
            else
            {
                _CamBotRigi.MoveRotation(Quaternion.Euler(0, CamInputRotation.x, 0));
            }
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
            if ((Input.GetKeyDown(KeyCode.Alpha1) || _isTel) && _weapon.sprite != _hand)
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

    [Header("SlowMo")]
    [Tooltip("Add Time Image Component")]
    public Image _time;

    void Slowmo()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            _time.enabled = _time.enabled == true ? false : true;
        }
        if (_time.enabled)
        {
            if (Player._isMoving)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
        }
        else
        {
            Time.timeScale = 1;
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
    public static bool _isCam = false;
    MeshRenderer _meshRenderer;
    [Tooltip("Add Teleporter Spawn Position")]
    public Transform _TargetPos;
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
    [Tooltip("Add CamBot Prefab")]
    public GameObject _camBot;
    [Tooltip("Add Text Component for Inv Count")]
    public TextMeshProUGUI _invCount;
    [Tooltip("Add Text Component for Jump Count")]
    public TextMeshProUGUI _jumpCount;
    [Tooltip("Add Text Component for Telporter Count")]
    public TextMeshProUGUI _telCount;
    public GameObject _playerUI;
    public GameObject _cambotUI;
    public TextMeshProUGUI _camTimer;
    int _invCountNum = 50;
    int _jumpCountNum = 50;
    int _telCountNum = 50;
    Rigidbody _CamBotRigi;
    bool _iscam = false;
    float _remainingTime = 15;

    void Abilities()
    {
        if (_remainingTime > 0)
        {
            _remainingTime -= Time.deltaTime;
        }
        else if (_remainingTime < 0)
        {
            _remainingTime = 0;
        }

        int minutes = Mathf.FloorToInt(_remainingTime / 60);
        int seconds = Mathf.FloorToInt(_remainingTime % 60);
        _camTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);

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
        if (Input.GetKeyDown(KeyCode.Q) && !_isJump && _jumpCountNum >= 1 && !_preview && !_isCam)
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
        if (Input.GetKeyDown(KeyCode.E) && !_Teleporter.activeInHierarchy && _telCountNum >= 1 && !_preview && !_isScope && !_isCam)
        {
            _Teleporter.transform.SetParent(_playerPos);
            _Teleporter.transform.localRotation = Quaternion.identity;
            _tel.color = new Color32(255, 255, 255, 255);
            _isTel = true;
            _Teleporter.SetActive(true);
            _Teleporter.transform.position = _TargetPos.position;
        }
        if (_Teleporter.activeInHierarchy)
        {
            Rigidbody _TeleporterRigi = _Teleporter.GetComponent<Rigidbody>();
            _Teleporter.transform.rotation = Quaternion.Euler(0, _Teleporter.transform.eulerAngles.y, 0);
            if (Input.GetMouseButtonDown(0) && _isTel)
            {
                _TeleporterRigi.useGravity = true;
                _TeleporterRigi.isKinematic = false;
                _tel.color = new Color32(45, 45, 45, 125);
                _Teleporter.transform.SetParent(null);
                _telCountNum--;
                _isTel = false;
            }
            if (!_isTel)
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
        if (Input.GetKeyDown(KeyCode.Z) && !_isInv && _invCountNum >= 1 && !_preview && !_isCam)
        {
            _inv.color = new Color32(255, 255, 255, 255);
            _meshRenderer =  _playerPos.gameObject.GetComponent<MeshRenderer>();
            _meshRenderer.enabled = false;
            StartCoroutine(Inv());
        }

        //CamBot Logic
        if(Input.GetKeyDown(KeyCode.G) && !_camBot.activeInHierarchy && !_isCam && !_isScope && !_isTel && !_preview)
        {
            _camBot.transform.rotation = Quaternion.Euler(0, _camBot.transform.eulerAngles.y, 0);
            _camBot.SetActive(true);
            _camera.enabled = false;
            _camBot.transform.position = _TargetPos.position;
            _CamBotRigi.useGravity = true;
            _CamBotRigi.isKinematic = false;
            _isCam = true;
            _iscam = true;
        }
        if (_camBot.activeInHierarchy)
        {
            _CamBotRigi.linearVelocity = _camBot.transform.forward * 10;
            if (_iscam)
            {
                StartCoroutine(Cam(_CamBotRigi));
            }
        }
    }

    IEnumerator Cam(Rigidbody _CamBotRigi)
    {
        _remainingTime = 15;
        _iscam = false;
        _playerUI.SetActive(false);
        _cambotUI.SetActive(true);
        yield return new WaitForSeconds(15);
        _playerUI.SetActive(true);
        _cambotUI.SetActive(false);
        _CamBotRigi.useGravity = false;
        _CamBotRigi.isKinematic = true;
        _camBot.SetActive(false);
        _camera.enabled = true;
        _isCam = false;
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
