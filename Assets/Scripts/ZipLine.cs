using UnityEngine;

public class ZipLine : MonoBehaviour
{
    public Transform _zipLine;
    public Transform _zipLineStart;
    public Transform _zipLineEnd;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager._zipTrig = true;
            GameManager._zipLine = _zipLine;
            GameManager._zipLineStart = _zipLineStart;
            GameManager._zipLineEnd = _zipLineEnd;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager._zipTrig = false;
        }
    }

}
