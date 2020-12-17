using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour
{
    public virtual void Move(Vector3 dir){}
    public virtual bool IsInSight(Transform target){ return false; }
}
