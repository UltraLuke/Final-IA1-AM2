using System.Collections.Generic;
using UnityEngine;

public class FleeState<T> : States<T>
{
    private LeaderController _iaController;
    private Model _model;
    private LeaderState _state;
    private AgentTheta _theta;
    private Transform _secureZone;
    private float _minHealthTreshold;
    private float _distToSecureZone;
    private INode _node;

    private List<Node> _wpNodes;

    public FleeState(LeaderController iaController, Model model, LeaderState state, AgentTheta theta, Transform secureZone, float minHealthTreshold, float distToSecureZone, INode node)
    {
        _iaController = iaController;
        _model = model;
        _state = state;
        _theta = theta;
        _secureZone = secureZone;
        _minHealthTreshold = minHealthTreshold;
        _distToSecureZone = distToSecureZone;
        _node = node;
    }

    public override void Awake()
    {
        _state.enemyOnSight = false;
        _state.onDominatingZone = false;
        _wpNodes = _theta.GetPathFinding(_model.transform.position, _secureZone.position);
        _iaController.SetWayPoints(_wpNodes, _secureZone.position);
    }

    public override void Execute()
    {
        //MOVIMIENTO POR PATHFINDING
        var dir = _secureZone.position - _model.transform.position;
        if (dir.magnitude <= _distToSecureZone)
            _iaController.Run();

        //CHEQUEO SI RECUPERE SUFICIENTE VIDA
        if(_model.GetHealth() >= _minHealthTreshold)
        {
            _state.lowHealth = false;
            //Cambio de estado
        }
    }
}


