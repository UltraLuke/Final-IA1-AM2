using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagGoal : MonoBehaviour
{
    [SerializeField] float addingValue;
    [SerializeField] float addingInterval;
    float totalValue;
    float dominanceState = 0;
    float currTime = 0;

    Dictionary<ITeam, int> entities = new Dictionary<ITeam, int>();

    private void Update()
    {
        if(currTime <= 0)
        {
            if (entities.Count > 0)
            {
                foreach (var entity in entities)
                {
                    if (entity.Value == 0)
                        dominanceState += addingValue;
                    else if(entity.Value == 1)
                        dominanceState -= addingValue;
                }
            }
            currTime = addingInterval;
        }
        else
            currTime -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<ITeam>(out var entity))
        {
            int team = entity.GetTeamNumber();
            entities[entity] = team;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<ITeam>(out var entity))
        {
            if(entities.ContainsKey(entity))
                entities.Remove(entity);
        }
    }
}
