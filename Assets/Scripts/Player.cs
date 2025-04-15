using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.XR;

public class Player : MonoBehaviour
{
    UI_Inventory _uiInv;
    GameManager _gm;
    
    void Start()
    {
        //_springJoint = GetComponent<SpringJoint>();
        _bullet = Resources.Load<GameObject>("Bullet");
        _tempBulletCount = GameManager._bulletCount;
        _camPos = transform.Find("Main Camera").transform;
        _matPlayer = GetComponent<Renderer>();
        _defaultMat = _matPlayer.material;
        Cursor.lockState = CursorLockMode.None;
        _uiInv = FindFirstObjectByType<UI_Inventory>();
        _gm = FindFirstObjectByType<GameManager>();
        _rb3D = GetComponent<Rigidbody>();
    }

    void Update()
    {
        PlayerMovementKB();
        RayCast();
        Crosshair();
        Shield();
        Shoot();
        PickNDrop();
        SpecialGroundCheck();
        //Spring();
    }
    void FixedUpdate()
    {
        //_rb3D.AddForce(new Vector3(-9.81f, 11f, 0f), ForceMode.Acceleration);
    }

    [Header("Shield")]
    [Tooltip("Add Shield Material")]
    [SerializeField] Material _shieldMat;
    public static int _shieldPower = 10;
    bool _isShield = false;
    Material _defaultMat;
    Renderer _matPlayer;

    void Shield()
    {
        if (!_isShield && Input.GetKeyDown(KeyCode.LeftAlt) && _shieldPower > 0)
        {
            _matPlayer.material = _shieldMat;
            _isShield = true;
            StartCoroutine(ShieldTimer());
        }
    }

    IEnumerator ShieldTimer()
    {
        yield return new WaitForSeconds(5);
        _matPlayer.material = _defaultMat;
        _shieldPower--;
        _isShield = false;
    }

    [Header("Damage")]
    [Tooltip("Add Damage hit Material")]
    [SerializeField] Material _damageMat;
    public static int _healthPower = 10;

