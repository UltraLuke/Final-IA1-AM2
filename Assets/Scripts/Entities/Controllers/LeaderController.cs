using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderController : Controller
{
    [Header("Pathfinding")]
    [Tooltip("Distancia requerida para que la entidad se dirija al siguiente waypoint.")] public float nextWaypointDistance;
    [Header("Obstacle avoidance")]
    public float obstacleDistance;
    public float avoidWeight;
    public LayerMask avoidLayer;
    [Header("Goal")]
    [Tooltip("El transform de la meta.")]
    public Transform goal;
    [Tooltip("Si la distancia entre el ente y el goal es menor al parámetro, el mismo considera que está en la meta.")]
    public float distToGoal;
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
    private LeaderState _leaderState;
    private AgentTheta _agentTheta;

    enum States { Move, Attack, Flee, Idle }
    FSM<States> _fsm;
    INode _root;

    void FSMInit()
    {
        _fsm = new FSM<States>();
        var move = new MoveState<States>(SetupPathfinding, Run, _model, _leaderState, goal, criticalHealthAmount, team, distToGoal, _root);
        var attack = new AttackState<States>(_model, _leaderState, shootCD, team, criticalHealthAmount, _root);
        var flee = new FleeState<States>(SetupPathfinding, Run, _model, _leaderState, secureZone, secureHealthAmount, distToSecureZone, _root);
        var idle = new IdleState<States>(_model, _leaderState, goal, distToGoal, criticalHealthAmount, team, _root);

        move.AddTransitionState(States.Idle, idle);
        move.AddTransitionState(States.Attack, attack);
        move.AddTransitionState(States.Flee, flee);
        attack.AddTransitionState(States.Move, move);
        attack.AddTransitionState(States.Flee, flee);
        flee.AddTransitionState(States.Move, move);
        idle.AddTransitionState(States.Move, move);
        idle.AddTransitionState(States.Flee, flee);
        idle.AddTransitionState(States.Attack, attack);

        _fsm.SetInit(move);
    }
    void TreeInit()
    {
        var move = new ActionNode(() => _fsm.Transition(States.Move));
        var attack = new ActionNode(() => _fsm.Transition(States.Attack));
        var flee = new ActionNode(() => _fsm.Transition(States.Flee));
        var idle = new ActionNode(() => _fsm.Transition(States.Idle));

        var questionOnGoalZone = new QuestionNode(() => _leaderState.onDominatingZone, idle, move);
        var questionEnemyOnSight = new QuestionNode(() => _leaderState.enemyOnSight, attack, questionOnGoalZone);
        var questionLowHealth = new QuestionNode(() => _leaderState.lowHealth, flee, questionEnemyOnSight);

        _root = questionLowHealth;
    }

    private void Awake()
    {
        _model = GetComponent<Model>();
        _leaderState = GetComponent<LeaderState>();
        _agentTheta = GetComponent<AgentTheta>();
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
    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (team == 0) EventsHandler.TriggerEvent("EVENT_TEAM2WINS");
        else if (team == 1) EventsHandler.TriggerEvent("EVENT_TEAM1WINS");
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
}
