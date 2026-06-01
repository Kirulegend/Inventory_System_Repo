using UnityEngine;
using UnityEngine.AI;

// ===================== Project_1_Enemy =====================

public class Project_1_Enemy : MonoBehaviour
{
    [SerializeField] public NavMeshAgent _nav; // Navigation agent for movement
    [SerializeField] public Animator _ani; // Animator for controlling enemy animations
    [SerializeField] Transform[] _cubes; // List of waypoints to move when idle

    Vector3 _player; // Current known player position
    bool _target = false; // Is player currently detected?
    bool _wasTargetFalse = false; // Tracks state change from true to false

    [SerializeField] float _loseTargetDelay = 0.1f; // Delay before losing target
    float _loseTimer = 0f; // Accumulated timer to lose target

    void Start()
    {
        _ani = GetComponent<Animator>();
        _nav = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        HandleAnimations(); // Update animation states
        UpdateTargeting(); // Handle movement and rotation logic
        PerformSurroundRaycast(36); // Check 360-degree vision around the enemy
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _target)
        {
            _nav.speed = 4f; // Chase speed when in contact with player
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && _target)
        {
            _nav.speed = 2f; // Return to patrol speed
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // Dynamically adjust speed based on targeting state
        _nav.speed = _target ? 4f : 2f;
    }

    void UpdateTargeting()
    {
        if (_target)
        {
            // Move toward player and face them
            _wasTargetFalse = true;
            _nav.SetDestination(_player);
            _nav.updateRotation = false;
            Vector3 lookDir = _player - transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDir), 10f * Time.deltaTime);
        }
        else if (_wasTargetFalse)
        {
            // Stop movement and restore auto-rotation
            _nav.ResetPath();
            _nav.updateRotation = true;
            _wasTargetFalse = false;
        }
    }

    void PerformSurroundRaycast(int numRays)
    {
        // Cast multiple rays in a circle to simulate vision
        float angleStep = 360f / numRays;
        bool hitPlayer = false;

        for (int i = 0; i < numRays; i++)
        {
            float angle = i * angleStep;
            Vector3 direction = Quaternion.Euler(0, angle, 0) * transform.forward;

            if (Physics.Raycast(transform.position + Vector3.up, direction, out RaycastHit hit, 10f))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    _player = hit.collider.transform.position;
                    hitPlayer = true; // Player detected
                }
            }
        }

        if (hitPlayer)
        {
            _target = true;
            _loseTimer = 0f; // Reset timer since player is visible
        }
        else if (_target)
        {
            // Accumulate time to lose target if not seen
            _loseTimer += Time.deltaTime;
            if (_loseTimer >= _loseTargetDelay) _target = false;
        }
    }

    public void RandDes()
    {
        // Patrol to random waypoint when idle
        if (!_target && _nav.velocity.magnitude == 0)
        {
            _nav.SetDestination(_cubes[Random.Range(0, _cubes.Length)].position);
            _nav.speed = 2f;
        }
        // Resume chasing if stopped but target exists
        else if (_target && _nav.velocity.magnitude == 0)
        {
            _nav.speed = 4f;
        }
    }

    void HandleAnimations()
    {
        // Set animator parameters based on navmesh movement and target status
        _ani.SetFloat("Speed", _nav.velocity.magnitude);
        _ani.SetBool("Attack", _target && _nav.remainingDistance <= _nav.stoppingDistance);
        _ani.SetBool("Target", _target);
    }
}

