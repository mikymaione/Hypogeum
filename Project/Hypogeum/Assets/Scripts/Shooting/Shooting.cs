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
using UnityEngine.UI;

public class Shooting : NetworkBehaviour
{

    public GameObject projectilePrefab;

    private Bullet projectileClass;
    private bool canShoot = true;

    private Vector3 target;
    private Camera cam;
    private CameraManager cameraManager;
    private Transform cannonPositionMarker, CameraPos;

    //The class that owns the stats of the faction    
    private GeneralCar generalCar;
    private HudScriptManager HUD;


    public override void OnStartLocalPlayer()
    {
        cam = Camera.main;
        cameraManager = cam.GetComponent<CameraManager>();
        projectileClass = projectilePrefab.GetComponent<Bullet>();
        CameraPos = transform.Find("CameraPos");

        var HUDo = GameObject.FindGameObjectWithTag("HUD");
        HUD = HUDo.GetComponent<HudScriptManager>();

        MostraMirino();
    }

    private GameObject ___car;
    private GameObject Car
    {
        get
        {
            if (___car == null)
            {
                var factionCarName = $"{GB.Animal.ToString()}sCar";
                var cars = GameObject.FindGameObjectsWithTag("car");

                foreach (var car in cars)
                    if (car.name.Equals($"{factionCarName}(Clone)"))
                        ___car = car;
            }

            return ___car;
        }
    }

    void Update()
    {
        if (isLocalPlayer)
        {
            if (generalCar == null)
                generalCar = Car?.GetComponent<GeneralCar>();

            if (cannonPositionMarker == null)
                cannonPositionMarker = Car?.transform.Find("CannonPosition");

            if (cannonPositionMarker != null)
                PlaceAndRotateCannon();

            target = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));

            if (canShoot && Input.GetMouseButtonDown(0))
            {
                canShoot = false;
                StartCoroutine(RechargeWeapon());

                var velocity = cam.transform.forward * projectileClass.speed;
                CmdIstantiateBulletAndShoot(GB.Animal.Value, gameObject, cam.transform.position, cam.transform.rotation, velocity);
            }

            HUD.setValues(generalCar);
        }
    }

    [ClientRpc] //all clients
    private void RpcShoot(GameObject projectile, Vector3 velocity)
    {
        var projectile_RB = projectile.GetComponent<Rigidbody>();
        projectile_RB.velocity = velocity;
    }

    [Command] //only host
    private void CmdIstantiateBulletAndShoot(GB.EAnimal animal, GameObject player, Vector3 position, Quaternion rotation, Vector3 velocity)
    {
        var projectile = Instantiate(projectilePrefab, position, rotation);
        var bulletClass = projectile.GetComponent<Bullet>();
        bulletClass.AnimaleCheHaSparatoQuestoColpo = animal;

        Destroy(projectile, 10);
        NetworkServer.SpawnWithClientAuthority(projectile, player);

        RpcShoot(projectile, velocity);
    }

    //Instantiate the explosion prefab
    private void PlayExplosion()
    {

    }

    private IEnumerator RechargeWeapon()
    {
        yield return new WaitForSeconds(0.999f);
        canShoot = true;
        StopCoroutine(RechargeWeapon());
    }

    //place and rotate the cannon along with the camera on axis Y
    private void PlaceAndRotateCannon()
    {
        transform.rotation = Quaternion.Euler(0, cameraManager.rotY, 0);
        transform.position = cannonPositionMarker.position;
        cam.transform.position = CameraPos.position;

        CmdSetRotationAndPositionOfCannon_onServer(gameObject.name, cameraManager.rotY);
    }

    [Command] //only host
    private void CmdSetRotationAndPositionOfCannon_onServer(string NomeCannone, float rotY)
    {
        RpcSetRotationAndPositionOfCannon_onClient(NomeCannone, rotY);
    }

    [ClientRpc] //all clients
    private void RpcSetRotationAndPositionOfCannon_onClient(string NomeCannone, float rotY)
    {
        var cannons = GameObject.FindGameObjectsWithTag("Cannon");

        foreach (var cannon in cannons)
            if (cannon.name.Equals(NomeCannone))
                cannon.transform.rotation = Quaternion.Euler(0, rotY, 0);
    }

    private void MostraMirino()
    {
        var oggetti = gameObject.scene.GetRootGameObjects();

        foreach (var o in oggetti)
            if (o.CompareTag("Canvas"))
            {
                var Mirino = o.GetComponentInChildren<Image>();

                if (Mirino != null)
                    Mirino.enabled = true;

                break;
            }
    }


}