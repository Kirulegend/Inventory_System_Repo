using NUnit.Framework.Internal.Filters;
using Unity.VisualScripting;
using UnityEngine;

public class BuildObj : MonoBehaviour
{
    public static bool _isGround = false;
    [Header("LayerMasks")]
    [Tooltip("Select the Ground Layer Mask")]
    public LayerMask _ground;
    BoxCollider _bc;
    bool _rotate = false;
    void Start()
    {
        _bc = GetComponent<BoxCollider>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            _rotate = !_rotate;
        }
        RayCast();
    }
    void RayCast()
    {
        if (!Placement._placed)
        {
            if (_bc.size.z == 2.1f)
            {
                if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hitInfo1))
                {
                    if (((1 << hitInfo1.collider.gameObject.layer) & _ground) != 0)
                    {
                        Debug.DrawRay(transform.position, -transform.up * 10, Color.green);
                        _isGround = true;
                    }
                    else
                    {
                        Debug.DrawRay(transform.position, -transform.up * 10, Color.red);
                        _isGround = false;
                    }
                }
            }
            if (!_rotate)
            {
                if (_bc.size.z == 4.1f)
                {
                    Vector3 Left = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2);
                    if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hitInfo1) && Physics.Raycast(Left, -transform.up, out RaycastHit hitInfo2))
                    {
                        if (((1 << hitInfo1.collider.gameObject.layer) & _ground) != 0 && ((1 << hitInfo2.collider.gameObject.layer) & _ground) != 0)
                        {
                            Debug.DrawRay(Left, -transform.up * 10, Color.green);
                            Debug.DrawRay(transform.position, -transform.up * 10, Color.green);
                            _isGround = true;
                        }
                        else
                        {
                            Debug.DrawRay(Left, -transform.up * 10, Color.red);
                            Debug.DrawRay(transform.position, -transform.up * 10, Color.red);
                            _isGround = false;
                        }
                    }
                }
                if (_bc.size.z == 6.1f)
                {
                    Vector3 Left = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2);
                    Vector3 Right = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2);
                    if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hitInfo1) && Physics.Raycast(Left, -transform.up, out RaycastHit hitInfo2) && Physics.Raycast(Right, -transform.up, out RaycastHit hitInfo3))
                    {
                        if (((1 << hitInfo1.collider.gameObject.layer) & _ground) != 0 && ((1 << hitInfo2.collider.gameObject.layer) & _ground) != 0 && ((1 << hitInfo3.collider.gameObject.layer) & _ground) != 0)
                        {
                            Debug.DrawRay(Left, -transform.up * 10, Color.green);
                            Debug.DrawRay(Right, -transform.up * 10, Color.green);
                            Debug.DrawRay(transform.position, -transform.up * 10, Color.green);
                            _isGround = true;
                        }
                        else
                        {
                            Debug.DrawRay(Left, -transform.up * 10, Color.red);
                            Debug.DrawRay(Right, -transform.up * 10, Color.red);
                            Debug.DrawRay(transform.position, -transform.up * 10, Color.red);
                            _isGround = false;
                        }
                    }
                }
            }
            else
            {
                if (_bc.size.z == 4.1f)
                {
                    Vector3 Left = new Vector3(transform.position.x - 2, transform.position.y, transform.position.z);
                    if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hitInfo1) && Physics.Raycast(Left, -transform.up, out RaycastHit hitInfo2))
                    {
                        if (((1 << hitInfo1.collider.gameObject.layer) & _ground) != 0 && ((1 << hitInfo2.collider.gameObject.layer) & _ground) != 0)
                        {
                            Debug.DrawRay(Left, -transform.up * 10, Color.green);
                            Debug.DrawRay(transform.position, -transform.up * 10, Color.green);
                            _isGround = true;
                        }
                        else
                        {
                            Debug.DrawRay(Left, -transform.up * 10, Color.red);
                            Debug.DrawRay(transform.position, -transform.up * 10, Color.red);
                            _isGround = false;
                        }
                    }
                }
                if (_bc.size.z == 6.1f)
                {
                    Vector3 Left = new Vector3(transform.position.x + 2, transform.position.y, transform.position.z);
                    Vector3 Right = new Vector3(transform.position.x - 2, transform.position.y, transform.position.z);
                    if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hitInfo1) && Physics.Raycast(Left, -transform.up, out RaycastHit hitInfo2) && Physics.Raycast(Right, -transform.up, out RaycastHit hitInfo3))
                    {
                        if (((1 << hitInfo1.collider.gameObject.layer) & _ground) != 0 && ((1 << hitInfo2.collider.gameObject.layer) & _ground) != 0 && ((1 << hitInfo3.collider.gameObject.layer) & _ground) != 0)
                        {
                            Debug.DrawRay(Left, -transform.up * 10, Color.green);
                            Debug.DrawRay(Right, -transform.up * 10, Color.green);
                            Debug.DrawRay(transform.position, -transform.up * 10, Color.green);
                            _isGround = true;
                        }
                        else
                        {
                            Debug.DrawRay(Left, -transform.up * 10, Color.red);
                            Debug.DrawRay(Right, -transform.up * 10, Color.red);
                            Debug.DrawRay(transform.position, -transform.up * 10, Color.red);
                            _isGround = false;
                        }
                    }
                }
            }
            if (_bc.size.z == 20f)
            {
                bool allHitGround = true;
                for (int i = 0; i < _bc.size.z / 2; i++)
                {
                    for (int j = 0; j < _bc.size.z / 2; j++)
                    {
                        Vector3 pos = new Vector3(transform.position.x + i * 2, transform.position.y, transform.position.z + j * 2);
                        if (Physics.Raycast(pos, -transform.up, out RaycastHit hitInfo))
                        {
                            if (((1 << hitInfo.collider.gameObject.layer) & _ground) != 0)
                            {
                                Debug.DrawRay(pos, -transform.up * 10, Color.green);
                                continue;
                            }
                        }
                        Debug.DrawRay(pos, -transform.up * 10, Color.red);
                        allHitGround = false;
                        break;
                    }
                    if (!allHitGround)
                        break;
                }
                _isGround = allHitGround;
            }
        }
    }
}
