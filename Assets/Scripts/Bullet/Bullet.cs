using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public float speed;
    public float damage;
    public float lifeTime;
    public int teamNumber;
    public bool friendlyFire;

    Rigidbody _rb;
    Vector3 _dir = Vector3.zero;
    float _timer;

    public Vector3 Dir { set => _dir = transform.forward = value; }

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        _timer = lifeTime;
    }

    private void Update()
    {
        _timer -= Time.deltaTime;
        if (_timer < 0)
            Destroy(gameObject);
        else
            _rb.velocity = _dir * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Model>(out var model))
        {
            if (!friendlyFire)
            {
                if(other.TryGetComponent<ITeam>(out var team) && team.GetTeamNumber() != teamNumber)
                {
                    model.ApplyDamage(damage);
                }
            }
            else
            {
                model.ApplyDamage(damage);
            }

            Destroy(gameObject);
        }
    }
}
