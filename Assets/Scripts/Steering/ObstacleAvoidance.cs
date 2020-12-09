using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : ISteering
{
    Transform _from;
    float _radius;
    LayerMask _mask;
    Transform _target;
    float _avoidWeight;

    Vector3 _toPos;
    bool vectorInsteadTransform;

    public ObstacleAvoidance(Transform from, Transform target, float radius, float avoidWeight, LayerMask mask)
    {
        _avoidWeight = avoidWeight;
        _target = target;
        _radius = radius;
        _mask = mask;
        _from = from;

        vectorInsteadTransform = false;
    }
    public ObstacleAvoidance(Transform from, Vector3 to, float radius, float avoidWeight, LayerMask mask)
    {
        _from = from;
        _toPos = to;
        _radius = radius;
        _avoidWeight = avoidWeight;
        _mask = mask;

        vectorInsteadTransform = true;
    }
    public Vector3 GetDir()
    {
        Vector3 target;
        if (vectorInsteadTransform)
            target = _toPos;
        else
            target = _target.position;

        Vector3 dir = (target - _from.position).normalized;
        
        Collider[] obstacles = Physics.OverlapSphere(_from.position, _radius, _mask);
        if (obstacles.Length > 0)
        {
            float distance = Vector3.Distance(obstacles[0].transform.position, _from.position);
            int indexSave = 0;
            for (int i = 1; i < obstacles.Length; i++)
            {
                float currDistance = Vector3.Distance(obstacles[i].transform.position, _from.position);
                if (currDistance < distance)
                {
                    distance = currDistance;
                    indexSave = i;
                }
            }
            Vector3 dirFromObs = (_from.position - obstacles[indexSave].transform.position).normalized * ((_radius - distance) / _radius) * _avoidWeight;
            dir += dirFromObs;
        }
        return dir.normalized;
    }
}
