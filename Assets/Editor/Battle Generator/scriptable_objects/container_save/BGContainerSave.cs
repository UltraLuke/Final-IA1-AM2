using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BG Container Save", menuName = "Battle Generator/Container Save")]
public class BGContainerSave : ScriptableObject
{
    public List<GameObject> leaders = new List<GameObject>() { null, null };
    public List<List<MinionData>> minions = new List<List<MinionData>>() { new List<MinionData>(), new List<MinionData>() };
    public List<TeamSettings> ts = new List<TeamSettings>() { new TeamSettings(), new TeamSettings()};
}

public class MinionData
{
    public GameObject minion;
    public Vector3 position;
}