using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class AgentTheta : MonoBehaviour
{
    [Header("Gizmos settings")]
    [SerializeField] LayerMask mask;
    [SerializeField] float radius;
    [SerializeField] Vector3 offset;
    //public float distanceMax;

    private Node init;
    private Node finit;
    private Vector3 finPos;
    private Controller controller;
    List<Node> _list;
    List<Vector3> _listVector;
    Theta<Node> _theta = new Theta<Node>();

    public Node Init { get => init; set => init = value; }
    public Node Finit { get => finit; set => finit = value; }
    public Vector3 FinPos { get => finPos; set => finPos = value; }

    private void Awake()
    {
        controller = GetComponent<Controller>();
    }

    public void PathFindingTheta()
    {
        _list = _theta.Run(init, Satisfies, GetNeighbours, GetCost, Heuristic, InSight);

        Vector3 dir;
        //for (int i = 0; i < _list.Count; i++)
        //while (_list.Count >= 2)
        //{
        //    dir = init.transform.position - transform.position;
        //    Debug.DrawRay(transform.position, dir);
        //    if (!Physics.Raycast(transform.position, dir.normalized, dir.magnitude, mask))
        //    {
        //        init = _list[1];
        //        _list.RemoveAt(0);
        //    }
        //    else
        //        break;
        //}
        for (int i = _list.Count - 2; i >= 0; i--)
        {
            dir = finPos - _list[i].transform.position;
            if (!Physics.Raycast(_list[i].transform.position, dir.normalized, dir.magnitude, mask))
                _list.RemoveAt(i + 1);
            else
                break;
        }
        controller.SetWayPoints(_list, finPos);
    }



    bool InSight(Node gP, Node gC)
    {
        var dir = gC.transform.position - gP.transform.position;
        if (Physics.Raycast(gP.transform.position, dir.normalized, dir.magnitude, mask))
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
