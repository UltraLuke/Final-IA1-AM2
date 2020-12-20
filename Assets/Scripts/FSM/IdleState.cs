using UnityEngine;

public class IdleState<T> : States<T>
{
    private Model _model;
    private LeaderState _state;
    private Transform _goal;
    private float _distToGoal;
    private float _critHealthAmount;
    private int _team;
    private INode _node;

    public IdleState(Model model, LeaderState state, Transform goal, float distToGoal, float critHealthAmount, int team, INode node)
    {
        _model = model;
        _state = state;
        _goal = goal;
        _distToGoal = distToGoal;
        _critHealthAmount = critHealthAmount;
        _team = team;
        _node = node;
    }

    public override void Awake()
    {
        Debug.Log("IdleState");
    }
    public override void Execute()
    {
        //CHEQUEO SI TENGO VIDA BAJA
        if (_model.GetHealth() <= _critHealthAmount)
        {
            _state.lowHealth = true;
            _node.Execute();
            return;
        }

        //CHEQUEO SI VEO UN ENEMIGO
        if (_model.CheckAndGetClosestEnemyInSight(_team, out _state.closestEnemyOnSight))
        {
            _state.enemyOnSight = true;
            _node.Execute();
            return;
        }

        //CHEQUEO SI ME ENCUENTRO AFUERA DEL PUNTO
        var dir = _goal.position - _model.transform.position;
        if (dir.magnitude >= _distToGoal)
        {
            _state.onDominatingZone = false;
            _node.Execute();
        }
    }

    public override void Exit()
    {
        _state.onDominatingZone = false;
    }
}


