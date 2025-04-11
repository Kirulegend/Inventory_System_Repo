using UnityEngine;

public class IdleState : State
{
    Transform _player;

    public IdleState(StateMachine _stateMachine, Transform _playerTransform) : base(_stateMachine)
    {
        _player = _playerTransform;
    }

    public override void Enter()
    {
        Debug.Log($"Entered Idle State");
    }
    public override void Tick()
    {
        if(Input.GetKeyDown(KeyCode.W))
        {
            _sMachine.SetState(new WalkState(_sMachine, _player));
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            _sMachine.SetState(new JumpState(_sMachine, _player));
        }
    }
    public override void Exit()
    {
        Debug.Log($"Exited Idle State");
    }
}

public class WalkState : State
{
    Transform _player;
    float _speed;

    public WalkState(StateMachine _stateMachine, Transform _playerTransform) : base(_stateMachine)
    {
        _player = _playerTransform;
    }

    public override void Enter()
    {
        Debug.Log($"Entered Walk State");
    }
    public override void Tick()
    {
        Vector3 Direction = Vector3.forward;

        _player.Translate(Direction * _speed * Time.deltaTime);
        if(Input.GetKeyUp(KeyCode.W))
        {
            _sMachine.SetState(new IdleState(_sMachine, _player));
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _sMachine.SetState(new JumpState(_sMachine, _player));
        }
    }
    public override void Exit()
    {
        Debug.Log($"Exited Walk State");
    }
}

public class JumpState : State
{
    Transform _player;
    float _jumpForce;

    public JumpState(StateMachine _stateMachine, Transform _playerTransform) : base(_stateMachine)
    {
        _player = _playerTransform;
    }

    public override void Enter()
    {
        Debug.Log($"Entered Walk State");
    }
    public override void Tick()
    {
        Vector3 Direction = Vector3.up;
        _player.Translate(Direction * _jumpForce * Time.deltaTime);
        if(Input.GetKeyUp(KeyCode.Space))
        {
            _sMachine.SetState(new IdleState( _sMachine, _player));
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            _sMachine.SetState(new WalkState(_sMachine, _player));
        }
    }
    public override void Exit()
    {
        Debug.Log($"Exited Walk State");
    }
}
