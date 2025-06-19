using UnityEngine;
using UnityEngine.AI;

public class Project_1_Enemy : MonoBehaviour
{
    [SerializeField] NavMeshAgent _nav;
    [SerializeField] Transform _player;
    void Start()
    {
        _player = GameObject.Find("Player").transform;
        _nav = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        _nav.SetDestination(_player.position);
    }
}
