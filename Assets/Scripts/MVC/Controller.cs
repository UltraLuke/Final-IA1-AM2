using System;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour, ITeam
{
    public Action<Controller> deathNotif;
    [SerializeField] protected bool _updateEnabled = false;
    [SerializeField] protected int team;

    public virtual void SetWayPoints(List<Node> newPoints, Vector3 finalPos){}
    public virtual void Run() { }
    public int GetTeamNumber() => team;
    protected virtual void Start()
    {
        SubscribeEvents();
    }
    protected virtual void OnDestroy()
    {
        deathNotif?.Invoke(this);
    }
    void SubscribeEvents()
    {
        EventsHandler.SubscribeToEvent("EVENT_START", () => _updateEnabled = true);
    }
}
