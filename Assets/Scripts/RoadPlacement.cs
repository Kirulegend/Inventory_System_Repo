using System.Diagnostics.Contracts;
using UnityEditor.PackageManager;
using UnityEngine;

public class RoadPlacement : MonoBehaviour
{
    Vector3 _hitPos;
    Vector3 _pos;
    Vector3 _current;
    Vector3 _past;
    public int _gridSize;
    public LayerMask _groundLayer;
    bool _isGrounded = false;

    public GameObject _road;

    void Update()
    {
        MouseCast();
    }

    GameObject _tempRoad;
    
    void MouseCast()
    {
        //Debug.Log(_isGrounded);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if(((1 << hit.collider.gameObject.layer) & _groundLayer) != 0)
            {
                _hitPos = hit.point;
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
        _pos = _hitPos;
        Vector3 snappedPosition = new Vector3(
        Mathf.Round(_pos.x / _gridSize) * _gridSize,
        _pos.y,
        Mathf.Round(_pos.z / _gridSize) * _gridSize
        );
        _current = snappedPosition;
        if (Input.GetMouseButton(0))
        {
            if (_isGrounded && _past != _current)
            {
                _tempRoad = Instantiate(_road);
                _tempRoad.transform.position = _current;
            }
        }
    }
}
