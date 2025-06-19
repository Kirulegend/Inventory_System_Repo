using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    public int _money = 1000;
    public Rigidbody _rb;
    public float _moveSpeed = 5;
    void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Movement()
    {
        float V = Input.GetAxis("Vertical");
        float H = Input.GetAxis("Horizontal");
        if(H != 0 || V != 0)
        {
            Vector3 moveDirection = transform.right * H + transform.forward * V;
            moveDirection.Normalize();
            _rb.linearVelocity = new Vector3(moveDirection.x * _moveSpeed, _rb.linearVelocity.y, moveDirection.z * _moveSpeed);
        }
        if(H == 0 && V == 0)
        {
            _rb.linearVelocity = new Vector3(0, _rb.linearVelocity.y, 0);
        }
    }
    void Update()
    {
        Movement();
    }
}
