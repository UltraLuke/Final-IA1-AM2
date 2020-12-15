using UnityEngine;

public class Rotator2 : MonoBehaviour
{
    [Header("Raycast")]
    //Raycast
    public LayerMask layerMaskRay;
    public float raycastRange;
    delegate void InstantiateRays();
    InstantiateRays instantiateRays;

    public float sensitivity;
    bool _hitWall;

    public bool HitWall { get => _hitWall; }

    private void Start()
    {
        instantiateRays += LeftRay;
        instantiateRays += RightRay;
        instantiateRays += FrontRay;
        instantiateRays += DownRay;
        instantiateRays += UpRay;
    }
    private void Update()
    {
        instantiateRays();
        RotateFromMouseInput();
    }
    void RotateFromMouseInput()
    {
        float rotateHorizontal = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * rotateHorizontal * sensitivity);
    }

    #region Raycasts
    void FrontRay()
    {
        RaycastHit hitFront;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitFront, raycastRange, layerMaskRay))
        {
            _hitWall = true;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitFront.distance, Color.yellow);
        }
        else
        {
            _hitWall = false;
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * raycastRange, Color.white);
        }
    }
    void LeftRay()
    {
        RaycastHit hitLeft;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hitLeft, raycastRange, layerMaskRay))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * hitLeft.distance, Color.yellow);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * raycastRange, Color.white);
        }
    }
    void RightRay()
    {
        RaycastHit hitRight;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hitRight, raycastRange, layerMaskRay))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hitRight.distance, Color.yellow);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * raycastRange, Color.white);
        }
    }
    void UpRay()
    {
        RaycastHit hitUp;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hitUp, raycastRange, layerMaskRay))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hitUp.distance, Color.yellow);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * raycastRange, Color.white);
        }
    }
    void DownRay()
    {
        RaycastHit hitDown;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitDown, raycastRange, layerMaskRay))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hitDown.distance, Color.yellow);
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * raycastRange, Color.white);
        }
    }
    #endregion
}