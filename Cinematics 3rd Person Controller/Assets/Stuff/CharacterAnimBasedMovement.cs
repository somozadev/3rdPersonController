using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]


public class CharacterAnimBasedMovement : MonoBehaviour
{
    public float distanciaSuelo = 0.1f;
    public GameObject baseRaycast;
    public Vector3 diresio;
    public float rotationSpeed = 4f;
    public float rotationThreshold = 0.3f;

    [Header("Animator Parameters")]
    public string motionParam = "motion";
    public string turn180 = "180turn";
    public string rotation = "rotation";
    public string mirrorIdleParam = "MirrorIdle";
    public string jumpParam = "Jump";
    public string wallHitParam = "WallCollided";
    public string fallParam = "Fall";
    public string fall2HighParam = "Fall2High";
    public string dance1Param = "dance1";
    public string dance2Param = "dance2";
    public string prankParam = "prank";
    [Header("Animator Smoothing")]
    [Range(0, 1f)]
    public float startAnimTime = 0.3f;
    [Range(0, 1f)]
    public float stopAnimTime = 0.15f;
    [SerializeField]
    private float speed;
    
    private Vector3 desiredMoveDirection;
    private CharacterController characterController;
    private Animator animator;


    private bool mirrorIdle;
    //private Vector3 pivotPosStart;
    //private Vector3 pivotPosCurrent;

    private bool turn180Bool;
    public int degreesToTurn = 100;


    public bool isColliding = false;
    private float tiempo;
    public bool isCollidingFloor = true;
    private float tiempo2; 
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        //pivotPosStart = animator.pivotPosition;

    }
    public void MoveCharacter(float hInput, float vInput, Camera cam, bool jump, bool dash)
    {
        float auxt = 0f;
        #region tiempo
        auxt += Time.deltaTime;
        //Debug.Log(tiempo);
        tiempo += Time.deltaTime;
        if (isColliding)
        {
            if (tiempo >= 1.3f)
            {
                animator.SetBool(wallHitParam, false);
                tiempo = 0;
                isColliding = false; 
            }
        }

        #endregion
        #region velocidad

        speed = new Vector2(hInput, vInput).normalized.sqrMagnitude;

        if (speed >= speed - rotationThreshold && dash)
        {
            speed = 1.5f;
        }

        #endregion
        #region TiposDeSaltoIdleMidBig
        Ray ray = new Ray(baseRaycast.transform.position, diresio);
        RaycastHit hitInfo;
        
        float rayDistance = 5.0f;
        if (Physics.Raycast(ray, out hitInfo, rayDistance))
        {
            
            if (Vector3.Distance(transform.position, hitInfo.point) < distanciaSuelo && !jump)
            {
                animator.SetBool(jumpParam, jump);

            }
            else
            {
                animator.SetBool(jumpParam, jump);
            }
        }
        #endregion
        #region falling
        Ray ray2 = new Ray(transform.position, diresio);
        RaycastHit hitInfo2;
        float rayDistance2 = 50f;
        float distanciaSuelo2 = 2f; 
        if (Physics.Raycast(ray2,out hitInfo2, rayDistance2))
        {
            Debug.DrawLine(transform.position, hitInfo2.point, Color.red);
            if (Vector3.Distance(transform.position,hitInfo2.point) > distanciaSuelo2 && Vector3.Distance(transform.position, hitInfo2.point) < 7f)
            {
                animator.SetBool(fallParam, true);
            }
            else
            {
                animator.SetBool(fallParam, false);
            }
            if (Vector3.Distance(transform.position, hitInfo2.point) > 7f)
            {
                animator.SetBool(fall2HighParam, true);
            }
            else
            {
                animator.SetBool(fall2HighParam, false);
            }
            //else if (Vector3.Distance(transform.position, hitInfo2.point) <= 0.5f)
            //{
            //    auxt = 0;
            //    if (auxt == 2f)
            //    {
            //        animator.SetBool(fall2HighParam, false);
            //    }
               
            //}
            

        }
        #endregion
        #region Rotacion&Movimiento
        if (speed > rotationThreshold)
        {
            animator.SetFloat(motionParam, speed, startAnimTime, Time.deltaTime); 
            Vector3 forward = cam.transform.forward;
            Vector3 right = cam.transform.right;
            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            desiredMoveDirection = forward * vInput + right * hInput;
            
            if (Vector3.Angle(transform.forward, desiredMoveDirection) >= degreesToTurn)
                turn180Bool = true;
            else
            {
                turn180Bool = false;
                transform.rotation = Quaternion.Slerp(transform.rotation,Quaternion.LookRotation(desiredMoveDirection), rotationSpeed * Time.deltaTime);
            }
            animator.SetBool(turn180, turn180Bool);
            animator.SetFloat(rotation, speed, startAnimTime, Time.deltaTime);
            animator.SetFloat(motionParam, speed, startAnimTime, Time.deltaTime);
        }

        else if (speed < rotationThreshold)
        {
            animator.SetBool(mirrorIdleParam, mirrorIdle);
            animator.SetFloat(motionParam, speed, stopAnimTime, Time.deltaTime);
        }
        #endregion
    }
    public void Gestos(bool dance1, bool dance2, bool prank)
    {
        animator.SetBool(dance1Param, dance1);
        animator.SetBool(dance2Param, dance2);
        animator.SetBool(prankParam, prank);

    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Wall")
        {
            tiempo = 0;
            //Debug.Log("chocó");
            isColliding = true;
            animator.SetBool(wallHitParam, true);
        }
       
    }
    private void OnAnimatorIK(int layerIndex)
    {
        if (speed < rotationThreshold) return;

        float distanceToLeftFoot = Vector3.Distance(transform.position, animator.GetIKPosition(AvatarIKGoal.LeftFoot));
        float distanceToRightFoot = Vector3.Distance(transform.position, animator.GetIKPosition(AvatarIKGoal.RightFoot));

        if (distanceToRightFoot > distanceToLeftFoot)
        {
            mirrorIdle = true;
        }
        else
        {
            mirrorIdle = false; 
        }

    }
}
