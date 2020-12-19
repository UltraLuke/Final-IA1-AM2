using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderModel : Model
{
    [Header("Health")]
    [Tooltip("Vida actual")] public float health;
    [Tooltip("Vida máxima")] public float maxHealth;

    [Header("Speed")]
    public float speed;
    public float speedRot = 0.2f;

    [Header("Attack")]
    public Bullet bullet;
    public Transform cannon;
    public LayerMask enemyLayers;

    [HideInInspector] public float meleeDamage;
    [HideInInspector] public float meleeRate;
    [HideInInspector] public float meleeDistance;
    [HideInInspector] public float shootDamage;
    [HideInInspector] public float shootRate;
    [HideInInspector] public float shootDistance;

    [Header("Vision")]
    public float visionDistance;
    public float visionAngle;
    public LayerMask visionMask;

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
    public override void LookAtDir(Vector3 dir) => transform.forward = dir;
    public override bool IsInSight(Transform target)
    {
        if (target == null) return false;
        Vector3 diff = (target.position - transform.position);
        //A--->B
        //B-A
        float distance = diff.magnitude;
        if (distance > visionDistance) return false;
        if (Vector3.Angle(transform.forward, diff) > visionAngle / 2) return false;
        if (Physics.Raycast(transform.position, diff.normalized, distance, visionMask)) return false;
        return true;
    }
    //Obtengo los enemigos (los que son de distinto equipo que el pasado por parámetro)
    public override bool CheckAndGetClosestEnemyInSight(int team, out Transform enemy)
    {
        bool enemyInSight = false;
        float distance = 0f;
        ITeam teamComp;
        enemy = null;
        Collider[] colliders = Physics.OverlapSphere(transform.position, visionDistance, enemyLayers);
        for (int i = 0; i < colliders.Length; i++)
        {
            if(IsInSight(colliders[i].transform) && colliders[i].TryGetComponent(out teamComp) && teamComp.GetTeamNumber() != team)
            {
                float currDistance = (colliders[i].transform.position - transform.position).magnitude;
                if (enemy == null || currDistance < distance)
                {
                    enemy = colliders[i].transform;
                    distance = currDistance;
                    enemyInSight = true;
                }
            }
        }
        return enemyInSight;
    }
    public override void Shoot(Transform target, int teamNumber)
    {
        if (bullet == null) return;
        Transform from;

        if (cannon != null) from = cannon;
        else from = transform;

        var dir = target.position - from.position;
        var newBullet = Instantiate(bullet);
        newBullet.Dir = dir.normalized;
        newBullet.teamNumber = teamNumber;
    }

    public override void ApplyDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
            Die();
    }
    void Die(){ Debug.Log(gameObject.name + " dice: Morí X_X"); }

    #region Interface methods
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
        visionRangeAngles = this.visionAngle;
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * visionDistance);
        Gizmos.DrawWireSphere(transform.position, visionDistance);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, visionAngle / 2, 0) * transform.forward * visionDistance);
        Gizmos.DrawRay(transform.position, Quaternion.Euler(0, -visionAngle / 2, 0) * transform.forward * visionDistance);
    }
}
