using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool _hasKey = false;
    public static bool _nearDoor = false;
    public Rigidbody _objDoor;
    void Update()
    {
        if(_hasKey)
        {
            _objDoor.isKinematic = false;
            _objDoor.AddForce(Vector3.up, ForceMode.Force);
        }
        if(_objDoor.transform.position.y >= 3.5f)
        {
            _objDoor.isKinematic = true;
            _objDoor.linearVelocity = Vector3.zero;
            _hasKey = false;
        }
    }
}
