using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAController : Controller
{
    Model _model;

    [Header("Pathfinding")]
    [Tooltip("Distancia requerida para que la entidad se dirija al siguiente waypoint")]
    public float nextWaypointDistance;

    [Header("Obstacle avoidance")]
    public float obstacleDistance;
    public float avoidWeight;
    public LayerMask _avoidLayer;

    #region Pathfinding Move
    private List<Node> _waypoints;
    private Vector3 _finalPos;
    private int _nextPoint;
    private ObstacleAvoidance _sb;
    private bool _lastConnection;
    private bool _readyToMove;

    public override void SetWayPoints(List<Node> newPoints, Vector3 finalPos)
    {
        if (newPoints.Count == 0) return;

        _waypoints = newPoints;
        _finalPos = finalPos;
        _nextPoint = 0;
        var pos = _waypoints[_nextPoint].transform.position;
        pos.y = transform.position.y;

        _sb = new ObstacleAvoidance(transform, _waypoints[_nextPoint].transform, obstacleDistance, avoidWeight, _avoidLayer);

        _lastConnection = false;
        _readyToMove = true;
    }

    public void Run()
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
                    _sb = new ObstacleAvoidance(transform, _waypoints[_nextPoint].transform, obstacleDistance, avoidWeight, _avoidLayer);
                }

                else if (_nextPoint + 1 >= _waypoints.Count)
                {
                    _lastConnection = true;
                    _sb = new ObstacleAvoidance(transform, _finalPos, obstacleDistance, avoidWeight, _avoidLayer);
                }
            }
        }

        _model.Move(dir.normalized + _sb.GetDir());
    }
    #endregion
}
