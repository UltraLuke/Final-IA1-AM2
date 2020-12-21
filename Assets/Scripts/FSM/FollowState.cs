using UnityEngine;

public class FollowState<T> : States<T>
{
    public delegate void MoveRun();
    MoveRun _moveRun;
    private Model _model;
    private MinionState _state;
    private float _critHealthAmount;
    private int _team;
    private INode _node;

    public FollowState(MoveRun mR, Model model, MinionState state, float criticalHealthAmount, int team, INode node)
    {
        _moveRun += mR;
        _model = model;
        _state = state;
        _critHealthAmount = criticalHealthAmount;
        _team = team;
        _node = node;
    }

    public override void Execute()
    {
        //MOVIMIENTO POR FLOCKING
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
        }
    }

    public override void Exit()
    {
        _model.Move(Vector3.zero);
    }
}
