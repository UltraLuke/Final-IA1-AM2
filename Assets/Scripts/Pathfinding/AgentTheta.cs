using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AgentTheta : MonoBehaviour
{
    [Header("Pathfinding Settings")]
    [Tooltip("La máscara correspondiente al obstáculo")]
    [SerializeField] LayerMask obstacleMask;
    [Tooltip("La máscara correspondiente al waypoint")]
    [SerializeField] LayerMask nodeMask;
    [Tooltip("La distancia maxima para detectar un nodo cercano a la propia posicion")]
    [SerializeField] float _distanceDetection;

    [Header("Gizmos settings")]
    [SerializeField] float radius;
    [SerializeField] Vector3 offset;

    private Node init;
    private Node finit;
    private Vector3 finPos;
    private List<Node> _list;
    private Theta<Node> _theta = new Theta<Node>();

    public List<Node> GetPathFinding(Node init, Node finit)
    {
        this.init = init;
        this.finit = finit;
        this.finPos = finit.transform.position;
        return _theta.Run(init, Satisfies, GetNeighbours, GetCost, Heuristic, InSight);
    }
    public List<Node> GetPathFinding(Vector3 init, Vector3 finPos)
    {
        this.init = GetNearestNodeToTarget(init, finPos);
        this.finit = GetNearestNodeToTarget(finPos, init);
        this.finPos = finPos;
        List<Node> list = _theta.Run(this.init, Satisfies, GetNeighbours, GetCost, Heuristic, InSight);
        return FilterStartAndEndPoints(list, finPos);
    }

    //Filtro los nodos innecesarios entre un punto y el nodo mas lejano que este a la vista
    private List<Node> FilterStartAndEndPoints(List<Node> list, Vector3 finPos)
    {
        Vector3 dir;
        while (list.Count > 2)
        {
            dir = list[1].transform.position - transform.position;
            if (!Physics.Raycast(transform.position, dir.normalized, dir.magnitude, obstacleMask))
            {
                list.RemoveAt(0);
            }
            else
                break;
        }
        for (int i = list.Count - 2; i >= 0; i--)
        {
            dir = finPos - list[i].transform.position;
            if (!Physics.Raycast(list[i].transform.position, dir.normalized, dir.magnitude, obstacleMask))
                list.RemoveAt(i + 1);
            else
                break;
        }
        return list;
    }

    //Obtengo el nodo mas cercano al punto inicial, en direccion al target
    private Node GetNearestNodeToTarget(Vector3 start, Vector3 target)
    {
        float distance = 0f;
        Node closestNode = null;

        Collider[] nodes = Physics.OverlapSphere(start, _distanceDetection, nodeMask);

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
    bool InSight(Node gP, Node gC)
    {
        var dir = gC.transform.position - gP.transform.position;
        if (Physics.Raycast(gP.transform.position, dir.normalized, dir.magnitude, obstacleMask))
        {
            return false;
        }
        return true;
    }
    float Heuristic(Node curr)
    {
        return Vector3.Distance(curr.transform.position, finPos);
    }
    float GetCost(Node from, Node to)
    {
        return Vector3.Distance(from.transform.position, to.transform.position);
    }
    List<Node> GetNeighbours(Node curr)
    {
        return curr.Neighbours;
    }
    bool Satisfies(Node curr)
    {
        return curr == finit;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (init != null)
            Gizmos.DrawSphere(init.transform.position + offset, radius);
        Gizmos.DrawSphere(finPos, radius);
        if (_list != null)
        {
            Gizmos.color = Color.blue;
            foreach (var item in _list)
            {
                if (item != init && item != finit)
                    Gizmos.DrawSphere(item.transform.position + offset, radius);
            }
        }
    }
}
