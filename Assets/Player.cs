using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Vector3 rawInputMovement;
    public Vector2 CamInputRotation;
    public float _Hor;
    public float _Ver;
    public float _moveSpeed;
    public float _jumpForce;
    public Rigidbody _rb3D;
    public LayerMask _layerMask;
    public Inventory _inv;
    public UI_Inventory _uiInv;
    public GameManager _gm;
    public GameObject _item;
    public Vector3 _hitPos;
    public int Count = 0;
    public float _dis;
    public bool _obj = false;
    public bool _isShield = false;
    public bool _canShoot = true;
    public static int _healthPower = 10;
    public static int _shieldPower = 10;
    public Material _shieldMat;
    public Material _damageMat;
    public Material _defaultMat;
    public Renderer _matPlayer;
    public Transform _placementPreview;
    public Transform _camPos;
    public GameObject _bullet;
    public GameObject _Bullet;
    public static int _tempBulletCount;
    public LayerMask _ignoreLayer;
    public SpringJoint _springJoint;
    void Start()
    {
        _springJoint = GetComponent<SpringJoint>();
        _tempBulletCount = GameManager._bulletCount;
        _camPos = transform.Find("Main Camera").transform;
        _matPlayer = GetComponent<Renderer>();
        _defaultMat = _matPlayer.material;
        Cursor.lockState = CursorLockMode.None;
        //_uiInv = GetComponent<UI_Inventory>();
        //FOV = _camera.fieldOfView;
        _rb3D = GetComponent<Rigidbody>();
    }

    void Update()
    {
        PlayerMovementKB();
        RayCast();
        Crosshair();
        Shield();
        Shoot();
        //Spring();
    }
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
    void Crosshair()
    {
        Vector3 baseDirection = Vector3.down;
        Vector3 CamPos = _camPos.position;
        Vector3 rotationAxis = _camPos.right * -1;
        Quaternion rotation = Quaternion.AngleAxis(_camPos.localRotation.x, rotationAxis);
        Vector3 angledDirection = rotation * transform.forward;
        if (Physics.Raycast(CamPos, angledDirection, out RaycastHit hitInfo, _dis))
        {
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
                else if (hitInfo.collider.gameObject.CompareTag("RopePoint"))
                {
                    GameManager._isRope = true;
                    if (!_gm._lineRen.enabled)
                    {
                        _gm._ropePoint = hitInfo.collider.gameObject;
                    }
                    if (_gm._tempRopeRen == null)
                    {
                        _gm._tempRopeRen = _gm._ropePoint.GetComponent<Renderer>();
                        _gm._defaultRopeMat = _gm._tempRopeRen.material;
                    }
                    GameManager._blackB = true;
                    GameManager._redB = false;
                    GameManager._greenB = false;
                }
                else if (!hitInfo.collider.gameObject.CompareTag("RopePoint"))
                {
                    if(_gm._tempRopeRen != null)
                    {
                        _gm._tempRopeRen.material = _gm._defaultRopeMat;
                        _gm._tempRopeRen = null;
                    }
                    GameManager._isRope = false;
                }
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
            GameManager._isRope = false;
            if (_gm._tempRopeRen != null)
            {
                _gm._tempRopeRen.material = _gm._defaultRopeMat;
                _gm._tempRopeRen = null;
            }
            _obj = false;
            GameManager._blackB = false;
            GameManager._redB = false;
            GameManager._greenB = false;
            Debug.DrawRay(CamPos, angledDirection * _dis, Color.red);
        }
    }
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
    void Spring()
    {
        _springJoint.anchor = transform.position;
        if(_gm._ropePoint != null && GameManager._isRope)
        {
            _springJoint.connectedAnchor = _gm._ropePoint.transform.position;
            _springJoint.autoConfigureConnectedAnchor = false;
            _springJoint.connectedAnchor = _gm._ropePoint.transform.position;

            float distanceFromPoint = Vector3.Distance(transform.position, transform.position);

            _springJoint.maxDistance = distanceFromPoint * 0.8f;
            _springJoint.minDistance = distanceFromPoint * 0.25f;

            _springJoint.spring = 4.5f;
            _springJoint.damper = 7f;
            _springJoint.massScale = 4.5f;
        }
            
        else _springJoint.connectedAnchor = transform.position;
    }
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
        if (Input.GetKeyDown(KeyCode.Space) && GroundCheck())
        {
            //_camera.fieldOfView += 10;
            //StartCoroutine(Cam(_camera));
            _rb3D.linearVelocity = new Vector3(_rb3D.linearVelocity.x, _jumpForce, _rb3D.linearVelocity.z);
        }
    }

    bool GroundCheck()
    {
        return Physics.BoxCast(transform.position, new Vector3(1, .5f, 1), Vector3.down, Quaternion.identity, 1f, _layerMask);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = GroundCheck() ? Color.green : Color.red;
        Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y + -1f, transform.position.z), new Vector3(1, .25f, 1));
    }

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
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Jumppad"))
        {
            _rb3D.AddForce(transform.up * _gm._jumpPadforce * _moveSpeed, ForceMode.Impulse);
            Destroy(other.gameObject, .25f);
        }
    }
}
