using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvoidanceBehavior : MonoBehaviour, IFlockBehavior
{
    public float avoidanceWeight;
    public LayerMask mask;
    public float range;
    public Vector3 GetDir(List<IFlockEntity> entities, IFlockEntity entity)
    {
        var obj = Physics.OverlapSphere(entity.Position, range, mask);
        Vector3 dir = Vector3.zero;
        for (int i = 0; i < obj.Length; i++)
        {
            var currEntity = obj[i];
            Vector3 currDir = (entity.Position - currEntity.transform.position);
            float distance = currDir.magnitude;
            if (distance > range)
            {
                distance = range - 0.1f;
            }
            currDir = currDir.normalized * (range - distance);
            dir += currDir;
        }
        return dir * avoidanceWeight;
    }
}
