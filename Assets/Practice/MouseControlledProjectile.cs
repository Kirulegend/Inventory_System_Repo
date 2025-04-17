using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class MouseControlledProjectile : MonoBehaviour
{
    public GameObject _ballPrefab;
    public Transform _shootPos;
    public float _maxPower;
    public float _powerMultiplayer = .5f;

    public int _tPoint = 30;
    public float _timeStamp = .1f;

    public Camera _cam;

    LineRenderer _lineRenderer;
    bool _isDragging = false;
    Vector3 _dragStart;
    Vector3 _dragEnd;
    float _gravity;

    void Start()
    {
        _shootPos.position = transform.forward * 2;
        _lineRenderer = GetComponent<LineRenderer>();
        _gravity = Mathf.Abs(Physics.gravity.y);
        if (!_cam)
        {
            _cam = Camera.main;
        }
    }

    void Update()
    {
        HandleMouseInput();
    }

    void HandleMouseInput()
    {
        Ray _ray;
        if (Input.GetMouseButtonDown(0))
        {
            _ray = _cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(_ray, out RaycastHit _hit))
            {
                _dragStart = _hit.point;
                _isDragging = true;
            }
        }
        if(Input.GetMouseButton(0) && _isDragging)
        {
            _ray = _cam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(_ray, out RaycastHit _hit))
            {
                _dragEnd = _hit.point;
                Vector3 _direction = _dragStart - _dragEnd;
                float _power = Mathf.Clamp(_direction.magnitude * _powerMultiplayer, 0f, _maxPower);
                Vector3 _velocity = _direction.normalized * _power;

                
            }
        }
        if(Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
            Vector3 _direction = _dragStart - _dragEnd;
            float _power = Mathf.Clamp(_direction.magnitude * _powerMultiplayer, 0f, _maxPower);
            Vector3 _velocity = _direction.normalized * _power;

            ShootBall(_velocity);
            ShowTri(_dragStart, _velocity);
            _lineRenderer.positionCount = 0;
        }
    }
    
    void ShootBall(Vector3 _velocity)
    {
        GameObject _ball = Instantiate(_ballPrefab, _shootPos.position, Quaternion.identity);
        Rigidbody _rb = _ball.GetComponent<Rigidbody>();
        _rb.linearVelocity = _velocity;
    }

    void ShowTri(Vector3 _startPos, Vector3 _velocity)
    {
        _lineRenderer.positionCount = _tPoint;
        for(int i = 0; i < _tPoint; i++)
        {
            float _t = i * _timeStamp;
            Vector3 _point = _startPos + _velocity * _t;
            _point.y = _startPos.y + (_velocity.y * _t - 0.5f * _gravity * _t * _t);
            _lineRenderer.SetPosition(i, _point);
        }
    }
}
