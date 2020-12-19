using UnityEngine;

public class AttackState<T> : States<T>
{
    private LeaderModel _leader;
    private LeaderState _state;
    private Transform _target;
    private float _shootCD;
    private int _teamNumber;
    private float _critHealthAmount;
    private INode _node;

    private float _currentShootCD;

    public AttackState(LeaderModel leader, LeaderState state, Transform target, float shootCD, int teamNumber, float critHealthAmount, INode node)
    {
        _leader = leader;
        _state = state;
        _target = target;
        _shootCD = shootCD;
        _teamNumber = teamNumber;
        _node = node;
    }
    public override void Awake()
    {
        _state.lowHealth = false;
        _state.onDominatingZone = false;
    }
    public override void Execute()
    {
        //CHEQUEO SI TENGO POCA VIDA
        if (_leader.GetHealth() <= _critHealthAmount)
        {
            _state.lowHealth = true;
            //Cambio de estado
        }

        //SI TENGO UN ENEMIGO A LA VISTA, LO ATACO
        //SINO, PASO AL SIGUIENTE ESTADO
        if(_target != null)
        {
            if (_currentShootCD <= 0)
                Shoot();
            else
                _currentShootCD -= Time.deltaTime;
        }
        else
        {
            _state.enemyOnSight = false;
            //Cambio de estado
        }
    }

    private void Shoot()
    {
        var dir = (_target.position - _leader.transform.position).normalized;
        dir = new Vector3(dir.x, _leader.transform.position.y, dir.z);
        _leader.LookAtDir(dir);
        _leader.Shoot(_target, _teamNumber);

        _currentShootCD = _shootCD;
    }
}