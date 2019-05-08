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

    public float speed = 20;
    public GameObject projectilePrefab;

    private bool canShoot = true;


    void Update()
    {
        if (isLocalPlayer)
        {
            if (canShoot && Input.GetMouseButtonDown(0))
            {
                canShoot = false;

                var velocity = transform.TransformDirection(Vector3.forward * speed);
                CmdShoot(gameObject, transform.position, transform.rotation, velocity);

                StartCoroutine(RechargeWeapon());
            }
        }
    }

    [ClientRpc]
    private void RpcAggiungiVelocita(GameObject projectile, Vector3 velocity)
    {
        var projectile_RB = projectile.GetComponent<Rigidbody>();
        projectile_RB.velocity = velocity;
    }

    [Command] //host
    private void CmdShoot(GameObject player, Vector3 position, Quaternion rotation, Vector3 velocity)
    {
        var projectile = Instantiate(projectilePrefab, position, rotation);
        Destroy(projectile, 10);
        NetworkServer.SpawnWithClientAuthority(projectile, player);
        RpcAggiungiVelocita(projectile, velocity);
    }

    private IEnumerator RechargeWeapon()
    {
        yield return new WaitForSeconds(0.5f);
        canShoot = true;
        StopCoroutine(RechargeWeapon());
    }

}