using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBehavior : MonoBehaviour, IFlockBehavior
{
    public float leaderWeight;
    public Transform target;
    public float minDistance;
    public Vector3 GetDir(List<IFlockEntity> entities, IFlockEntity entity)
    {
        Vector3 dir = target.position - entity.Position;
        float distance = dir.magnitude;
        if (distance < minDistance)
        {
            distance -= minDistance;
            return dir.normalized * leaderWeight * distance;
        }
        else
            return dir.normalized * leaderWeight;
    }

}
