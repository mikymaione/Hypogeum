/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Carrarini Andrea
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using UnityEngine;
using UnityEngine.Networking;

public class CarCollisionManager : NetworkBehaviour
{

    private GeneralCar playerCar;
    private Rigidbody playerCar_RB;
    private AudioSource audioSource;
    private AudioClip carCollisionAudioClip;


    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    public override void OnStartLocalPlayer()
    {
        playerCar = GetComponent<GeneralCar>();
        playerCar_RB = GetComponent<Rigidbody>();
        carCollisionAudioClip = Resources.Load("Audio/CarCollision") as AudioClip;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isLocalPlayer)
        {
            if (collision.gameObject.CompareTag("car"))
            {
                if (playerCar != null)
                {
                    //TODO must be networked to both team mates
                    audioSource.PlayOneShot(carCollisionAudioClip);

                    var damage = collision.relativeVelocity.magnitude / (playerCar.Defense * 0.1f);

                    CmdTakeDamage(GB.Animal.Value, gameObject, damage);
                }
            }
            else if (collision.gameObject.CompareTag("Bullet"))
            {
                var bullet = collision.gameObject.GetComponent<Bullet>();

                if (bullet.AnimaleCheHaSparatoQuestoColpo != GB.Animal)
                {
                    var otherTeamAttack = 8 * GB.GetTeamAttack(bullet.AnimaleCheHaSparatoQuestoColpo);                    

                    CmdTakeDamage(GB.Animal.Value, gameObject, otherTeamAttack);
                }
            }
        }
    }

    [Command] //only host
    private void CmdTakeDamage(GB.EAnimal animale, GameObject player, float damage)
    {
        var p = player.GetComponent<GeneralCar>();
        p.Health -= damage;
    }


}