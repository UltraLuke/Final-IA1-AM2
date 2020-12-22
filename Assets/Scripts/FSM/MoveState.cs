using System.Collections.Generic;
using UnityEngine;

public class MoveState<T> : States<T>
{
    public delegate void MoveSetup(Transform target);
    public delegate void MoveRun();
    MoveSetup _moveSetup;
    MoveRun _moveRun;
    private Model _model;
    private LeaderState _state;
    private Transform _goal;
    private float _critHealthAmount;
    private int _team;
    private float _distToGoal;
    private INode _node;

    public MoveState(MoveSetup mS, MoveRun mR, Model model, LeaderState state, Transform goal, float critHealthAmount, int team, float distToGoal, INode node)
    {
        _moveSetup += mS;
        _moveRun += mR;
        _model = model;
        _state = state;
        _goal = goal;
        _critHealthAmount = critHealthAmount;
        _team = team;
        _distToGoal = distToGoal;
        _node = node;
    }
    public override void Awake()
    {
        _moveSetup(_goal);
    }
    public override void Execute()
    {
        //MOVIMIENTO
        var dir = _goal.position - _model.transform.position;
        if (dir.magnitude >= _distToGoal)
            _moveRun();

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

        //CHEQUEO SI ESTOY EN EL OBJETIVO
        if (dir.magnitude < _distToGoal)
        {
            _state.onDominatingZone = true;
            _node.Execute();
        }
    }

    public override void Exit()
    {
        _model.Move(Vector3.zero);
    }
}


