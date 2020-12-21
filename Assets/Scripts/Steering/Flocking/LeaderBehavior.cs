using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderBehavior : MonoBehaviour, IFlockBehavior
{
    public float leaderWeight;
    public Transform target;
    public float minDistance;
    public Transform trWhenLeaderDead;

    public Vector3 GetDir(List<IFlockEntity> entities, IFlockEntity entity)
    {
        Vector3 position;
        if (target != null)
            position = target.position;
        else
            position = trWhenLeaderDead.position;

        Vector3 dir = position - entity.Position;
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
