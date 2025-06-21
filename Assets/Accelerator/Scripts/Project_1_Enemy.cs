using UnityEngine;
using UnityEngine.AI;

public class Project_1_Enemy : MonoBehaviour
{
    [SerializeField] NavMeshAgent _nav;
    [SerializeField] Vector3 _player;
    [SerializeField] Animator _ani;
    [SerializeField] Transform[] _cubes;
    void Start()
    {
        _ani = GetComponent<Animator>();
        _nav = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        Animations();
        if (_target)
        {
            _wasTargetFalse = true;
            _nav.SetDestination(_player);
            _nav.updateRotation = false;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_player - transform.position), 10 * Time.deltaTime);
        }
        else
        {
            if (_wasTargetFalse)
            {
                _nav.ResetPath();
                _wasTargetFalse = false;
            }
            _nav.updateRotation = true;
        }
        Ray(50);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_target && _nav.speed != 4) _nav.speed = 4;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_target && _nav.speed != 2) _nav.speed = 2;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (_target && _nav.speed != 4) _nav.speed = 4;
            if (!_target && _nav.speed != 2) _nav.speed = 2;
        }
    }
    [SerializeField] bool _target = false;
    [SerializeField] bool _wasTargetFalse = false;
    [SerializeField] float _loseTargetDelay = 0.1f;
    float _loseTimer = 0f;
    void Ray(int Num)
    {
        float angleStep = 360f / Num;
        bool hitPlayer = false;
        for (int i = 0; i < Num; i++)
        {
            float angle = i * angleStep;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;
            if (Physics.Raycast(transform.position + transform.up, direction, out RaycastHit hit, 10))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    _player = hit.collider.transform.position;
                    Debug.DrawRay(transform.position + transform.up, direction * hit.distance, Color.green);
                    hitPlayer = true;
                }
                //else Debug.DrawRay(transform.position + transform.up, direction * hit.distance, Color.red);
            }
        }
        if (hitPlayer)
        {
            _target = true;
            _loseTimer = 0f;
        }
        else
        {
            if (_target)
            {
                _loseTimer += Time.deltaTime;
                if (_loseTimer >= _loseTargetDelay) _target = false;
            }
        }
    }
    public void RandDes()
    {
        if (!_target && _nav.velocity.magnitude == 0) _nav.SetDestination(_cubes[Random.Range(0, _cubes.Length)].position); _nav.speed = 2;
    }
    void Animations()
    {
        _ani.SetFloat("Speed", _nav.velocity.magnitude);
        _ani.SetBool("Attack", (_nav.remainingDistance <= _nav.stoppingDistance && _target));
        _ani.SetBool("Target", _target);
    }
}
