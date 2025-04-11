using TMPro;
using UnityEngine;

public class Test1 : MonoBehaviour
{
    void TakeDamage(int Damage)
    {
        Debug.Log($"Health : {Damage}");
    }
    private void OnEnable()
    {
        PracicePlayer.HealthE += TakeDamage;
    }
    private void OnDisable()
    {
        PracicePlayer.HealthE -= TakeDamage;
    }
    void Start()
    {
        Test.Instance.TempMethod();   
    }

}
