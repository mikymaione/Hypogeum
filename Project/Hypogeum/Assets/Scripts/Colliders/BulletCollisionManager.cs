/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Michele Maione
Contributors: Carrarini Andrea
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using UnityEngine;
using UnityEngine.Networking;

public class BulletCollisionManager : NetworkBehaviour
{
    public AudioSource audioSource;

    public AudioClip ExplosionSound;

    public GameObject explosion, sand;

    private bool hasExploded = false;


    private void OnCollisionEnter(Collision collision)
    {
        if (!hasExploded)
        {
            if (collision.gameObject.CompareTag("Arena"))
            {
                var _sand = Instantiate(sand, transform.position, Quaternion.Euler(-90, 0, 0));

                Destroy(gameObject, 5); //eliminare dopo 5 secondi così rimbalza e fa un altro po' di strada
                Destroy(_sand, 10);
            }
            else
            {
                hasExploded = true;

                var _explosion = Instantiate(explosion, transform.position, transform.rotation);
                _explosion.name += Time.time.ToString();

                gameObject.GetComponent<AudioSource>().PlayOneShot(ExplosionSound);

                Destroy(gameObject); //eliminare subito perché il proiettile è esploso, altrimenti vedo l'esplosione ma il proiettile continua ad andare
                Destroy(_explosion, 3.8f);
            }
        }
    }


}