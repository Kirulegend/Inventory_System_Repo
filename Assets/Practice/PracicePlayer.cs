using UnityEditor.PackageManager;
using UnityEditor.UIElements;
using UnityEngine;

public class PracicePlayer : MonoBehaviour
{
    enum PlayerState
    {
        idle = 1, walking = 2, running = 3, jump = 4
    }
    PlayerState _state;
    public delegate void Health(int Health);
    public static event Health HealthE;

    int _health = 100;

    public void TakeDamage(int Damage)
    {
        _health -= Damage;
        HealthE?.Invoke(_health);
    }
    void StateControl()
    {
        switch (_state)
        {
            case PlayerState.idle:
                if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
                {
                    _state = PlayerState.walking;
                }
                if(Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 && Input.GetKey(KeyCode.LeftShift))
                {
                    _state = PlayerState.running;
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _state = PlayerState.jump;
                }
                break;
            case PlayerState.walking:
                if (Input.GetAxisRaw("Horizontal") == 0 || Input.GetAxisRaw("Vertical") == 0)
                {
                    _state = PlayerState.idle;
                }
                if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0 && Input.GetKey(KeyCode.LeftShift))
                {
                    _state = PlayerState.running;
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _state = PlayerState.jump;
                }
                //Movement
                break;
            case PlayerState.running:
                if (Input.GetAxisRaw("Horizontal") == 0 || Input.GetAxisRaw("Vertical") == 0)
                {
                    _state = PlayerState.idle;
                }
                if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
                {
                    _state = PlayerState.walking;
                }
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _state = PlayerState.jump;
                }
                //Movement * speed
                break;
            case PlayerState.jump:
                //Jump
                break;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(1);
        }
    }
}
