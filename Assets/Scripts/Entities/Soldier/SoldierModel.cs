using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierModel : Model, IHealth, ISpeed, IMelee, IShooter, IVision
{
    [Header("Health")]
    [Tooltip("Vida actual")] public float health;
    [Tooltip("Vida máxima")] public float maxHealth;
    
    [Header("Speed")]
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

    private void OnValidate()
    {
        health = Mathf.Clamp(health, 0, maxHealth);
    }
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
    public override void ApplyDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
            Die();
    }
    void Die() { /*Debug.Log(gameObject.name + " dice: Morí X_X");*/Destroy(gameObject); }
    public override void Heal(float amount)
    {
        health += amount;
        if (health > maxHealth) health = maxHealth;
    }
    #region method interfaces
    public override Component HealthSettings(float health)
    {
        this.health = health;
        return this;
    }
    public override Component SpeedSettings(float speed)
    {
        this.speed = speed;
        return this;
    }
    public override Component MeleeSettings(float damage, float rate, float distance)
    {
        meleeDamage = damage;
        meleeRate = rate;
        meleeDistance = distance;
        return this;
    }
    public override Component ShootSettings(float damage, float rate, float distance)
    {
        shootDamage = damage;
        shootRate = rate;
        shootDistance = distance;
        return this;
    }
    public override Component VisionSettings(float distance, float angle)
    {
        visionDistance = distance;
        visionAngle = angle;
        return this;
    }
    public override float GetHealth() => health;
    public override float GetSpeed() => speed;
    public override void GetMeleeData(out float meleeDamage, out float meleeRate, out float meleeDistance)
    {
        meleeDamage = this.meleeDamage;
        meleeRate = this.meleeRate;
        meleeDistance = this.meleeDistance;
    }
    public override void GetShootData(out float shootDamage, out float shootRate, out float shootDistance)
    {
        shootDamage = this.shootDamage;
        shootRate = this.shootRate;
        shootDistance = this.shootDistance;
    }
    public override void GetVisionData(out float visionDistance, out float visionRangeAngles)
    {
        visionDistance = this.visionDistance;
        visionRangeAngles = visionAngle;
    }
    #endregion
}
