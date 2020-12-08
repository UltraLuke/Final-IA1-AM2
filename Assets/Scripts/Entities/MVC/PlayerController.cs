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
    private List<Node> waypoints;
    Vector3 _finalPos;
    int _nextPoint = 0;
    bool _readyToMove;
    bool _lastConnection;
    ISteering _sb;

    [Header("Obstacle avoidance")]
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
        if (_readyToMove)
            Run();
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
        _nextPoint = 0;
        if (newPoints.Count == 0) return;
        waypoints = newPoints;
        var pos = waypoints[_nextPoint].transform.position;
        pos.y = transform.position.y;

        _sb = new ObstacleAvoidance(transform, waypoints[_nextPoint].transform, obstacleDistance, avoidWeight, _obstacleLayer);

        _finalPos = finalPos;
        _lastConnection = false;
        _readyToMove = true;
    }
    public void Run()
    {
        var point = waypoints[_nextPoint];
        var posPoint = point.transform.position;
        posPoint.y = transform.position.y;

        Vector3 dir;
        if (!_lastConnection)
            dir = posPoint - transform.position;
        else
            dir = _finalPos - transform.position;

        if (dir.magnitude < 0.2f)
        {
            if (!_lastConnection)
            {
                if (_nextPoint + 1 < waypoints.Count)
                    _nextPoint++;

                if (_nextPoint + 1 >= waypoints.Count)
                    _lastConnection = true;
            }
            _sb = new ObstacleAvoidance(transform, waypoints[_nextPoint].transform, obstacleDistance, avoidWeight, _obstacleLayer);
        }

        _model.Move(dir.normalized);
    }
    #endregion
}
