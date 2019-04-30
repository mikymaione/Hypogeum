using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkingController : Controller
{

    Vector3 walkVelocity;
    public float walkSpeed = 3f;

    public override void ReadInput(InputData data)
    {
        walkVelocity = Vector3.zero;

        //set vertical movement
        if (data.axis[0] != 0f)
            walkVelocity += Vector3.forward * data.axis[0] * walkSpeed;

        //set horizontal movement
        if (data.axis[1] != 0f)
            walkVelocity += Vector3.forward * data.axis[1] * walkSpeed;

        newInput = true;
    }

    //executed after all the updates, we need to stop moving
    void LateUpdate()
    {
        if (!newInput)
            walkVelocity = Vector3.zero;

        //rb.velocity.y instead of anything (it will always be 0) because of gravity
        rb.velocity = new Vector3(walkVelocity.x, rb.velocity.y, walkVelocity.z);
        newInput = false;
    }

}
