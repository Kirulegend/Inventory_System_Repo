using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject _bullet;
    public GameObject _Bullet;
    public Transform _instantiatePos;
    public Vector3 _playerPos;
    public Rigidbody _bulletRigi;
    public Rigidbody _enemyRigi;
    public bool _isPlayer = false;
    public bool _isFiring = false;
    void Update()
    {
        if (_isPlayer)
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
        //Vector3 direction = _playerPos - _Bullet.transform.position;
        //Quaternion targetRotation = Quaternion.LookRotation(direction);
        //_Bullet.transform.rotation = targetRotation;
        _bulletRigi = _Bullet.GetComponent<Rigidbody>();
        _bulletRigi.AddForce(Vector3.back * 200, ForceMode.Force);
        Destroy(_Bullet, 4);
        StartCoroutine(TimeBullet());
    }
    IEnumerator TimeBullet()
    {
        yield return new WaitForSeconds(1);
        _isFiring = true;
    }
}
