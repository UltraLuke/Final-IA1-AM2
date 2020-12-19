using System.Collections.Generic;
using UnityEngine;

public class MoveState<T> : States<T>
{
    private IAController _iAController;
    private LeaderModel _leader;
    private LeaderState _state;
    private AgentTheta _theta;
    private List<Node> _wpNodes;
    private Transform _goal;
    private float _critHealthAmount;
    private float _distToGoal;
    private INode _node;

    public MoveState(IAController iaController, LeaderModel leader, LeaderState state, AgentTheta theta, Transform goal, float critHealthAmount, float distToGoal, INode node)
    {
        _iAController = iaController;
        _leader = leader;
        _state = state;
        _theta = theta;
        _goal = goal;
        _critHealthAmount = critHealthAmount;
        _distToGoal = distToGoal;
        _node = node;
    }
    public override void Awake()
    {
        _state.onDominatingZone = false;
        _state.enemyOnSight = false;
        _state.lowHealth = false;

        _wpNodes = _theta.GetPathFinding(_leader.transform.position, _goal.position);
        _iAController.SetWayPoints(_wpNodes, _goal.position);
    }
    public override void Execute()
    {
        bool blocked = true;

        //MOVIMIENTO POR PATHFINDING
        _iAController.Run();

        //CHEQUEO SI TENGO VIDA BAJA
        if(_leader.GetHealth() <= _critHealthAmount)
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

        //CHEQUEO SI ESTOY EN EL OBJETIVO
        var dir = _goal.position - _leader.transform.position;
        if (dir.magnitude < _distToGoal)
        {
            _state.onDominatingZone = true;
            //Cambio de estado
        }
    }
}


