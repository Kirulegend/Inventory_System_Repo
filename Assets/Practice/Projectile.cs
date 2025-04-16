using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;

public class PhysicsController : MonoBehaviour
{
    public bool _useManualGravity = false;
    public bool _hover = false;
    public float _gravity = -9.81f;
    //Vector3 _velocity;
    Rigidbody _rb;

    public GameObject _gameObject;
    public Transform _transform;

    public float _launchForce = 10f;
    public float _angle = 45;
    public int _res = 30;
    public float _timeStamp = 0.1f;

    public LineRenderer _lineRenderer;
    Vector3 _velocity;

    void Start()    
    {
        _rb = GetComponent<Rigidbody>();
        _lineRenderer = GetComponent<LineRenderer>();
        _gravity = Mathf.Abs(Physics.gravity.y);
    }
    void Update()
    {
        ShowTri();
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
        if (_useManualGravity)
        {
            
            _velocity.y += -9.81f * Time.deltaTime;
            transform.position += _velocity * Time.deltaTime;
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _rb.AddForce(transform.up * 2, ForceMode.Impulse);
        }
        if (_hover && Input.GetKey(KeyCode.Space))
        {
            float _hoverError = 1f - transform.position.y;
            _rb.AddForce(Vector3.up * _hoverError * 2f);
        }
    }
    void ShowTri()
    {
        _lineRenderer.positionCount = _res;
        float _radAngle = _angle * Mathf.Deg2Rad;
        Vector3 _velocity = new Vector3(_transform.forward.x,
                                        Mathf.Sin(_radAngle),
                                        _transform.forward.z).normalized * _launchForce;
        for(int i = 0; i < _res; i++)
        {
            float _t = i * _timeStamp;

            Vector3 _point = _transform.position + _velocity * _t;
            _point.y = _transform.position.y + (_velocity.y * _t) - .5f * _gravity * _t * _t;
            _lineRenderer.SetPosition(i, _point);
        }

    }
    void Shoot()
    {
        GameObject _gObj = Instantiate(_gameObject, _transform.position, Quaternion.identity);
        Rigidbody _rb = _gObj.GetComponent<Rigidbody>();

        float _radAngle = _angle * Mathf.Deg2Rad;
        Vector3 _velocity = new Vector3(_transform.forward.x, 
                                        Mathf.Sin(_radAngle), 
                                        _transform.forward.z).normalized * _launchForce;
    }
    void OnCollisionEnter(Collision collision)
    {
        _useManualGravity = false;
        //if (_useManualGravity)
        //{
        //    ContactPoint _contact = collision.contacts[0];
        //    _velocity = Vector3.Reflect(_velocity, _contact.normal * .8f);
        //}
    }
    //void OnCollisionExit(Collision collision)
    //{
    //    _useManualGravity = true;
    //}
}
