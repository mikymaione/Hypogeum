/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Carrarini Andrea
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using UnityEngine;

/* creates a wind shield around the car deflecting incoming projectiles
 * it is the "WindAbility" GameObject in EaglesCar, it has a boxCollider to enable when using the ability
 * since the WindAbility BoxCollider is not much greater than the car normal collider, 
 * it should be good to use it as the collision collider for collisions 
 * */
public class WindTriggerable : MonoBehaviour
{

	[HideInInspector]
	public int biteDamage = 1;                         // Set the number of hitpoints that this gun will take away from shot objects with a health script.

	[HideInInspector]
	public float range = 30f;                   // Distance in unity units over which the player can fire.

	[HideInInspector]
	public float hitForce = 100f;

	public BoxCollider windBoxCollider; // private?

    public void Initialize()
    {
        var WindShield = GameObject.FindGameObjectWithTag("WindShield");
        windBoxCollider = WindShield.GetComponent<BoxCollider>();
    }

    public void ActivateWind()
    {
        windBoxCollider.enabled = true;
    }

    public void Update()
    {
		
    }

}