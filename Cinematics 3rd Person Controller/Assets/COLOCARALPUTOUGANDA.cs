using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class COLOCARALPUTOUGANDA : MonoBehaviour
{
    public GameObject ALIENUGANDA;
    public Vector3 pos;
    void Start()
    {
        ALIENUGANDA.transform.position = pos;
    }

}
