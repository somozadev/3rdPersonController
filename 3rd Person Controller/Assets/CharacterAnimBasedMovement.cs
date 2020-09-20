using System.Collections;
using System.Collections.Generic;
using UnityEngine;




[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Animator))]


public class CharacterAnimBasedMovement : MonoBehaviour
{

    public float rotationSpeed = 4f;
    public float rotationThreshold = 0.3f;

    [Header("Animator Parameters")]
    public string motionParam = "motion";
    public string turn180 = "180turn";
    public string rotation = "rotation";
    public string mirrorIdleParam = "MirrorIdle";
    public string jumpParam = "Jump";
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


    private bool turn180Bool;
    public int degreesToTurn = 100;


    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    public void MoveCharacter(float hInput, float vInput, Camera cam, bool jump, bool dash)
    {
        Debug.Log(jump + ">>>salto");
        //Debug.LogWarning(Vector3.Angle(transform.forward, desiredMoveDirection) + "    ASDASDASDASDASD" );
        speed = new Vector2(hInput, vInput).normalized.sqrMagnitude;

        if (speed >= speed - rotationThreshold && dash)
        {
            speed = 1.5f;
        }
        if (jump)
            animator.SetBool(jumpParam, jump);
        //else
        //    animator.SetBool(jumpParam, jump);

        if (speed > rotationThreshold)
        {
            animator.SetFloat(motionParam, speed, startAnimTime, Time.deltaTime); //AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA
            Vector3 forward = cam.transform.forward;
            Vector3 right = cam.transform.right;
            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            desiredMoveDirection = forward * vInput + right * hInput;
            //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotationSpeed * Time.deltaTime);

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
