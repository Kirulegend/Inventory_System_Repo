using UnityEngine;

public class Project_1_Player : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 10;
    [SerializeField] float _jumpForce = 10;
    [SerializeField] float _mouseSens = 100;
    [SerializeField] Vector2 _mouseRot;
    [SerializeField] Vector2 _mouseXBound;
    [SerializeField] LayerMask _groundMask;
    [SerializeField] Vector2 _playerMovement;
    Rigidbody _rb;
    [SerializeField] Transform _camtransform;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        _rb = GetComponent<Rigidbody>();
        _camtransform = transform.Find("Cam").transform;
    }
    void Update()
    {
        PlayerMovement();
        MouseMovement();
        Debug.Log("Hi");
    }
    void MouseMovement()
    {
        _mouseRot.x += Input.GetAxisRaw("Mouse X") * _mouseSens;
        _mouseRot.y += Input.GetAxisRaw("Mouse Y") * _mouseSens;
        _mouseRot.y = Mathf.Clamp(_mouseRot.y, _mouseXBound.x, _mouseXBound.y);
        transform.localRotation = Quaternion.Euler(0, _mouseRot.x, 0);
        _camtransform.localRotation = Quaternion.Euler(-_mouseRot.y, 0, 0);
    }
    void PlayerMovement()
    {
        _playerMovement.x = Input.GetAxisRaw("Horizontal");
        _playerMovement.y = Input.GetAxisRaw("Vertical");
        if ((_playerMovement.x != 0 || _playerMovement.y != 0))
        {
            Vector3 moveDirection = transform.right * _playerMovement.x + transform.forward * _playerMovement.y;
            moveDirection.Normalize();
            _rb.linearVelocity = new Vector3(moveDirection.x * _moveSpeed, _rb.linearVelocity.y, moveDirection.z * _moveSpeed);
        }
        else
        {
            _rb.linearVelocity = new Vector3(0, _rb.linearVelocity.y, 0);
        }
        if ((Input.GetKeyDown(KeyCode.LeftShift)))
        {
            _moveSpeed *= 1.5f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _moveSpeed /= 1.5f;
        }
        if (GroundCheck())
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, _jumpForce, _rb.linearVelocity.z);
            }
            else
            {
                _rb.linearVelocity = new Vector3(_rb.linearVelocity.x, _rb.linearVelocity.y, _rb.linearVelocity.z);
            }
        }
    }
    bool GroundCheck()
    {
        return Physics.BoxCast(transform.position - Vector3.up * .5f, new Vector3(0.5f, 0.25f, 0.5f), Vector3.down, Quaternion.identity, 0.5f, _groundMask);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = GroundCheck() ? Color.green : Color.red;
        Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y + -1f, transform.position.z), new Vector3(1, .25f, 1));
    }
}
