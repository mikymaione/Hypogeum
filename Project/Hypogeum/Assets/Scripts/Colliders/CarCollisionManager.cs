/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Carrarini Andrea
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using Prototype.NetworkLobby;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class CarCollisionManager : NetworkBehaviour
{

    private GeneralCar playerCar;
    private Rigidbody playerCar_RB;
    private int otherCarDefense;
    private float relativeCollisionVelocity;


    public override void OnStartLocalPlayer()
    {
        playerCar = GetComponent<GeneralCar>();
        playerCar_RB = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("car"))
        {
            otherCarDefense = collision.gameObject.GetComponent<GeneralCar>().Defense;
            relativeCollisionVelocity = collision.relativeVelocity.magnitude;
            CalculateCollisionDamage(otherCarDefense, relativeCollisionVelocity);
        }
        else if (collision.gameObject.CompareTag("Bullet"))
        {
            var bullet = collision.gameObject.GetComponent<Bullet>();

            //grazie Michele, lo avevo pensato adesso, ma lo hai gia' impostato tu <3
            if (bullet.AnimaleCheHaSparatoQuestoColpo == GB.Animal)
            {
                //mi sono sparato da solo, no damage
                CalculateBulletDamage(0);
            }
            else
            {
                int otherTeamAttack = GB.GetTeamAttack(bullet.AnimaleCheHaSparatoQuestoColpo);
                CalculateBulletDamage(otherTeamAttack);
            }
        }
    }

    private void CalculateBulletDamage(int otherTeamAttack)
    {
        float damage = otherTeamAttack * 8;

        CmdTakeDamage(GB.Animal.Value, gameObject, damage);
    }

    private void CalculateCollisionDamage(int otherCarDefense, float relativeCollisionVelocity)
    {
        if (playerCar != null)
        {
            float damage = (relativeCollisionVelocity) / (playerCar.Defense * 0.1f);

            CmdTakeDamage(GB.Animal.Value, gameObject, damage);
        }
    }

    [Command] //only host
    private void CmdTakeDamage(GB.EAnimal animale, GameObject player, float damage)
    {
        var p = player.GetComponent<GeneralCar>();

        p.Health -= damage;

        if (p.Health <= 0)
            foreach (var lobbyPlayer in LobbyManager.s_Singleton.Players)
                if (lobbyPlayer.Value.animal == animale)
                    RpcTeamRemoved(lobbyPlayer.Value.animal);
    }

    [ClientRpc] //all clients
    private void RpcTeamRemoved(GB.EAnimal animale)
    {
        if (GB.Animal == animale)
        {
            Destroy(gameObject);

            var loss = GameObject.FindGameObjectWithTag("Loss");
            loss.SetActive(true);
            StartCoroutine(EsciDalGioco());
        }
        else
        {
            //conta players rimasti in campo
            //var win = GameObject.FindGameObjectsWithTag("Win");
            //win.SetActive(true);
            //StartCoroutine(EsciDalGioco());
        }
    }

    private IEnumerator EsciDalGioco()
    {
        yield return new WaitForSeconds(2);

        Cursor.visible = true;
        GB.GotoScene(GB.EScenes.StartTitle);
    }


}