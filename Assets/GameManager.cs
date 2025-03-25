using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool _hasKey = false;
    public static bool _nearDoor = false;
    public Rigidbody _objDoor;
    private bool _doorOpened = false;
    void Update()
    {
        if(_hasKey && !_doorOpened)
        {
            _objDoor.isKinematic = false;
            _objDoor.AddForce(Vector3.up, ForceMode.Force);
        }
        if(_objDoor.transform.position.y >= 4f && !_doorOpened)
        {
            _doorOpened = true;
            _objDoor.isKinematic = true;
            _hasKey = false;
        }
    }
}
