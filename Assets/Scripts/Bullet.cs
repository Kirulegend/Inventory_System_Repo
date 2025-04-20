using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Coroutine routine;
    Rigidbody _bulletRigi;
    void Awake()
    {
        _bulletRigi = GetComponent<Rigidbody>();
    }
    void OnEnable()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;
        }

        routine = StartCoroutine(BulletCheck());
    }

    IEnumerator BulletCheck()
    {
        yield return new WaitForSeconds(2.5f);
        _bulletRigi.linearVelocity = Vector3.zero;
        _bulletRigi.angularVelocity = Vector3.zero;
        gameObject.SetActive(false);
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    gameObject.SetActive(false);
    //}
}
