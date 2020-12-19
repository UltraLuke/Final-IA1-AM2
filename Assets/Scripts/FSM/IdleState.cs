using UnityEngine;

public class IdleState<T> : States<T>
{
    private LeaderModel _leader;
    private LeaderState _state;
    private Transform _goal;
    private float _distToGoal;
    private float _critHealthAmount;

    public IdleState(LeaderModel leader, LeaderState state, Transform goal, float distToGoal, float critHealthAmount)
    {
        _leader = leader;
        _state = state;
        _goal = goal;
        _distToGoal = distToGoal;
        _critHealthAmount = critHealthAmount;
    }

    public override void Awake()
    {
        _state.enemyOnSight = false;
        _state.lowHealth = false;
    }
    public override void Execute()
    {
        bool blocked = true;

        //CHEQUEO SI TENGO VIDA BAJA
        if (_leader.GetHealth() <= _critHealthAmount)
        {
            _state.lowHealth = true;
            //Cambio de estado
        }

        //CHEQUEO SI VEO UN ENEMIGO
        if (!blocked)
        {
            _state.enemyOnSight = true;
            //Cambio de estado
        }

        //CHEQUEO SI ME ENCUENTRO AFUERA DEL PUNTO
        var dir = _goal.position - _leader.transform.position;
        if (dir.magnitude >= _distToGoal)
        {
            _state.onDominatingZone = false;
            //Cambio de estado
        }
    }
}


