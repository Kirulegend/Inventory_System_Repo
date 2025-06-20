using UnityEngine;
using UnityEngine.AI;

public class Project_1_Enemy : MonoBehaviour
{
    [SerializeField] NavMeshAgent _nav;
    [SerializeField] Transform _player;
    [SerializeField] Animator _ani;
    [SerializeField] Transform[] _cubes;
    void Start()
    {
        _ani = GetComponent<Animator>();
        _player = GameObject.Find("Player").transform;
        _nav = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        _ani.SetFloat("Speed", _nav.velocity.magnitude);
        _ani.SetBool("Attack", (_nav.remainingDistance <= _nav.stoppingDistance && _check));
        _ani.SetBool("Target", _target);
        if(!_check && _nav.velocity.magnitude == 0)
        {
            _nav.SetDestination(_cubes[Random.Range(0, _cubes.Length)].position);
        }
        if (_target)
        {
            _nav.SetDestination(_targetPos);
            _nav.updateRotation = false;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(_targetPos - transform.position), 10 * Time.deltaTime);
        }
        else
        {
            _nav.updateRotation = true;
        }
        Ray(100);
        if (!_target && _wasTargetFalse)
        {
            _nav.ResetPath();
            _wasTargetFalse = false;
        }
        else if (_target)
        {
            _wasTargetFalse = true;
        }
    }
    [SerializeField] bool _check = false;
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            _nav.speed *= 2;
            _check = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _nav.speed /= 2;
            _check = false;
            //_ani.SetBool("Attack", false);
            //_nav.ResetPath();
        }
    }
    [SerializeField] bool _target = false;
    bool _wasTargetFalse = false;
    [SerializeField] Vector3 _targetPos = Vector3.zero;
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
                    _targetPos = hit.collider.transform.position;
                    Debug.Log(hit.collider.name);
                    Debug.DrawRay(transform.position + transform.up, direction * hit.distance, Color.green);
                    hitPlayer = true;
                }
                else
                {
                    Debug.DrawRay(transform.position + transform.up, direction * hit.distance, Color.red);
                }
            }
            else
            {
                Debug.DrawRay(transform.position + transform.up, direction * 10, Color.red);
            }
        }
        _target = hitPlayer;
    }
}
