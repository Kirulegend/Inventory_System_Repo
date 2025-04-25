using System.Diagnostics.Contracts;
using UnityEditor.PackageManager;
using UnityEngine;

public class RoadPlacement : MonoBehaviour
{
    Vector3 _hitPos;
    Vector3 _pos;
    Vector3 _orgin;
    Vector3 _current;
    Vector3 _past;
    public int _gridSize;
    public GameObject _road;
    GameObject _tempRoad;

    public bool _left = false;
    public bool _right = false;
    public bool _leftO = false;
    public bool _rightO = false;
    void Start()
    {
        _tempRoad = Instantiate(_road);
    }
    // Update is called once per frame
    void Update()
    {
        _orgin = new Vector3(_tempRoad.transform.position.x, _tempRoad.transform.position.y * 2, _tempRoad.transform.position.z);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            _hitPos = hit.point;
        }
        RayCast();
        _pos = _hitPos;
        Vector3 snappedPosition = new Vector3(
        Mathf.Round(_pos.x / _gridSize) * _gridSize,
        _pos.y,
        Mathf.Round(_pos.z / _gridSize) * _gridSize
        );
        if (Input.GetMouseButton(0))
        {
            _tempRoad.transform.position = snappedPosition;
        }
    }

    void RayCast()
    {
        if (Physics.Raycast(_orgin, _tempRoad.transform.forward, out RaycastHit AhitInfo, 2))
        {
            if (AhitInfo.collider.gameObject.CompareTag("Road"))
            {
                Debug.Log(AhitInfo);
                Debug.DrawRay(_orgin, _tempRoad.transform.forward * 10f, Color.green);
                _left = true;
            }
            else
            {
                _left = false;
                Debug.DrawRay(_orgin, _tempRoad.transform.forward * 10f, Color.red);
            }
        }
        else
        {
            _left = false;
            Debug.DrawRay(_orgin, _tempRoad.transform.forward * 10f, Color.red);
        }

        if (Physics.Raycast(_orgin, -_tempRoad.transform.forward, out RaycastHit BhitInfo, 2))
        {
            if (BhitInfo.collider.gameObject.CompareTag("Road"))
            {
                Debug.Log(BhitInfo);
                Debug.DrawRay(_orgin, -_tempRoad.transform.forward * 10f, Color.green);
                _leftO = true;
            }
            else
            {
                _leftO = false;
                Debug.DrawRay(_orgin, -_tempRoad.transform.forward * 10f, Color.red);
            }
        }
        else
        {
            _leftO = false;
            Debug.DrawRay(_orgin, -_tempRoad.transform.forward * 10f, Color.red);
        }

        if (Physics.Raycast(_orgin, _tempRoad.transform.right, out RaycastHit ChitInfo, 2))
        {
            if (ChitInfo.collider.gameObject.CompareTag("Road"))
            {
                Debug.Log(ChitInfo);
                Debug.DrawRay(_orgin, _tempRoad.transform.right * 10f, Color.green);
                _right = true;
            }
            else
            {
                _right = false;
                Debug.DrawRay(_orgin, _tempRoad.transform.right * 10f, Color.red);
            }
        }
        else
        {
            _right = false;
            Debug.DrawRay(_orgin, _tempRoad.transform.right * 10f, Color.red);
        }

        if (Physics.Raycast(_orgin, -_tempRoad.transform.right, out RaycastHit DhitInfo, 2))
        {
            if (DhitInfo.collider.gameObject.CompareTag("Road"))
            {
                Debug.Log(DhitInfo);
                Debug.DrawRay(_orgin, -_tempRoad.transform.right * 10f, Color.green);
                _rightO = true;
            }
            else
            {
                _rightO = false;
                Debug.DrawRay(_orgin, -_tempRoad.transform.right * 10f, Color.red);
            }
        }
        else
        {
            _rightO = false;
            Debug.DrawRay(_orgin, -_tempRoad.transform.right * 10f, Color.red);
        }
    }
}
