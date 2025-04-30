using System.Collections;
using System.Diagnostics.Contracts;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering;

public class RoadPlacement : MonoBehaviour
{
    Vector3 _hitPos;
    Vector3 _pos;
    Vector3 _current;
    Vector3 _past;
    public int _gridSize;
    public LayerMask _groundLayer;
    public LayerMask _roadMask;
    public LayerMask _ignoreMask;
    bool _isGrounded = false;

    public GameObject _road;
    Vector3 BsnappedPosition;

    void Update()
    {
        MouseCast();
        BuildCheck();
    }

    GameObject _tempRoad;
    
    void MouseCast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ~_ignoreMask))
        {
            _hitPos = hit.point;
            if (((1 << hit.collider.gameObject.layer) & _groundLayer) != 0)
            {
                _pos = _hitPos;
                _isGrounded = true;
            }
            else
            {
                _isGrounded = false;
            }
        }
        else
        {
            _isGrounded = false;
        }
        if(_past != _current)
        {
            _past = _current;
        }
        Vector3 snappedPosition = new Vector3(
        Mathf.Round(_pos.x / _gridSize) * _gridSize,
        _pos.y,
        Mathf.Round(_pos.z / _gridSize) * _gridSize
        );
        _current = snappedPosition;
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
        {
            if (_isGrounded && _past != _current && !_build)
            {
                _tempRoad = Instantiate(_road);
                _tempRoad.transform.position = _current;
            }
        }
        if (Input.GetMouseButton(1))
        {
            if (((1 << hit.collider.gameObject.layer) & _roadMask) != 0)
            {
                Destroy(hit.collider.gameObject);
            }
        }
    }
    public GameObject Cube1;
    public GameObject Cube2;
    public GameObject Cube3;
    public GameObject Cube4;
    public GameObject Cube5;
    GameObject _activeCube = null;
    bool _build = false;

    public void Build(int Cube)
    {
        switch (Cube)
        {
            case 1:
                if (!_activeCube) _activeCube = Instantiate(Cube1, _hitPos, Quaternion.identity);
                _activeCube.layer = _ignoreMask;
                StartCoroutine(BuildTimer());
                break;
            case 2:
                if (!_activeCube) _activeCube = Instantiate(Cube2, _hitPos, Quaternion.identity);
                StartCoroutine(BuildTimer());
                break;
            case 3:
                if (!_activeCube) _activeCube = Instantiate(Cube3, _hitPos, Quaternion.identity);
                StartCoroutine(BuildTimer());
                break;
            case 4:
                if (!_activeCube) _activeCube = Instantiate(Cube4, _hitPos, Quaternion.identity);
                StartCoroutine(BuildTimer());
                break;
            case 5:
                if (!_activeCube) _activeCube = Instantiate(Cube5, _hitPos, Quaternion.identity);
                StartCoroutine(BuildTimer());
                break;
        }
    }
    void BuildCheck()
    {
        BsnappedPosition = new Vector3(
        Mathf.Round(_hitPos.x / _gridSize) * _gridSize,
        _hitPos.y,
        Mathf.Round(_hitPos.z / _gridSize) * _gridSize
        );
        if (_activeCube != null)
        {
            _activeCube.transform.position = BsnappedPosition;
            if (Input.GetMouseButtonUp(0) && _build)
            {
                //_activeCube.layer = 0;
                _activeCube = null;
                _build = false;
            }
        }
    }
    IEnumerator BuildTimer()
    {
        yield return new WaitForSeconds(1);
        _build = true;
    }
}
