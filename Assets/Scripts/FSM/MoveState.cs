using System.Collections.Generic;
using UnityEngine;

public class MoveState<T> : States<T>
{
    private LeaderController _iAController;
    private Model _model;
    private LeaderState _state;
    private AgentTheta _theta;
    private List<Node> _wpNodes;
    private Transform _goal;
    private float _critHealthAmount;
    private int _team;
    private float _distToGoal;
    private INode _node;

    public MoveState(LeaderController iaController, Model model, LeaderState state, AgentTheta theta, Transform goal, float critHealthAmount, int team, float distToGoal, INode node)
    {
        _iAController = iaController;
        _model = model;
        _state = state;
        _theta = theta;
        _goal = goal;
        _critHealthAmount = critHealthAmount;
        _team = team;
        _distToGoal = distToGoal;
        _node = node;
    }
    public override void Awake()
    {
        _state.onDominatingZone = false;
        _state.enemyOnSight = false;
        _state.lowHealth = false;

        _wpNodes = _theta.GetPathFinding(_model.transform.position, _goal.position);
        _iAController.SetWayPoints(_wpNodes, _goal.position);
    }
    public override void Execute()
    {
        //bool blocked = true;

        //MOVIMIENTO POR PATHFINDING
        _iAController.Run();

        //CHEQUEO SI TENGO VIDA BAJA
        if(_model.GetHealth() <= _critHealthAmount)
        {
            _state.lowHealth = true;
            //Cambio de estado
        }

        //CHEQUEO SI VEO UN ENEMIGO
        if (_model.CheckAndGetClosestEnemyInSight(_team, out _state.closestEnemyOnSight))
        {
            _state.enemyOnSight = true;
            //Cambio de estado
        }

        //CHEQUEO SI ESTOY EN EL OBJETIVO
        var dir = _goal.position - _model.transform.position;
        if (dir.magnitude < _distToGoal)
        {
            _state.onDominatingZone = true;
            //Cambio de estado
        }
    }
}


