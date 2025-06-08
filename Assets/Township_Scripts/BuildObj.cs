using NUnit.Framework.Internal.Filters;
using Unity.VisualScripting;
using UnityEngine;

public class BuildObj : MonoBehaviour
{
    public static bool _isGround = false;
    public LayerMask _ground;
    BoxCollider _bc;
    void Start()
    {
        _bc = GetComponent<BoxCollider>();
    }
    void Update()
    {
        RayCast();
    }
    void RayCast()
    {
        if (Placement._buildCheck)
        {
            if (_bc.size.z == 2.1f)
            {
                if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hitInfo1))
                {
                    if (((1 << hitInfo1.collider.gameObject.layer) & _ground) != 0)
                    {
                        _isGround = true;
                    }
                    else
                    {
                        _isGround = false;
                    }
                }
            }
            if (_bc.size.z == 4.1f)
            {
                Vector3 Left = new Vector3(transform.position.x, transform.position.y, transform.position.z - 2);
                if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hitInfo1) && Physics.Raycast(Left, -transform.up, out RaycastHit hitInfo2))
                {
                    if (((1 << hitInfo1.collider.gameObject.layer) & _ground) != 0 && ((1 << hitInfo2.collider.gameObject.layer) & _ground) != 0)
                    {
                        _isGround = true;
                    }
                    else
                    {
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
                        _isGround = true;
                    }
                    else
                    {
                        _isGround = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < _bc.size.z / 2; i++)
                {
                    for (int j = 0; j < _bc.size.z / 2; j++)
                    {
                        Vector3 Pos = new Vector3(transform.position.x + i * 2, transform.position.y, transform.position.z + j * 2);
                        if (Physics.Raycast(Pos, -transform.up, out RaycastHit hitInfo))
                        {
                            if (((1 << hitInfo.collider.gameObject.layer) & _ground) != 0)
                            {
                                _isGround = true;
                                Debug.DrawRay(Pos, -transform.up * 10, Color.green);
                            }
                            else
                            {
                                _isGround = false;
                            }
                        }
                    }
                }
            }
        }
    }
}