    IEnumerator Damage()
    {
        _healthPower -= 1;
        _matPlayer.material = _damageMat;
        yield return new WaitForSeconds(.05f);
        _matPlayer.material = _defaultMat;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            if (!_isShield)
            {
                StartCoroutine(Damage());
            }
            Destroy(collision.gameObject);
        }
    }

    [Header("Raycast")]
    [Tooltip("Enter the Ray Length")]
    [SerializeField] float _dis;
    [Tooltip("Select the Ignore Layer Mask")]
    [SerializeField] LayerMask _ignoreLayer;
    Transform _camPos;
    [HideInInspector] public bool _obj = false;
    GameObject _item;
    
    void Crosshair()
    {
        Vector3 baseDirection = Vector3.down;
        Vector3 CamPos = _camPos.position;
        Vector3 rotationAxis = _camPos.right * -1;
        Quaternion rotation = Quaternion.AngleAxis(_camPos.localRotation.x, rotationAxis);
        Vector3 angledDirection = rotation * transform.forward;
        if (Physics.Raycast(CamPos, angledDirection, out RaycastHit hitInfo, _dis, ~_ignoreLayer))
        {
            //Debug.Log(hitInfo.collider.gameObject.name);
            if (GameManager._isGun)
            {
                GameManager._blackB = false;
                GameManager._redB = false;
                GameManager._greenB = false;
            }
            else
            {
                if (hitInfo.collider.gameObject.CompareTag("Item"))
                {
                    UI_Item._isItem = true;
                    _item = hitInfo.collider.gameObject;
                    
                    if (Input.GetKeyDown(KeyCode.F))
                    {
                        Inventory._invInstance.AddItem(_item);
                        Destroy(_item);
                    }
                    
                    _obj = true;
                    GameManager._blackB = true;
                    GameManager._redB = false;
                    GameManager._greenB = false;
                }
                //else if (hitInfo.collider.gameObject.CompareTag("RopePoint"))
                //{
                //    GameManager._isRope = true;
                //    if (!_gm._lineRen.enabled)
                //    {
                //        _gm._ropePoint = hitInfo.collider.gameObject;
                //    }
                //    if (_gm._tempRopeRen == null)
                //    {
                //        _gm._tempRopeRen = _gm._ropePoint.GetComponent<Renderer>();
                //        _gm._defaultRopeMat = _gm._tempRopeRen.material;
                //    }
                //    GameManager._blackB = true;
                //    GameManager._redB = false;
                //    GameManager._greenB = false;
                //}
                //else if (!hitInfo.collider.gameObject.CompareTag("RopePoint"))
                //{
                //    if(_gm._tempRopeRen != null)
                //    {
                //        _gm._tempRopeRen.material = _gm._defaultRopeMat;
                //        _gm._tempRopeRen = null;
                //    }
                //    GameManager._isRope = false;
                //}
                else if (hitInfo.collider.gameObject.CompareTag("Door"))
                {
                    UI_Item._isItem = true;
                    GameManager._nearDoor = true;
                    if (!GameManager._hasKey)
                    {
                        GameManager._blackB = false;
                        GameManager._redB = true;
                        GameManager._greenB = false;
                    }
                    if (GameManager._hasKey)
                    {
                        GameManager._blackB = false;
                        GameManager._redB = false;
                        GameManager._greenB = true;
                    }
                }
                else if (hitInfo.collider.gameObject.CompareTag("Enemy"))
                {
                    UI_Item._isItem = true;
                    GameManager._blackB = false;
                    GameManager._redB = true;
                    GameManager._greenB = false;
                }
                else if (hitInfo.collider.gameObject.CompareTag("Ground"))
                {
                    _obj = false;
                    UI_Item._isItem = false;
                    GameManager._blackB = false;
                    GameManager._redB = false;
                    GameManager._greenB = false;
                }
                else
                {
                    UI_Item._isItem = false;
                    GameManager._blackB = false;
                    GameManager._redB = false;
                    GameManager._greenB = false;
                    GameManager._nearDoor = false;
                }
                Debug.DrawRay(CamPos, angledDirection * hitInfo.distance, Color.green);
            }
        }
        else
        {
            //GameManager._isRope = false;
            //if (_gm._tempRopeRen != null)
            //{
            //    _gm._tempRopeRen.material = _gm._defaultRopeMat;
            //    _gm._tempRopeRen = null;
            //}
            _obj = false;
            GameManager._blackB = false;
            GameManager._redB = false;
            GameManager._greenB = false;
            Debug.DrawRay(CamPos, angledDirection * _dis, Color.red);
        }
    }

    [Header("Pick & Drop (Gravity Gun)")]
    [Tooltip("Attach the Transform for Obj PlaceHolder")]
    [SerializeField] Transform _objectHolder;
    Rigidbody _grabbedRB;
    float _force = 10;

    void PickNDrop()
    {
        if (_grabbedRB)
        {
            _grabbedRB.MovePosition(Vector3.Lerp(_grabbedRB.position, _objectHolder.position, Time.deltaTime * 100));
            if (Input.GetMouseButton(1))
            {
                _force += .01f;
            }
            if (Input.GetMouseButtonUp(1))
            {
                _grabbedRB.isKinematic = false;
                _grabbedRB.AddForce(_objectHolder.forward * (int)_force, ForceMode.VelocityChange);
                _grabbedRB = null;
                _force = 10;
            }
        }
        if (Input.GetMouseButtonDown(0) && _obj && _uiInv._aniIndex != 2)
        {
            if (!_grabbedRB)
            {
                _grabbedRB = _item.GetComponent<Rigidbody>();
                if (_grabbedRB)
                {
                    _grabbedRB.isKinematic = true;
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            _force = 10;
            if (_grabbedRB)
            {
                _grabbedRB.isKinematic = false;
                _grabbedRB.linearVelocity = Vector3.zero;
                _grabbedRB.angularVelocity = Vector3.zero;
                _grabbedRB = null;
            }
        }
    }

    [HideInInspector] public Vector3 _hitPos;

    public bool RayCast()
    {
        Vector3 baseDirection = Vector3.down;
        Vector3 CamPos = _camPos.position;
        Vector3 rotationAxis = _camPos.right * -1;
        Quaternion rotation = Quaternion.AngleAxis(_camPos.localRotation.x, rotationAxis);
        Vector3 angledDirection = rotation * transform.forward;
        if (Physics.Raycast(CamPos, angledDirection, out RaycastHit hitInfo, _dis, ~_ignoreLayer))
        {
            _hitPos = hitInfo.point;
            if (hitInfo.collider.gameObject.layer == Mathf.Log(_layerMask.value, 2))
            {
                UI_Item._isItem = false;
            }
            Debug.DrawRay(CamPos, angledDirection * hitInfo.distance, Color.green);
            return true;
        }
        else
        {
            Debug.DrawRay(CamPos, angledDirection * _dis, Color.red);
            return false;
        }
    }

    bool _canShoot = true;
    GameObject _bullet;
    GameObject _Bullet;
    public static int _tempBulletCount;

    void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.R) || GameManager._unlimitedAmmo)
        {
            _tempBulletCount = GameManager._bulletCount;
        }
        if(_uiInv._aniIndex != 2 && Input.GetMouseButton(0) && _canShoot && _tempBulletCount !=0 && GameManager._isGun && !GameManager._telPreview)
        {
            StartCoroutine(DelayedShoot());
        }
    }

    IEnumerator DelayedShoot()
    {
        _canShoot = false;
        yield return new WaitForSeconds(.05f);
        Vector3 rotationAxis = _camPos.right * -1;
        Quaternion rotation = Quaternion.AngleAxis(_camPos.localRotation.x, rotationAxis);
        Vector3 shootDirection = rotation * transform.forward;
        Vector3 spawnPosition = _camPos.position + shootDirection * 1f;
        _Bullet = Instantiate(_bullet, spawnPosition, Quaternion.LookRotation(shootDirection));
        if (!GameManager._unlimitedAmmo)
        {
            _tempBulletCount--;
        }
        Rigidbody _bulletRigi = _Bullet.GetComponent<Rigidbody>();
        _bulletRigi.AddForce(shootDirection * 5000f, ForceMode.Force);
        Destroy(_Bullet, 3f);
        yield return new WaitForSeconds(.05f);
        _canShoot = true;
    }

    //public SpringJoint _springJoint;
    //void Spring()
    //{
    //    _springJoint.anchor = transform.position;
    //    if(_gm._ropePoint != null && GameManager._isRope)
    //    {
    //        _springJoint.connectedAnchor = _gm._ropePoint.transform.position;
    //        _springJoint.autoConfigureConnectedAnchor = false;
    //        _springJoint.connectedAnchor = _gm._ropePoint.transform.position;

    //        float distanceFromPoint = Vector3.Distance(transform.position, transform.position);

    //        _springJoint.maxDistance = distanceFromPoint * 0.8f;
    //        _springJoint.minDistance = distanceFromPoint * 0.25f;

    //        _springJoint.spring = 4.5f;
    //        _springJoint.damper = 7f;
    //        _springJoint.massScale = 4.5f;
    //    }

    //    else _springJoint.connectedAnchor = transform.position;
    //}

    [Header("Player Movement")]
    [Tooltip("Enter the Player Move Speed")]
    [SerializeField] float _moveSpeed;
    [Tooltip("Enter the Player Jump Speed")]
    [SerializeField] float _jumpForce;
    float _Hor;
    float _Ver;
    Rigidbody _rb3D;

    void PlayerMovementKB()
    {
        _Hor = Input.GetAxisRaw("Horizontal");
        _Ver = Input.GetAxisRaw("Vertical");
        if (_Hor != 0 || _Ver != 0)
        {
            Vector3 moveDirection = transform.right * _Hor + transform.forward * _Ver;
            moveDirection.Normalize();
            _rb3D.linearVelocity = new Vector3(moveDirection.x * _moveSpeed, _rb3D.linearVelocity.y, moveDirection.z * _moveSpeed);
        }
        else
        {
            _rb3D.linearVelocity = new Vector3(0, _rb3D.linearVelocity.y, 0);
        }
        if ((Input.GetKeyDown(KeyCode.LeftShift)))
        {
            _moveSpeed *= 1.5f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _moveSpeed /= 1.5f;
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Vector3 dashDirection = transform.right * _Hor + transform.forward * _Ver;
            dashDirection.y = 0;
            dashDirection.Normalize();
            _rb3D.AddForce(dashDirection * 400f, ForceMode.VelocityChange);
        }
        if (Input.GetKeyDown(KeyCode.Space) && GroundCheck())
        {
            _rb3D.linearVelocity = new Vector3(_rb3D.linearVelocity.x, _jumpForce, _rb3D.linearVelocity.z);
        }
        else
        {
            _rb3D.linearVelocity = new Vector3(_rb3D.linearVelocity.x, _rb3D.linearVelocity.y, _rb3D.linearVelocity.z);
        }
    }

    [Header("Ground Dectection")]
    [Tooltip("Select the Ground Layer Mask")]
    [SerializeField] LayerMask _layerMask;

    bool GroundCheck()
    {
        return Physics.BoxCast(transform.position, new Vector3(1, .5f, 1), Vector3.down, Quaternion.identity, 1f, _layerMask);
    }

    //bool isOnSpecialGround = false;
    [Tooltip("Select the Special Ground Layer Mask")]
    [SerializeField] LayerMask _specialLayerMask;

    void SpecialGroundCheck()
    {
        if(Physics.Raycast(transform.position, -transform.up, out RaycastHit hitInfo, _dis, _specialLayerMask))
        {
            Debug.DrawRay(transform.position, -transform.up * hitInfo.distance, Color.green);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = GroundCheck() ? Color.green : Color.red;
        Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y + -1f, transform.position.z), new Vector3(1, .25f, 1));
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Jumppad"))
        {
            //_rb3D.AddForce(transform.up * _gm._jumpPadforce * _moveSpeed, ForceMode.Impulse);
            Destroy(other.gameObject, .25f);
        }
    }
}
