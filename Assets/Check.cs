using System.Collections;
using UnityEngine;

public class Check : MonoBehaviour
{
    public int _amount = 20;
    bool _entered = false;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name == "Capsule")
        {
            StartCoroutine(Timer(other.gameObject));
        }
    }
    void OnTriggerExit(Collider other)
    {
        
    }
    IEnumerator Timer(GameObject Player)
    {
        yield return new WaitForSeconds(5);
        Player.GetComponent<PlayerMovement>()._money -= _amount;
        Debug.Log("Money Debeted " + _amount + " ; Current Amount " + Player.GetComponent<PlayerMovement>()._money);
    }
}
