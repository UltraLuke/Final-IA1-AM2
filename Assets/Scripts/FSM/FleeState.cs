using System.Collections.Generic;
using UnityEngine;

public class FleeState<T> : States<T>
{
    private IAController _iaController;
    private LeaderModel _leader;
    private LeaderState _state;
    private AgentTheta _theta;
    private Transform _secureZone;
    private float _minHealthTreshold;
    private INode _node;
    private List<Node> _wpNodes;

    public FleeState(IAController iaController, LeaderModel leader, LeaderState state, AgentTheta theta, Transform secureZone, float minHealthTreshold, INode node)
    {
        _iaController = iaController;
        _leader = leader;
        _state = state;
        _theta = theta;
        _secureZone = secureZone;
        _minHealthTreshold = minHealthTreshold;
        _node = node;
    }

    public override void Awake()
    {
        _state.enemyOnSight = false;
        _state.onDominatingZone = false;
        _wpNodes = _theta.GetPathFinding(_leader.transform.position, _secureZone.position);
        _iaController.SetWayPoints(_wpNodes, _secureZone.position);
    }

    public override void Execute()
    {
        //MOVIMIENTO POR PATHFINDING
        _iaController.Run();

        //CHEQUEO SI RECUPERE SUFICIENTE VIDA
        if(_leader.GetHealth() >= _minHealthTreshold)
        {
            _state.lowHealth = false;
            //Cambio de estado
        }
    }
}


