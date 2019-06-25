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

public class GM : NetworkBehaviour
{

    private GameObject[] cars, cannons;

    public SyncListInt AnimaliMorti = new SyncListInt();

    [SyncVar]
    public int NumeroAnimaliVistiVivi = 0;

    private int NumeroCannoniMatchati = 0;

    private bool FineGioco = false;


    void FixedUpdate()
    {
        if (isServer)
        {
            cars = GameObject.FindGameObjectsWithTag("car");
            cannons = GameObject.FindGameObjectsWithTag("Cannon");

            if (NumeroAnimaliVistiVivi < cars.Length)
                NumeroAnimaliVistiVivi = cars.Length;

            if (Prototype.NetworkLobby.LobbyManager.s_Singleton.Players.Count > NumeroCannoniMatchati)
                server_MatchCannoni();

            server_GestioneVite();
        }
    }

    [ClientRpc] //tutti i client
    private void RpcSpegnetevi()
    {
        StartCoroutine(EsciDalGioco());
    }

    private IEnumerator EsciDalGioco()
    {
        yield return new WaitForSeconds(8);
        StopCoroutine(EsciDalGioco());

        Cursor.visible = true;
        GB.GotoScene(GB.EScenes.StartTitle);
    }

    void server_GestioneVite()
    {
        foreach (var car in cars)
        {
            var gc = car.GetComponent<GeneralCar>();

            if (gc.Health <= 0)
            {
                var a = (int)gc.AnimalType;

                if (!AnimaliMorti.Contains(a))
                {
                    AnimaliMorti.Add(a);

                    foreach (var cannon in cannons)
                    {
                        var sh = cannon.GetComponent<Shooting>();

                        if (sh.TipoDiArma == gc.AnimalType)
                            Destroy(cannon);
                    }

                    Destroy(car);
                }
            }
        }

        if (!FineGioco)
            if (NumeroAnimaliVistiVivi == AnimaliMorti.Count + 1)
            {
                FineGioco = true;
                RpcSpegnetevi();
            }
    }

    void server_MatchCannoni()
    {
        if (cannons.Length > 0)
            foreach (var car in cars)
            {
                var gc = car.GetComponent<GeneralCar>();

                if (string.IsNullOrEmpty(gc.MyCannonName))
                    foreach (var cannon in cannons)
                    {
                        var sh = cannon.GetComponent<Shooting>();

                        if (sh.TipoDiArma == gc.AnimalType && string.IsNullOrEmpty(sh.CarName))
                        {
                            //matched!                            
                            gc.MyCannonName = cannon.name;
                            sh.CarName = car.name;
                            NumeroCannoniMatchati++;
                        }
                    }
            }
    }


}