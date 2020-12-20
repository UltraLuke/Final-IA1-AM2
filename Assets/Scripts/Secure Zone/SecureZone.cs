using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecureZone : MonoBehaviour
{
    [Header("Healing")]
    [SerializeField] int team;
    [SerializeField] float healingValue;
    [SerializeField] float healingInterval;

    List<Model> _entitiesModels = new List<Model>();
    private float _currTime;

    private void Update()
    {
        if (_currTime <= 0)
        {
            if (_entitiesModels.Count > 0)
            {
                for (int i = 0; i < _entitiesModels.Count; i++)
                {
                    if (_entitiesModels[i] != null)
                        _entitiesModels[i].Heal(healingValue);
                }
            }
            _currTime = healingInterval;
        }
        else
            _currTime -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Model>(out var model))
        {
            if (other.TryGetComponent<ITeam>(out var team) && this.team == team.GetTeamNumber())
            {
                _entitiesModels.Add(model);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent<Model>(out var model) && _entitiesModels.Contains(model))
        {
            _entitiesModels.Remove(model);
        }
    }
}
