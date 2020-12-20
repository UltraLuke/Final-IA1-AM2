using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemyChecker : MonoBehaviour
{
    [Tooltip("El propio team. Cualquier entidad de equipo distinto se guarda")]
    public int team;

    float _range;
    List<Controller> _enemies = new List<Controller>();
    SphereCollider _collider;
    private bool _cleanList;

    public float Range
    {
        get => _range;
        set => _range = _collider.radius = value;
    }
    public List<Controller> Enemies { get => _enemies; }

    private void Awake()
    {
        _collider = GetComponent<SphereCollider>();
    }
    private void Update()
    {
        if(_cleanList)
        {
            for (int i = 0; i < _enemies.Count; i++)
            {
                if (_enemies[i] == null)
                {
                    _enemies.RemoveAt(i);
                    i--;
                }
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Controller>(out var controller))
        {
            if (controller.GetTeamNumber() != team)
                _enemies.Add(controller);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Controller>(out var controller) && _enemies.Contains(controller))
            _enemies.Remove(controller);
    }
    public void ClearNullElements() => _cleanList = true;
}
