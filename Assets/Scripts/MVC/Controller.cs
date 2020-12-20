using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour, ITeam
{
    [SerializeField] protected int team;

    public virtual void SetWayPoints(List<Node> newPoints, Vector3 finalPos){}
    public virtual void Run() { }
    public int GetTeamNumber() => team;
}
