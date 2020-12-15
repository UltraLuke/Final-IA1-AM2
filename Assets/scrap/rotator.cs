using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rotator : MonoBehaviour
{
    [Header("Raycast")]
    //Raycast
    public LayerMask layerMaskRay;
    public float raycastRange;
    delegate void InstantiateRays();
    InstantiateRays instantiateRays;

    [Header("Rotation")]
    //Rotation
    bool rotating;
    public float rotationCounter;
    public Vector3 rotate;

    float time = 0.8f;

    //PLAYER:
    [Header("Player Movement")]
    public float sensitivity;
    public float runSpeedMovement;
    public float walkSpeedMovement;
    float speedMovement;
    //public bool jumping;
    //public float distanceMarginToMoveUp;
    //public float distanceMarginToMoveDown;
    //public float jumpSpeed;
    //public float jumpHeight;
    //public float maxJumpHeight;

    Rotator2 rotator2;

    private void Start()
    {
        rotator2 = GetComponentInChildren<Rotator2>();

        //instantiateRays += LeftRay;
        //instantiateRays += RightRay;
        //instantiateRays += FrontRay;
        //instantiateRays += DownRay;
        //instantiateRays += UpRay;
    }
    private void Update()
    {
        //instantiateRays();
        KeyboardMovement();
        CheckRotationToWall();
    }
    public void Rotate()
    {
        StartCoroutine(RotateMe(rotate, time));
    }

    void KeyboardMovement()
    {
        if (!rotating)
        {
            Transform trRotator = rotator2.transform;
            if (Input.GetKey(KeyCode.LeftShift))
                speedMovement = runSpeedMovement;
            else
                speedMovement = walkSpeedMovement;
            float horizontalSpeed = Input.GetAxis("Horizontal");
            float verticalSpeed = Input.GetAxis("Vertical");
            transform.position += trRotator.forward * verticalSpeed * speedMovement * Time.deltaTime;
            transform.position += trRotator.right * horizontalSpeed * speedMovement * Time.deltaTime;
        }
    }
    void CheckRotationToWall()
    {
        bool hitWall = rotator2.HitWall;

        if (hitWall)
        {
            if (!rotating)
            {
                rotationCounter += Time.deltaTime;
                if (rotationCounter >= 2)
                {
                    rotating = true;

                    Vector3 rotationAngle = rotate;
                    StartCoroutine(RotateMe(rotationAngle, 0.8f));
                }
            }
        }
        else
        {
            rotationCounter = 0;
        }
    }

    //#region Raycasts
    //void FrontRay()
    //{
    //    bool hitWall;

    //    RaycastHit hitFront;
    //    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hitFront, raycastRange, layerMaskRay))
    //    {
    //        hitWall = true;
    //        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitFront.distance, Color.yellow);
    //    }
    //    else
    //    {
    //        hitWall = false;
    //        rotationCounter = 0;
    //        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * raycastRange, Color.white);
    //    }

    //    if (hitWall)
    //    {
    //        if (!rotating)
    //        {
    //            rotationCounter += Time.deltaTime;
    //            if (rotationCounter >= 2)
    //            {
    //                rotating = true;

    //                Vector3 rotationAngle = new Vector3(-90, 0, 0);
    //                StartCoroutine(RotateMe(rotationAngle, 0.8f));
    //            }
    //        }
    //    }
    //}
    //void LeftRay()
    //{
    //    RaycastHit hitLeft;
    //    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.left), out hitLeft, raycastRange, layerMaskRay))
    //    {
    //        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * hitLeft.distance, Color.yellow);
    //    }
    //    else
    //    {
    //        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.left) * raycastRange, Color.white);
    //    }
    //}
    //void RightRay()
    //{
    //    RaycastHit hitRight;
    //    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.right), out hitRight, raycastRange, layerMaskRay))
    //    {
    //        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * hitRight.distance, Color.yellow);
    //    }
    //    else
    //    {
    //        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.right) * raycastRange, Color.white);
    //    }
    //}
    //void UpRay()
    //{
    //    RaycastHit hitUp;
    //    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.up), out hitUp, raycastRange, layerMaskRay))
    //    {
    //        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * hitUp.distance, Color.yellow);
    //    }
    //    else
    //    {
    //        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.up) * raycastRange, Color.white);
    //    }
    //}
    //void DownRay()
    //{
    //    RaycastHit hitDown;
    //    if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hitDown, raycastRange, layerMaskRay))
    //    {
    //        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hitDown.distance, Color.yellow);
    //    }
    //    else
    //    {
    //        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * raycastRange, Color.white);
    //    }
    //}
    //#endregion
    IEnumerator RotateMe(Vector3 byAngles, float inTime)
    {
        //Proceso del giro
        var fromAngle = transform.localRotation;
        var toAngle = Quaternion.Euler(transform.localEulerAngles + byAngles);
        for (var t = 0f; t < 1.1f; t += Time.deltaTime / inTime)
        {
            transform.localRotation = Quaternion.Slerp(fromAngle, toAngle, t);
            yield return null;
        }

        rotating = false;
    }
}
