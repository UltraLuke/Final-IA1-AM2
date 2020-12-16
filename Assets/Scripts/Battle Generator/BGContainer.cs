using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGContainer : MonoBehaviour
{
    [Header("Leaders")]
    public List<GameObject> leaders;
    [Header("Minions")]
    public List<MinionGroup> minionGroups = new List<MinionGroup>() { new MinionGroup(), new MinionGroup() };

    [Header("Flocking")]
    public List<FlockingGroup> flockingGroup = new List<FlockingGroup>() { new FlockingGroup() , new FlockingGroup() };

    List<List<GameObject>> _minions;

    public List<List<GameObject>> Minions
    {
        get => _minions;
        set
        {
            _minions = value;

            for (int i = 0; i < minionGroups.Count; i++)
            {
                minionGroups[i].minions = _minions[i];
            }
        }
    }
    //public List<GameObject> leaders { get => _leaders; set => _leaders = value; }
    //public List<FlockingGroup> flockingGroup { get => _flockingGroup; set => _flockingGroup = value; }
}

[System.Serializable]
public class MinionGroup
{
    public List<GameObject> minions;
}
[System.Serializable]
public class FlockingGroup
{
    public Vector3 areaPosition;
    public Vector2 areaSize;
    public int quantityRow;
    public int quantityColumn;
}