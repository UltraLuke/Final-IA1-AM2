using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
    [Header("Basic Settings")]
    [SerializeField] bool useMouse;
    Model _model;

    //Mouse fields
    [Header("Point and Click settings")]
    [SerializeField] LayerMask _obstacleLayer;
    [SerializeField] float nextWaypointDistance = 0.2f;
    private List<Node> waypoints;
    Vector3 _finalPos;
    int _nextPoint = 0;
    bool _readyToMove;
    bool _lastConnection;
    ISteering _sb;

    [Header("Obstacle avoidance")]
    [SerializeField] LayerMask _avoidLayer;
    [SerializeField] float obstacleDistance;
    [SerializeField] float avoidWeight;

    private void Awake()
    {
        _model = GetComponent<Model>();
    }
    private void Update()
    {
        if (!useMouse)
            KeyboardUpdate();
        else
            MouseUpdate();
    }

    private void MouseUpdate()
    {
        if (_readyToMove) Run();
    }

    private void KeyboardUpdate()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        _model.Move(dir);
    }

    #region Mouse methods
    public override void SetWayPoints(List<Node> newPoints, Vector3 finalPos)
    {
        if (newPoints.Count == 0) return;

        waypoints = newPoints;
        _finalPos = finalPos;
        _nextPoint = 0;
        var pos = waypoints[_nextPoint].transform.position;
        pos.y = transform.position.y;

        _sb = new ObstacleAvoidance(transform, waypoints[_nextPoint].transform, obstacleDistance, avoidWeight, _avoidLayer);

        _lastConnection = false;
        _readyToMove = true;
    }
    public override void Run()
    {
        var point = waypoints[_nextPoint];
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
                if (_nextPoint + 1 < waypoints.Count)
                {
                    _nextPoint++;
                    _sb = new ObstacleAvoidance(transform, waypoints[_nextPoint].transform, obstacleDistance, avoidWeight, _avoidLayer);
                }

                else if (_nextPoint + 1 >= waypoints.Count)
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
