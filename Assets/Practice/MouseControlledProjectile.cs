using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(LineRenderer))]
public class MouseControlledProjectile : MonoBehaviour
{
    public GameObject _ballPrefab;
    public float _maxPower = 10f;
    public float _powerMultiplier = 0.5f;
    public int _tPoint = 30;
    public float _timeStamp = 0.1f;
    public Camera _cam;

    private LineRenderer _lineRenderer;
    private bool _isDragging = false;
    private Vector3 _spawn;
    private Vector3 _dragStart;
    private Vector3 _dragEnd;
    private float _gravity;
    private Plane _fallbackPlane;

    void Start()
    {
        _spawn = transform.position+(transform.up*1.5f);
        _lineRenderer = GetComponent<LineRenderer>();
        _gravity = Mathf.Abs(Physics.gravity.y);
        _fallbackPlane = new Plane(Vector3.up, Vector3.zero);
        if (!_cam)
        {
            _cam = Camera.main;
        }
        _lineRenderer.positionCount = 0;
    }

    void Update()
    {
        HandleMouseInput();
    }

    void HandleMouseInput()
    {
        Ray ray = _cam.ScreenPointToRay(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            if (GetWorldPoint(ray, out _dragStart))
            {
                _isDragging = true;
            }
        }

        if (Input.GetMouseButton(0) && _isDragging)
        {
            if (GetWorldPoint(ray, out _dragEnd))
            {
                Vector3 direction = _dragStart - _dragEnd;
                float power = Mathf.Clamp(direction.magnitude * _powerMultiplier, 0f, _maxPower);
                Vector3 velocity = direction.normalized * power;
                ShowTrajectory(_spawn, velocity);
            }
        }

        if (Input.GetMouseButtonUp(0) && _isDragging)
        {
            _isDragging = false;
            if (GetWorldPoint(ray, out _dragEnd))
            {
                Vector3 direction = _dragStart - _dragEnd;
                float power = Mathf.Clamp(direction.magnitude * _powerMultiplier, 0f, _maxPower);
                Vector3 velocity = direction.normalized * power;

                ShootBall(velocity);
            }
            _lineRenderer.positionCount = 0;
        }

        if (!_isDragging)
        {
            _lineRenderer.positionCount = 0;
        }
    }

    bool GetWorldPoint(Ray ray, out Vector3 point)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            point = hit.point;
            return true;
        }
        if (_fallbackPlane.Raycast(ray, out float enter))
        {
            point = ray.GetPoint(enter);
            return true;
        }

        point = Vector3.zero;
        return false;
    }

    void ShootBall(Vector3 velocity)
    {
        GameObject ball = Instantiate(_ballPrefab, _spawn, Quaternion.identity);
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.linearVelocity = velocity;
    }

    void ShowTrajectory(Vector3 startPos, Vector3 velocity)
    {
        _lineRenderer.positionCount = _tPoint;
        for (int i = 0; i < _tPoint; i++)
        {
            float t = i * _timeStamp;
            Vector3 point = startPos + velocity * t;
            point.y = startPos.y + (velocity.y * t - 0.5f * _gravity * t * t);
            _lineRenderer.SetPosition(i, point);
        }
    }
}