/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Carrarini Andrea
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using UnityEngine;

public class BiteTriggerable : MonoBehaviour
{
    [HideInInspector] public int biteDamage = 1;                         // Set the number of hitpoints that this gun will take away from shot objects with a health script.
    [HideInInspector] public float range = 30f;                   // Distance in unity units over which the player can fire.
    [HideInInspector] public float hitForce = 100f;                     // Amount of force which will be added to objects with a rigidbody shot by the player.
    //public Transform gunEnd;                                            // Holds a reference to the gun end object, marking the muzzle location of the gun.
    //[HideInInspector] public LineRenderer laserLine;                    // Reference to the LineRenderer component which will display our laserline.

    private Camera fpsCam;                                              // Holds a reference to the first person camera.
    private WaitForSeconds shotDuration = new WaitForSeconds(.07f);     // WaitForSeconds object used by our ShotEffect coroutine, determines time laser line will remain visible.


    public void Initialize()
    {
        //Get and store a reference to our LineRenderer component
        //laserLine = GetComponent<LineRenderer>();

        //Get and store a reference to our Camera
        //fpsCam = GetComponentInParent<Camera>();
    }

    public void Bite()
    {

    }

    /*public void Fire()
    {

        //Create a vector at the center of our camera's near clip plane.
        Vector3 rayOrigin = fpsCam.ViewportToWorldPoint(new Vector3(.5f, .5f, 0));

        //Draw a debug line which will show where our ray will eventually be
        Debug.DrawRay(rayOrigin, fpsCam.transform.forward * range, Color.green);

        //Declare a raycast hit to store information about what our raycast has hit.
        RaycastHit hit;

        //Start our ShotEffect coroutine to turn our laser line on and off
        StartCoroutine(ShotEffect());

        //Set the start position for our visual effect for our laser to the position of gunEnd
        laserLine.SetPosition(0, gunEnd.position);

        //Check if our raycast has hit anything
        if (Physics.Raycast(rayOrigin, fpsCam.transform.forward, out hit, range))
        {
            //Set the end position for our laser line 
            laserLine.SetPosition(1, hit.point);

            //Get a reference to a health script attached to the collider we hit
            ShootableBox health = hit.collider.GetComponent<ShootableBox>();

            //If there was a health script attached
            if (health != null)
            {
                //Call the damage function of that script, passing in our biteDamage variable
                health.Damage(biteDamage);
            }

            //Check if the object we hit has a rigidbody attached
            if (hit.rigidbody != null)
            {
                //Add force to the rigidbody we hit, in the direction it was hit from
                hit.rigidbody.AddForce(-hit.normal * hitForce);
            }
        }
        else
        {
            //if we did not hit anything, set the end of the line to a position directly away from
            laserLine.SetPosition(1, fpsCam.transform.forward * range);
        }
    } */

    /*private IEnumerator ShotEffect()
    {

        //Turn on our line renderer
        laserLine.enabled = true;
        //Wait for .07 seconds
        yield return shotDuration;

        //Deactivate our line renderer after waiting
        laserLine.enabled = false;
    }
    */
}