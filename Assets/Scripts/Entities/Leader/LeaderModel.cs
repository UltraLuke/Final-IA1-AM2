using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderModel : Model, IHealth, ISpeed, IMelee, IShooter, IVision
{
    public float health;
    public float speed;
    public float speedRot = 0.2f;

    public float meleeDamage;
    public float meleeRate;
    public float meleeDistance;
    public float shootDamage;
    public float shootRate;
    public float shootDistance;
    public float visionDistance;
    public float visionAngle;

    Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    public override void Move(Vector3 dir)
    {
        dir.y = 0;
        _rb.velocity = dir * speed;
        transform.forward = Vector3.Lerp(transform.forward, dir, speedRot);
    }

    #region Interface Setters
    public Component HealthSettings(float health)
    {
        this.health = health;
        return this;
    }
    public Component SpeedSettings(float speed)
    {
        this.speed = speed;
        return this;
    }
    public Component MeleeSettings(float damage, float rate, float distance)
    {
        meleeDamage = damage;
        meleeRate = rate;
        meleeDistance = distance;
        return this;
    }
    public Component ShootSettings(float damage, float rate, float distance)
    {
        shootDamage = damage;
        shootRate = rate;
        shootDistance = distance;
        return this;
    }
    public Component VisionSettings(float distance, float angle)
    {
        visionDistance = distance;
        visionAngle = angle;
        return this;
    }
    #endregion
}
