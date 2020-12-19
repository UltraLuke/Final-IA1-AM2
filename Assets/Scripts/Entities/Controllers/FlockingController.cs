using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingController : Controller
{
    Vector3 _dir = Vector3.zero;
    private FlockEntity _flock;
    private Model _model;

    private void Awake()
    {
        _flock = GetComponent<FlockEntity>();
        _model = GetComponent<Model>();
    }
    void Update()
    {
        _dir = _flock.GetDir();
        _model.Move(_dir);
    }
}
