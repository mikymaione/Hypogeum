using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* creates a wind shield around the car deflecting incoming projectiles
 * it is the "WindAbility" GameObject in EaglesCar, it has a boxCollider to enable when using the ability
 * since the WindAbility BoxCollider is not much greater than the car normal collider, 
 * it should be good to use it as the collision collider for collisions 
 * */
public class WindTriggerable : MonoBehaviour
{

    public BoxCollider windBoxCollider;

    public void Initialize()
    {
        windBoxCollider = GameObject.FindGameObjectWithTag("WindShield").GetComponent<BoxCollider>();
    }

    public void ActivateWind()
    {
        windBoxCollider.enabled = true;
    }

    public void Update()
    {
		
    }
}
