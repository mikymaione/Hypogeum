/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Maione Michele
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class Shooting : NetworkBehaviour
{

    public GameObject projectilePrefab;

    private Bullet projectileClass;
    private bool canShoot = true;

    private Vector3 target;
    private Camera cam;
	private CameraManager cameraManager;
	private Transform cannonPosition;

    void Start()
    {
        cam = Camera.main;
		cameraManager = cam.GetComponent<CameraManager>();
        projectileClass = projectilePrefab.GetComponent<Bullet>();
		//Debug.Log($"CannonFaction coordinates are: {cannonPosition.position.ToString()}");
		//cannonPosition = factionCar.transform.Find("CannonPosition");
		
    }

    void Update()
    {
        if (isLocalPlayer)
        {
			if (cannonPosition == null)
			{
				var factionCarName = $"{GB.Animal.ToString()}sCar";
				var factionCar = GameObject.FindGameObjectWithTag(factionCarName);
				cannonPosition = factionCar?.transform.Find("CannonPosition");
				//cannonPosition = Helper.FindComponentInChildWithTag<Transform>(factionCar, "CannonPosition");
			}

            target = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));

			PlaceAndRotateCannon();

            if (canShoot && Input.GetMouseButtonDown(0))
            {
                canShoot = false;
                StartCoroutine(RechargeWeapon());

                var velocity = cam.transform.forward * projectileClass.speed;
                CmdIstantiateBulletAndShoot(gameObject, cam.transform.position, cam.transform.rotation, velocity);
            }
        }
    }

    [ClientRpc] //all clients
    private void RpcShoot(GameObject projectile, Vector3 velocity)
    {
        var projectile_RB = projectile.GetComponent<Rigidbody>();
        projectile_RB.velocity = velocity;
    }

    [Command] //only host
    private void CmdIstantiateBulletAndShoot(GameObject player, Vector3 position, Quaternion rotation, Vector3 velocity)
    {
        var projectile = Instantiate(projectilePrefab, position, rotation);

        //Destroy(projectile, 10); //vi pregooooo voglio macerie ovunque
        NetworkServer.SpawnWithClientAuthority(projectile, player);

        RpcShoot(projectile, velocity);
    }

    private IEnumerator RechargeWeapon()
    {
        yield return new WaitForSeconds(0.5f);
        canShoot = true;
        StopCoroutine(RechargeWeapon());
    }

	//place and rotate the cannon along with the camera on axis Y
	private void PlaceAndRotateCannon()
	{
		var localRotation = Quaternion.Euler(0, cameraManager.rotY, 0);

		if (cannonPosition != null)
		{
			cam.transform.position = cannonPosition.position;
			transform.position = cannonPosition.position;
			Debug.Log($"CannonFaction coordinates are: {cannonPosition.position.ToString()}");
		}

		transform.rotation = localRotation;
	}

}