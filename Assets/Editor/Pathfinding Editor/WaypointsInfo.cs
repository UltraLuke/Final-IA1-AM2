using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaypointsInfo", menuName = "Pathfinding Editor/Waypoints Info")]

public class WaypointsInfo : ScriptableObject
{
    public bool displayConnectionLines;
    public float radiusDistanceConnection;
    public List<WaypointData> waypointsData;
}

[System.Serializable]
public struct WaypointData
{
    public int id;
    public Vector3 position;
    public List<int> connectedNodesID;
}
