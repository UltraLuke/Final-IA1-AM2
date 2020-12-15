using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BG Leader Preset", menuName = "Battle Generator/Leader Preset")]
public class BGLeaderPreset : BGPreset
{
    public GameObject leaderEntity;
    public Vector3 leaderPosition;
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
}
