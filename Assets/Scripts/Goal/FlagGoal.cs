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

    //Dictionary<GameObject, int> entities = new Dictionary<GameObject, int>();
    Dictionary<ITeam, int> entities = new Dictionary<ITeam, int>();

    private void Update()
    {
        if(currTime <= 0)
        {
            if (entities.Count > 0)
            {
                int iteracion = 0;
                foreach (var entity in entities)
                {
                    if (entity.Value == 0)
                        dominanceValue += addingValue;
                    else if(entity.Value == 1)
                        dominanceValue -= addingValue;
                    iteracion++;
                }
            }
            currTime = addingInterval;
        }
        else
            currTime -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<ITeam>(out var entity))
        {
            int team = entity.GetTeamNumber();
            entities[entity] = team;
        }

        //if (entitiesTeam1.Contains(other.gameObject))
        //{
        //    entities[other.gameObject] = 0;
        //    Debug.Log("entity team 1 asignado");
        //}
        //else if (entitiesTeam1.Contains(other.gameObject))
        //{
        //    entities[other.gameObject] = 1;
        //    Debug.Log("entity team 2 asignado");
        //}
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<ITeam>(out var entity))
        {
            if (entities.ContainsKey(entity))
                entities.Remove(entity);
        }
    }
}
