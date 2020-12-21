using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagGoal : MonoBehaviour
{
    [SerializeField] List<GameObject> entitiesTeam1;
    [SerializeField] List<GameObject> entitiesTeam2;
    [SerializeField] float addingValue;
    [SerializeField] float addingInterval;

    //NO EDITAR A MANO!!
    public float leftValue;
    public float rightValue;
    public float dominanceValue = 0;
    public float currTime = 0;

    Dictionary<Controller, int> entities = new Dictionary<Controller, int>();
    Queue<Controller> _incomingEntities = new Queue<Controller>();
    Queue<Controller> _outgoingEntities = new Queue<Controller>();

    private void Update()
    {
        if (_incomingEntities.Count > 0)
        {
            while (_incomingEntities.Count > 0)
            {
                var newTeamMember = _incomingEntities.Dequeue();
                entities[newTeamMember] = newTeamMember.GetTeamNumber();
            }
        }
        if (_outgoingEntities.Count > 0)
        {
            while (_outgoingEntities.Count > 0)
            {
                entities.Remove(_outgoingEntities.Dequeue());
            }
        }

        if (currTime <= 0)
        {
            if (entities.Count > 0)
            {
                int iteracion = 0;
                foreach (var entity in entities)
                {
                    if (entity.Key == null)
                    {
                        entities.Remove(entity.Key);
                    }
                    else
                    {
                        if (entity.Value == 0)
                            dominanceValue += addingValue;
                        else if(entity.Value == 1)
                            dominanceValue -= addingValue;
                        iteracion++;
                    }
                }
            }

            if(dominanceValue >= rightValue)
                EventsHandler.TriggerEvent("EVENT_TEAM1WINS");
            else if (dominanceValue <= leftValue)
                EventsHandler.TriggerEvent("EVENT_TEAM2WINS");

            currTime = addingInterval;
        }
        else
            currTime -= Time.deltaTime;
    }

    public void DeathNotif(Controller controller)
    {
        if (entities.ContainsKey(controller))
            _outgoingEntities.Enqueue(controller);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Controller>(out var entity))
        {
            if (!entities.ContainsKey(entity))
            {
                entity.deathNotif += DeathNotif;
                _incomingEntities.Enqueue(entity);
            }
            //int team = entity.GetTeamNumber();
            //entities[entity] = team;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Controller>(out var entity))
        {
            if (entities.ContainsKey(entity))
            {
                entity.deathNotif -= DeathNotif;
                _outgoingEntities.Enqueue(entity);
            }
            //if (entities.ContainsKey(entity))
            //    entities.Remove(entity);
        }
    }
}