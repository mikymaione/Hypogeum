/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Carrarini Andrea
Contributors: Maione Michele
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using UnityEngine;
using UnityEngine.Networking;

public class InstinctReasonManager : NetworkBehaviour
{

    public GameObject none, instinct, reason, damage, control;


    private AudioSource _audioSource;
    private AudioSource audioSource
    {
        get
        {
            if (_audioSource == null)
                _audioSource = gameObject.GetComponent<AudioSource>();

            return _audioSource;
        }
    }


    [Command] //solo host
    public void Cmd_server_OnCoinChosed(GB.EAnimal animal, GB.ECoin tipo, GameObject coin)
    {
        //dici a tutti i client che l'animal ha preso un coin
        Rpc_client_OnCoinChosed(animal, tipo, coin);
    }

    [ClientRpc] //tutti i client
    public void Rpc_client_OnCoinChosed(GB.EAnimal animal, GB.ECoin tipo, GameObject coin)
    {
        if (GB.Animal.Value == animal)
        {
            Destroy(coin);
            client_AttivaIR(tipo);
            audioSource.Play();
        }
    }

    private void client_AttivaIR(GB.ECoin IR)
    {
        var I = (IR == GB.ECoin.Instinct);

        //Changing buttons image
        none.SetActive(false);
        instinct.SetActive(I);
        reason.SetActive(!I);

        //Showing power-up
        damage.SetActive(I);
        control.SetActive(!I);
    }


}