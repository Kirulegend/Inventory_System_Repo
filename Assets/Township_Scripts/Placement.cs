using System.Collections;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using System.Collections.Generic;

public class Placement : MonoBehaviour
{
    Vector3 _hitPos;
    Vector3 _current;
    Vector3 _past;
    int _gridSize = 2;
    [Header("LayerMasks")]
    [Tooltip("Select the Ground Layer Mask")]
    public LayerMask _groundLayer;
    [Tooltip("Select the Road Layer Mask")]
    public LayerMask _roadMask;
    bool _isGrounded = false;

    GameObject _road;
    Vector3 BsnappedPosition;

    Animator Ani;

    GameData _gameData;
    Material _editMaterial;
    Material _editMaterialR;
    Material _defaultMaterial;
    void Awake()
    {
        _editMaterial = Resources.Load<Material>("Holo");
        _editMaterialR = Resources.Load<Material>("Holo Red");
        _road = Resources.Load<GameObject>("Placement");
        _gameData = GameObject.Find("GameData")?.GetComponent<GameData>();
        _buildParent = GameObject.Find("Builds")?.GetComponent<Transform>();
        _roadParent = GameObject.Find("Roads")?.GetComponent<Transform>();
    }
    void Update()
    {
        MouseCast();
        BuildCheck();
    }

    GameObject _tempRoad;
    GameObject _editBuildObj;
    bool _roadPlace = false;
    [HideInInspector] public bool _buildCheck = false;
    public static bool _placed = false;
    [HideInInspector]public bool _editBuild = false;
    float Timer = 0;

    void MouseCast()
    {
        //Ray for Pos Check only for ground
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _groundLayer))
        {
            _hitPos = hit.point;
        }
        //hit check for any obj
        if (Physics.Raycast(ray, out RaycastHit Hit))
        {
            if (((1 << Hit.collider.gameObject.layer) & _groundLayer) != 0)
            {
                _isGrounded = true;
            }
            else
            {
                _isGrounded = false;
            }
            Vector3 _buildPos = Hit.point;
            if (Hit.collider.gameObject.CompareTag("Build"))
            {
                if (Input.GetMouseButton(0) && Timer < .5f && !_editBuild && !_isRoad)
                {
                    Timer += Time.deltaTime;
                }
                if (Input.GetMouseButtonUp(0) && Timer <= .5f)
                {
                    Timer = 0;
                }
                if(Timer >= .5f)
                {
                    _editBuildObj = Hit.collider.gameObject;
                    _placed = false;
                    _editBuild = true;
                    Timer = 0;
                }
            }
            else
            {
                Timer = 0;
            }
        }
        else
        {
            _isGrounded = false;
        }
        if (_editBuild)
        {
            if (!_defaultMaterial)
            {
                _defaultMaterial = _editBuildObj.GetComponent<MeshRenderer>().material;
                _editBuildObj.GetComponent<MeshRenderer>().material = _editMaterial;
            }
            if (BuildObj._isGround) _editBuildObj.GetComponent<MeshRenderer>().material = _editMaterial;
            else _editBuildObj.GetComponent<MeshRenderer>().material = _editMaterialR;
            _buildCheck = true;
            _buildCheck = true;
            _editBuildObj.transform.position = new Vector3(BsnappedPosition.x, BsnappedPosition.y + 1, BsnappedPosition.z);
            Ani = _editBuildObj.GetComponent<Animator>();
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (_editBuildObj.transform.rotation.y == 0)
                    _editBuildObj.transform.Rotate(0, 90, 0);
                else _editBuildObj.transform.Rotate(0, -90, 0);
            }
            if (Input.GetMouseButtonDown(0) && BuildObj._isGround)
            {
                if (_defaultMaterial)
                {
                    _editBuildObj.GetComponent<MeshRenderer>().material = _defaultMaterial;
                    _defaultMaterial = null;
                }
                _editBuildObj.transform.position = BsnappedPosition;
                Ani.SetTrigger("Place");
                _placed = true;
                _editBuildObj = null;
                _editBuild = false;
                _buildCheck = false;
            }
        }
        _current = BsnappedPosition;
        if (Input.GetMouseButtonDown(0) && _isRoad && !EventSystem.current.IsPointerOverGameObject())
        {
            _roadPlace = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _roadPlace = false;
        }
        if (_isGrounded && _past != _current && !_isBuild && _roadPlace)
        {
            _gameData._roadCount++;
            _tempRoad = Instantiate(_road, _roadParent);
            _tempRoad.transform.position = _current;
            _past = _current;
        }
        if (Input.GetMouseButton(1) && _isRoad)
        {
            if (((1 << Hit.collider.gameObject.layer) & _roadMask) != 0)
            {
                _gameData._roadCount--;
                _past = _current;
                Destroy(Hit.collider.gameObject);
            }
        }
    }

    [HideInInspector] public GameObject _activeCube = null;
    public static bool _isBuild = false;
    Transform _buildParent;
    Transform _roadParent;

    public void Build(string Build)
    {
        if (!_activeCube) _activeCube = Instantiate(Resources.Load<GameObject>(Build), _hitPos, Quaternion.identity, _buildParent);
        if (!_defaultMaterial)
        {
            _defaultMaterial = _activeCube.GetComponent<MeshRenderer>().material;
            _activeCube.GetComponent<MeshRenderer>().material = _editMaterial;
        }
        _placed = false;
        StartCoroutine(BuildTimer());
        Ani = _activeCube.GetComponent<Animator>();
    }
    void BuildCheck()
    {
        BsnappedPosition = new Vector3(
        Mathf.Round(_hitPos.x / _gridSize) * _gridSize,
        _hitPos.y,
        Mathf.Round(_hitPos.z / _gridSize) * _gridSize
        );
        if (_activeCube)
        {
            if (BuildObj._isGround) _activeCube.GetComponent<MeshRenderer>().material = _editMaterial;
            else _activeCube.GetComponent<MeshRenderer>().material = _editMaterialR;
            _buildCheck = true;
            _activeCube.transform.position = new Vector3(BsnappedPosition.x, BsnappedPosition.y + 1, BsnappedPosition.z);
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (_activeCube.transform.rotation.y == 0)
                _activeCube.transform.Rotate(0, 90, 0);
                else _activeCube.transform.Rotate(0, -90, 0);
            }
            if (Input.GetMouseButtonDown(0) && BuildObj._isGround)
            {
                if (_defaultMaterial)
                {
                    _activeCube.GetComponent<MeshRenderer>().material = _defaultMaterial;
                    _defaultMaterial = null;
                }
                _gameData._buildCount++;
                _activeCube.transform.position = BsnappedPosition;
                Ani.SetTrigger("Place");
                _activeCube = null;
                _placed = true;
                _isBuild = false;
                _buildCheck = false;
            }
        }
    }
    IEnumerator BuildTimer()
    {
        yield return new WaitForSeconds(.25f);
        _isBuild = true;
    }

    public static bool _isRoad = false;

    public void Road()
    {
        _isRoad = _isRoad ? false : true;
    }
}
