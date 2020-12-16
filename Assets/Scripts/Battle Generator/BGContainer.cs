using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGContainer : MonoBehaviour
{
    [Header("Leaders")]
    [SerializeField] List<GameObject> _leaders;
    [Header("Minions")]
    [SerializeField] List<MinionGroup> _minionGroups = new List<MinionGroup>() { new MinionGroup(), new MinionGroup() };

    [Header("Flocking")]
    [SerializeField] List<FlockingGroup> _flockingGroup = new List<FlockingGroup>() { new FlockingGroup() , new FlockingGroup() };

    List<List<GameObject>> _minions;
    //Vector3[] _areaPosition;
    //Vector3[] _areaSize;
    //int[] _quantityRow;
    //int[] _quantityColumn;

    public List<List<GameObject>> Minions
    {
        get => _minions;
        set
        {
            _minions = value;

            for (int i = 0; i < _minionGroups.Count; i++)
            {
                _minionGroups[i].minions = _minions[i];
            }
        }
    }
    public List<GameObject> Leaders { get => _leaders; set => _leaders = value; }
    public List<FlockingGroup> FlockingGroup { get => _flockingGroup; set => _flockingGroup = value; }
    //public Vector3[] AreaPosition
    //{
    //    get => _areaPosition;
    //    set
    //    {
    //        _areaPosition = value;
    //        for (int i = 0; i < _flockingGroup.Count; i++)
    //        {
    //            _flockingGroup[i].areaPosition = _areaPosition[i];
    //        }
    //    }
    //}
    //public Vector3[] AreaSize
    //{
    //    get => _areaSize;
    //    set
    //    {
    //        _areaSize = value;
    //        for (int i = 0; i < _flockingGroup.Count; i++)
    //        {
    //            _flockingGroup[i].areaSize = _areaSize[i];
    //        }
    //    }
    //}
    //public int[] QuantityRow
    //{
    //    get => _quantityRow;
    //    set
    //    {
    //        _quantityRow = value;
    //        for (int i = 0; i < _flockingGroup.Count; i++)
    //        {
    //            _flockingGroup[i].quantityRow = _quantityRow[i];
    //        }
    //    }
    //}
    //public int[] QuantityColumn
    //{
    //    get => _quantityColumn;
    //    set
    //    {
    //        _quantityColumn = value;
    //        for (int i = 0; i < _flockingGroup.Count; i++)
    //        {
    //            _flockingGroup[i].quantityColumn = _quantityColumn[i];
    //        }
    //    }
    //}
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