using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionController : Controller
{
    [Header("Pathfinding")]
    [Tooltip("Distancia requerida para que la entidad se dirija al siguiente waypoint.")] public float nextWaypointDistance;
    public float obstacleDistance;
    public float avoidWeight;
    public LayerMask avoidLayer;
    [Header("Health Behaviour")]
    public Transform secureZone;
    [Tooltip("Umbral de vida crítica. Si el la vida es menor que el umbral, el ente corre a un lugar seguro.")]
    public float criticalHealthAmount;
    [Tooltip("Umbral de vida segura. Si el la vida es mayor al umbral, el ente sale de la zona segura.")]
    public float secureHealthAmount;
    [Tooltip("Si la distancia entre el ente y la zona segura es menor al parámetro, el mismo considera que está en la zona.")]
    public float distToSecureZone;
    [Header("Shoot")]
    [Tooltip("Cooldown de disparo, o Rate of Fire")] public float shootCD;

    private Model _model;
    private MinionState _minionState;
    private AgentTheta _agentTheta;

    enum States { Follow, Attack, Flee }
    FSM<States> _fsm;
    INode _root;

    void FSMInit()
    {
        _fsm = new FSM<States>();
        var follow = new FollowState<States>(FlockingRun, _model, _minionState, criticalHealthAmount, team, _root);
        var attack = new AttackState<States>(_model, _minionState, shootCD, team, criticalHealthAmount, _root);
        var flee = new FleeState<States>(SetupPathfinding, Run, _model, _minionState, secureZone, secureHealthAmount, distToSecureZone, _root);

        follow.AddTransitionState(States.Attack, attack);
        follow.AddTransitionState(States.Flee, flee);
        attack.AddTransitionState(States.Follow, follow);
        attack.AddTransitionState(States.Flee, flee);
        flee.AddTransitionState(States.Follow, follow);

        _fsm.SetInit(follow);
    }
    void TreeInit()
    {
        var follow = new ActionNode(() => _fsm.Transition(States.Follow));
        var attack = new ActionNode(() => _fsm.Transition(States.Attack));
        var flee = new ActionNode(() => _fsm.Transition(States.Flee));

        var questionEnemyOnSight = new QuestionNode(() => _minionState.enemyOnSight, attack, follow);
        var questionLowHealth = new QuestionNode(() => _minionState.lowHealth, flee, questionEnemyOnSight);

        _root = questionLowHealth;
    }
    private void Awake()
    {
        _model = GetComponent<Model>();
        _minionState = GetComponent<MinionState>();
        _agentTheta = GetComponent<AgentTheta>();
        _flock = GetComponent<FlockEntity>();
        _model = GetComponent<Model>();
    }
    private void Start()
    {
        TreeInit();
        FSMInit();
    }
    private void Update()
    {
        _fsm.OnUpdate();
    }

    #region Pathfinding Move
    private List<Node> _waypoints;
    private Vector3 _finalPos;
    private int _nextPoint;
    private ObstacleAvoidance _sb;
    private bool _lastConnection;

    public void SetupPathfinding(Transform target)
    {
        var wpNodes = _agentTheta.GetPathFinding(_model.transform.position, target.position);
        SetWayPoints(wpNodes, target.position);
    }
    public override void SetWayPoints(List<Node> newPoints, Vector3 finalPos)
    {
        if (newPoints.Count == 0) return;

        _waypoints = newPoints;
        _finalPos = finalPos;
        _nextPoint = 0;
        var pos = _waypoints[_nextPoint].transform.position;
        pos.y = transform.position.y;

        _sb = new ObstacleAvoidance(transform, _waypoints[_nextPoint].transform, obstacleDistance, avoidWeight, avoidLayer);

        _lastConnection = false;
    }
    public override void Run()
    {
        var point = _waypoints[_nextPoint];
        var posPoint = point.transform.position;
        posPoint.y = transform.position.y;

        Vector3 dir;
        if (!_lastConnection)
            dir = posPoint - transform.position;
        else
            dir = _finalPos - transform.position;

        if (dir.magnitude < nextWaypointDistance)
        {
            if (!_lastConnection)
            {
                if (_nextPoint + 1 < _waypoints.Count)
                {
                    _nextPoint++;
                    _sb = new ObstacleAvoidance(transform, _waypoints[_nextPoint].transform, obstacleDistance, avoidWeight, avoidLayer);
                }

                else if (_nextPoint + 1 >= _waypoints.Count)
                {
                    _lastConnection = true;
                    _sb = new ObstacleAvoidance(transform, _finalPos, obstacleDistance, avoidWeight, avoidLayer);
                }
            }
        }
        _model.Move(dir.normalized + _sb.GetDir());
    }
    #endregion

    #region Flocking Move
    Vector3 _dir = Vector3.zero;
    private FlockEntity _flock;
    public void FlockingRun()
    {
        _dir = _flock.GetDir();
        _model.Move(_dir);
    }

    #endregion
}
