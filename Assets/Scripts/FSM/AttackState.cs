using UnityEngine;

public class AttackState<T> : States<T>
{
    private Model _model;
    private DataState _state;
    private Transform _target;
    private float _shootCD;
    private int _teamNumber;
    private float _critHealthAmount;
    private INode _node;

    private float _currentShootCD;

    public AttackState(Model model, DataState state, float shootCD, int teamNumber, float critHealthAmount, INode node)
    {
        _model = model;
        _state = state;
        _shootCD = shootCD;
        _teamNumber = teamNumber;
        _critHealthAmount = critHealthAmount;
        _node = node;
    }
    public override void Awake()
    {
        Debug.Log("AttackState");
        _target = _state.closestEnemyOnSight;
    }
    public override void Execute()
    {
        //CHEQUEO SI TENGO POCA VIDA
        if (_model.GetHealth() <= _critHealthAmount)
        {
            _state.lowHealth = true;
            _node.Execute();
            return;
        }

        //SI TENGO UN ENEMIGO A LA VISTA, LO ATACO
        //SINO, PASO AL SIGUIENTE ESTADO
        if (_target != null && _model.IsInSight(_target))
        {
            if (_currentShootCD <= 0)
                Shoot();
            else
                _currentShootCD -= Time.deltaTime;
        }
        else
        {
            _state.enemyOnSight = false;
            _node.Execute();
        }
    }
    public override void Exit()
    {
        _state.enemyOnSight = false;
    }
    private void Shoot()
    {
        var dir = (_target.position - _model.transform.position).normalized;
        dir.y = 0;
        _model.LookAtDir(dir);
        _model.Shoot(_target, _teamNumber);

        _currentShootCD = _shootCD;
    }
}