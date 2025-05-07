using System.Collections;
using System.Diagnostics.Contracts;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

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

    [Header("Road Parent")]
    [Tooltip("Insert the Road Placement Prefabs")]
    public GameObject _road;
    Vector3 BsnappedPosition;

    Animator Ani;

    void Update()
    {
        MouseCast();
        BuildCheck();
    }

    GameObject _tempRoad;
    GameObject _editBuildObj;
    bool _roadPlace = false;
    public static bool _buildCheck = false;
    public bool _editBuild = false;
    float Timer = 0;

    void MouseCast()
    {
        // for UI check
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }
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
                _editBuildObj.transform.position = BsnappedPosition;
                Ani.SetTrigger("Place");
                _editBuildObj = null;
                _editBuild = false;
                _buildCheck = false;
            }
        }
        _current = BsnappedPosition;
        if (Input.GetMouseButtonDown(0) && _isRoad)
        {
            _roadPlace = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            _roadPlace = false;
        }
        if (_isGrounded && _past != _current && !_isBuild && _roadPlace)
        {
            GameData._roadCount++;
            _tempRoad = Instantiate(_road, _roadParent);
            _tempRoad.transform.position = _current;
            _past = _current;
        }
        if (Input.GetMouseButton(1) && _isRoad)
        {
            if (((1 << Hit.collider.gameObject.layer) & _roadMask) != 0)
            {
                GameData._roadCount--;
                _past = _current;
                Destroy(Hit.collider.gameObject);
            }
        }
    }

    [Header("Build Objs")]
    [Tooltip("Insert all the Build Prefabs")]
    public GameObject[] _build;
    GameObject _activeCube = null;
    bool _isBuild = false;
    public Transform _buildParent;
    public Transform _roadParent;

    public void Build(int Cube)
    {
        if (!_activeCube) _activeCube = Instantiate(_build[Cube],_hitPos, Quaternion.identity, _buildParent);
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
                GameData._buildCount++;
                _activeCube.transform.position = BsnappedPosition;
                Ani.SetTrigger("Place");
                _activeCube = null;
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
