using Unity.VisualScripting;
using UnityEngine;

public abstract class State
{
    protected StateMachine _sMachine;

    public State(StateMachine sMachine)
    {
        this._sMachine = sMachine;
    }

    public virtual void Enter() { }
    public virtual void Tick() { }
    public virtual void Exit() { }
} 

public class StateMachine : MonoBehaviour
{
    private State _currentState;

    public void SetState(State _newState)
    {
        if(_currentState != null)
        {
            _currentState.Exit();
        }

        _currentState = _newState;

        if( _currentState != null )
        {
            _currentState.Enter();
        }
    }
    public State CurrentS()
    {
        return _currentState;
    }
    public void Update()
    {
        if( _currentState != null )
        {
            _currentState.Tick();
        }
    }
}
