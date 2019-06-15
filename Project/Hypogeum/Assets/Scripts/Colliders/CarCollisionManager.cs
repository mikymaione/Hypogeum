/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Carrarini Andrea
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using Prototype.NetworkLobby;
using UnityEngine;
using UnityEngine.Networking;

public class CarCollisionManager : NetworkBehaviour
{

    private GeneralCar playerCar;
    private Rigidbody playerCar_RB;


    public override void OnStartLocalPlayer()
    {
        playerCar = GetComponent<GeneralCar>();
        playerCar_RB = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("car"))
        {
            CalculateCollisionDamage();
        }
        else if (collision.gameObject.CompareTag("Bullet"))
        {
            var bullet = collision.gameObject.GetComponent<Bullet>();

            if (bullet.AnimaleCheHaSparatoQuestoColpo == GB.Animal)
            {
                //mi sono sparato da solo
            }
            else
            {
                CalculateCollisionDamage(100);
            }
        }
    }

    private void CalculateCollisionDamage(int AdditionalDamage = 0)
    {
        if (playerCar != null)
        {
            var v = AdditionalDamage + (playerCar_RB.velocity.magnitude * 5 - playerCar.Defense);

            CmdTakeDamage(GB.Animal.Value, gameObject, v);
        }
    }

    [Command] //only host
    private void CmdTakeDamage(GB.EAnimal animale, GameObject player, float v)
    {
        var p = player.GetComponent<GeneralCar>();

        p.Health -= v;

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
            Cursor.visible = true;

            LobbyManager.s_Singleton.infoPanel.Display("Sei muerto!", "Chiudi", () =>
            {
                GB.GotoScene(GB.EScenes.StartTitle);
            });
        }
    }


}