using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BG FLocking Preset", menuName = "Battle Generator/Flocking Preset")]
public class BGFlockingPreset : BGPreset
{
    [Header("Minions Settings")]
    public GameObject minionEntity;
    public Vector3 minionSpawnAreaPosition;
    public float minionSpawnAreaWidth;
    public float minionSpawnAreaLength;
    public int minionsQuantityRow;
    public int minionsQuantityColumn;
    public float minionHealth;
    public float minionSpeed;
    public float minionMeleeDamage;
    public float minionMeleeRate;
    public float minionMeleeDistance;
    public float minionShootDamage;
    public float minionShootRate;
    public float minionShootDistance;
    public float minionVisionDistance;
    public float minionVisionRangeAngles;

    [Header("Flocking Settings")]
    public float flockEntityRadius;
    public LayerMask flockEntityMask;
    public float flockLeaderBehaviourWeight;
    public float flockLeaderBehaviourMinDistance;
    public float flockAlineationBehaviourWeight;
    public float flockSeparationBehaviourWeight;
    public float flockSeparationBehaviourRange;
    public float flockCohesionBehaviourWeight;
    public float flockAvoidanceBehaviourWeight;
    public LayerMask flockAvoidanceBehaviourMask;
    public float flockAvoidanceBehaviourRange;
}
