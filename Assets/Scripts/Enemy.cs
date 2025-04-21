using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameObject _bullet;
    GameObject _Bullet;
    Transform _instantiatePos;
    Vector3 _playerPos;
    Rigidbody _bulletRigi;
    bool _isPlayer = false;
    bool _isFiring = false;

    void Start()
    {
        _instantiatePos = transform.Find("BulletSpawn");
        _bullet = Resources.Load<GameObject>("Bullet");
    }

    void Update()
    {
        if (_isPlayer && !GameManager._isInv)
        {
            if (_isFiring)
            {
                _isFiring = false;
                InstantiateBullet();
            }
            Vector3 direction = _playerPos - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerPos = other.transform.position;
            _isFiring = true;
            _isPlayer = true;
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _playerPos = other.transform.position;
            _isPlayer = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _isPlayer = false;
        }
    }

    void InstantiateBullet()
    {
        _Bullet = Instantiate(_bullet, _instantiatePos.position, Quaternion.identity);
        _bullet.tag = "Bullet";
        _bulletRigi = _Bullet.GetComponent<Rigidbody>();
        Vector3 direction = _playerPos - _Bullet.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        _Bullet.transform.rotation = targetRotation;
        _bulletRigi.AddForce(transform.forward * 200, ForceMode.Force);
        Destroy(_Bullet, 3);
        StartCoroutine(TimeBullet());
    }

    IEnumerator TimeBullet()
    {
        yield return new WaitForSeconds(1);
        _isFiring = true;
    }
}
