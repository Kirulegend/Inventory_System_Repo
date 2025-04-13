using UnityEngine;

public class Boundary : MonoBehaviour
{
    [Header("Check Point")]
    [Tooltip("Attach the Player Respawn Position")]
    [SerializeField] Transform _CheckPoint;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.position = _CheckPoint.position;
        }
    }
}
