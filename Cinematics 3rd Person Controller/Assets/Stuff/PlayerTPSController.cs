using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class PlayerTPSController : MonoBehaviour
{
    public UnityEvent onInteractionInput;
    public Camera cam;
    private InputData inputData;
    private CharacterAnimBasedMovement characterMovement;

    public bool onInteractionZone{get;set;}
    void Start()
    {
        characterMovement = GetComponent<CharacterAnimBasedMovement>();
    }
    
    void Update()
    {
      
        inputData.GetInput();

        if(onInteractionZone){
            onInteractionInput.Invoke();
        }

        characterMovement.Gestos(inputData.dance1,inputData.dance2, inputData.prank);
        characterMovement.MoveCharacter(inputData.hMovement, inputData.vMovement, cam, inputData.jump, inputData.dash);
    }
}
