using System.Collections.Generic;
using UnityEngine;

public class FleeState<T> : States<T>
{
    public delegate void MoveSetup(Transform target);
    public delegate void MoveRun();
    MoveSetup _moveSetup;
    MoveRun _moveRun;
    private Model _model;
    private DataState _state;
    private Transform _secureZone;
    private float _minHealthTreshold;
    private float _distToSecureZone;
    private INode _node;

    public FleeState(MoveSetup mS, MoveRun mR, Model model, DataState state, Transform secureZone, float minHealthTreshold, float distToSecureZone, INode node)
    {
        _moveSetup += mS;
        _moveRun += mR;
        _model = model;
        _state = state;
        _secureZone = secureZone;
        _minHealthTreshold = minHealthTreshold;
        _distToSecureZone = distToSecureZone;
        _node = node;
    }

    public override void Awake()
    {
        Debug.Log("FleeState");

        _moveSetup(_secureZone);
    }

    public override void Execute()
    {
        //MOVIMIENTO
        var dir = _secureZone.position - _model.transform.position;
        if (dir.magnitude >= _distToSecureZone)
            _moveRun();
        else
            _model.Move(Vector3.zero);

        //CHEQUEO SI RECUPERE SUFICIENTE VIDA
        if (_model.GetHealth() >= _minHealthTreshold)
        {
            _state.lowHealth = false;
            _node.Execute();
        }
    }

    public override void Exit()
    {
        _model.Move(Vector3.zero);
        _state.lowHealth = false;
    }
}


