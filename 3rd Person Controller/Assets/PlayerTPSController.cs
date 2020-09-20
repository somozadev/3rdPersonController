using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTPSController : MonoBehaviour
{
    public Camera cam;
    private InputData inputData;
    private CharacterAnimBasedMovement characterMovement;

    void Start()
    {
        characterMovement = GetComponent<CharacterAnimBasedMovement>();
    }
    
    void Update()
    {
      
        inputData.GetInput();
        characterMovement.MoveCharacter(inputData.hMovement, inputData.vMovement, cam, inputData.jump, inputData.dash);
    }
}
