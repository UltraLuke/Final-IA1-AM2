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
    bool _clicked = false;

    Camera _cmra;
    AgentTheta _agentTheta;

    Node initNode;
    Node finitNode;

    private void Awake()
    {
        _agentTheta = GetComponent<AgentTheta>();
    }
    private void Start()
    {
        _plane = new Plane(Vector3.up, _clickPlaneReference);
        _cmra = Camera.main;
    }

    private void Update()
    {
        if (_clicked)
        {
            _agentTheta.PathFindingTheta();
            _clicked = false;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            bool validateClick = false;
            Ray ray = _cmra.ScreenPointToRay(Input.mousePosition);

            if (_plane.Raycast(ray, out float enter))
            {
                var hitPoint = ray.GetPoint(enter);

                if((initNode = GetNearestNodeToTarget(transform.position, hitPoint)) &&
                   (finitNode = GetNearestNodeToTarget(hitPoint, transform.position)))
                {
                    validateClick = true;
                }

                if (validateClick)
                {
                    _agentTheta.Init = initNode;
                    _agentTheta.Finit = finitNode;
                    _agentTheta.FinPos = hitPoint;
                    _clicked = true;
                }
            }
        }
    }

    private Node GetNearestNodeToTarget(Vector3 start, Vector3 target)
    {
        float distance = 0f;
        Node closestNode = null;

        Collider[] nodes = Physics.OverlapSphere(/*transform.position*/ start, _areaDetection, _layerToDetect);

        if (nodes == null) return null;
        for (int i = 0; i < nodes.Length; i++)
        {
            float newDistance = Vector3.Distance(nodes[i].transform.position, target);

            //Si initNode es nulo, lo asigno directamente
            //Si la distancia entre el punto y el nodo es menor que el que tengo guardado, lo asigno tambien
            if (closestNode == null || newDistance < distance)
            {
                closestNode = nodes[i].GetComponent<Node>();
                distance = newDistance;
            }
        }

        return closestNode;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(_clickPlaneReference + new Vector3(-1, 0, 1), _clickPlaneReference + new Vector3(1, 0, 1));
        Gizmos.DrawLine(_clickPlaneReference + new Vector3(1, 0, 1), _clickPlaneReference + new Vector3(1, 0, -1));
        Gizmos.DrawLine(_clickPlaneReference + new Vector3(1, 0, -1), _clickPlaneReference + new Vector3(-1, 0, -1));
        Gizmos.DrawLine(_clickPlaneReference + new Vector3(-1, 0, -1), _clickPlaneReference + new Vector3(-1, 0, 1));
    }
}