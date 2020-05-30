using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraControl : MonoBehaviour
{
    //Variables
    public float Speed = 5;
    public GameObject Target;
    private Vector3 FinalPos;

    //Camera movement following Player -> Target
    private void FixedUpdate()
    {
        if (Target.transform.parent == null)
        {
            FinalPos = new Vector3(Target.transform.position.x + 6, transform.position.y, -12);
            transform.position = Vector3.Lerp(transform.position, FinalPos, Speed * Time.deltaTime);
        }
    }
}
