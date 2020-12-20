using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model : MonoBehaviour, IHealth, ISpeed, IMelee, IShooter, IVision
{
    public virtual void Move(Vector3 dir) { }
    public virtual void LookAtDir(Vector3 dir) { }
    public virtual bool IsInSight(Transform target) { return false; }
    public virtual bool CheckAndGetClosestEnemyInSight(int team, out Transform enemy) { enemy = null; return false; }
    public virtual void Shoot(Transform target, int teamNumber) { }
    public virtual void ApplyDamage(float amount) { }
    public virtual void Heal(float amount) { }

    #region interfaces
    public virtual Component HealthSettings(float health) { return this; }
    public virtual Component SpeedSettings(float speed) { return this; }
    public virtual Component MeleeSettings(float damage, float rate, float distance) { return this; }
    public virtual Component ShootSettings(float damage, float rate, float distance) { return this; }
    public virtual Component VisionSettings(float distance, float angle) { return this; }
    public virtual float GetHealth() { return 0; }
    public virtual float GetSpeed() { return 0; }
    public virtual void GetMeleeData(out float meleeDamage, out float meleeRate, out float meleeDistance)
    {
        meleeDamage = meleeRate = meleeDistance = 0;
    }
    public virtual void GetShootData(out float shootDamage, out float shootRate, out float shootDistance)
    {
        shootDamage = shootRate = shootDistance = 0;
    }
    public virtual void GetVisionData(out float visionDistance, out float visionRangeAngles)
    {
        visionDistance = visionRangeAngles = 0;
    }
    #endregion
}
