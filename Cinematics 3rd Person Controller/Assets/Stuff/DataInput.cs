using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct InputData
{
    public float hMovement;
    public float vMovement;

    public float verticalMouse;
    public float horizontalMouse;

    public bool dash;
    public bool jump;
    public bool dance1;
    public bool dance2;
    public bool prank;

    public void GetInput()
    {
        hMovement = Input.GetAxis("Horizontal");
        vMovement = Input.GetAxis("Vertical");

        verticalMouse = Input.GetAxis("Mouse Y");
        horizontalMouse = Input.GetAxis("Mouse X");

        dash = Input.GetButton("Dash");
        jump = Input.GetButtonDown("Jump");
        dance1 = Input.GetButton("dance1"); 
        dance2 = Input.GetButton("dance2");
        prank = Input.GetButton("prank");
    }



}