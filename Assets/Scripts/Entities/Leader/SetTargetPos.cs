using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SetTargetPos : MonoBehaviour
{
    [SerializeField] Vector3 _clickPlaneReference;
    [SerializeField] [Range(0, 20)] float _areaDetection;
    [SerializeField] LayerMask _layerToDetect;
    Plane _plane;

    Camera _cmra;
    AgentTheta _agentTheta;
    Controller _controller;

    private void Awake()
    {
        _agentTheta = GetComponent<AgentTheta>();
        _controller = GetComponent<Controller>();
    }
    private void Start()
    {
        _plane = new Plane(Vector3.up, _clickPlaneReference);
        _cmra = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = _cmra.ScreenPointToRay(Input.mousePosition);

            if (_plane.Raycast(ray, out float enter))
            {
                var hitPoint = ray.GetPoint(enter);
                var nodes = _agentTheta.GetPathFinding(transform.position, hitPoint);
                _controller.SetWayPoints(nodes, hitPoint);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(_clickPlaneReference + new Vector3(-1, 0, 1), _clickPlaneReference + new Vector3(1, 0, 1));
        Gizmos.DrawLine(_clickPlaneReference + new Vector3(1, 0, 1), _clickPlaneReference + new Vector3(1, 0, -1));
        Gizmos.DrawLine(_clickPlaneReference + new Vector3(1, 0, -1), _clickPlaneReference + new Vector3(-1, 0, -1));
        Gizmos.DrawLine(_clickPlaneReference + new Vector3(-1, 0, -1), _clickPlaneReference + new Vector3(-1, 0, 1));
    }
}