using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour, ITeam
{
    [SerializeField] int team;

    public virtual void SetWayPoints(List<Node> newPoints, Vector3 finalPos){}
    public int GetTeamNumber() => team;
}
