using UnityEngine;
public struct TeamSettings
{
    //Leader
    public float leaderHealth;
    public float leaderSpeed;
    public float leaderMeleeDamage;
    public float leaderMeleeRate;
    public float leaderMeleeDistance;
    public float leaderShootDamage;
    public float leaderShootRate;
    public float leaderShootDistance;
    public float leaderVisionDistance;
    public float leaderVisionRangeAngles;

    //Minions
    public int minionsQuantity;
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

    //Flocking
    public float flockEntityRadius;
    public LayerMask flockEntityMask;
    public float flockLeaderBehaviourWeight;
    public Transform flockLeaderBehaviourTarget;
    public float flockLeaderBehaviourMinDistance;
    public float flockAlineationBehaviourWeight;
    public float flockSeparationBehaviourWeight;
    public float flockSeparationBehaviourRange;
    public float flockCohesionBehaviourWeight;
    public float flockAvoidanceBehaviourWeight;
    public LayerMask flockAvoidanceBehaviourMask;
    public float flockAvoidanceBehaviourRange;
}