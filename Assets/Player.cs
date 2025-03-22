using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Vector3 rawInputMovement;
    public float _Hor;
    public float _Ver;
    public float _moveSpeed;
    public float _jumpForce;
    public Rigidbody _rb3D;
    public LayerMask _layerMask;
    public Inventory _inv;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //_inv = GetComponent<Inventory>();
        if (_inv == null)
        {
            Debug.LogError("Inventory component not found on player!");
        }
        _rb3D = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovementKB();
    }

    void PlayerMovementKB()
    {
        //Move
        _Hor = Input.GetAxisRaw("Horizontal");
        _Ver = Input.GetAxisRaw("Vertical");
        if (_Hor != 0 || _Ver != 0)
        {
            _rb3D.linearVelocity = new Vector3(_Hor * _moveSpeed, _rb3D.linearVelocity.y, _Ver * _moveSpeed);
        }
        else
        {
            _rb3D.linearVelocity = new Vector3(0, _rb3D.linearVelocity.y, 0);
        }
        //Sprint
        if ((Input.GetKeyDown(KeyCode.LeftShift)))
        {
            _moveSpeed *= 2;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _moveSpeed /= 2;
        }
        //Jump
        if (Input.GetKeyDown(KeyCode.Space) && GroundCheck())
        {
            _rb3D.linearVelocity = new Vector3(_rb3D.linearVelocity.x, _jumpForce, _rb3D.linearVelocity.z);
        }
    }

    bool GroundCheck()
    {
        return Physics.BoxCast(transform.position, new Vector3(1, .5f, 1), Vector3.down, Quaternion.identity, 1f, _layerMask);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = GroundCheck() ? Color.green : Color.red;
        Gizmos.DrawCube(new Vector3(transform.position.x, transform.position.y + -1f, transform.position.z), new Vector3(1, .25f, 1));
    }

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("Triggered with: " + other.gameObject.name);
        if (other.CompareTag("Item"))
        {
            GameObject newItem = other.gameObject.GetComponent<Item>()._itemPrefab;
            _inv.AddItem(newItem);
            //Destroy(other.gameObject, 5);
        }
    }
}
