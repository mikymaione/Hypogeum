/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Maione Michele
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using HypogeumDBConnector;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : MonoBehaviour
{
    private readonly KeyCode[] arrows = { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.DownArrow, KeyCode.UpArrow };

    void Start()
    {
        try
        {
            var iniziata = HdbC.IniziaPartita("awjskajsa");
            var s = HdbC.StatoPartita("asjsakjsa");
            var partite = HdbC.ListaPartite(System.DateTime.Now.AddMonths(-1), System.DateTime.Now);
        }
        catch (System.Exception ex)
        {
            var er = ex.Message;
        }
    }

    void Update()
    {
        if (GetComponent<NetworkIdentity>().isLocalPlayer)
            if (Input.anyKey)
                if (isArraowKeysPressed())
                {
                    ArrowKeys();
                }
                else
                {

                }
    }

    private bool isArraowKeysPressed()
    {
        foreach (var arrow in arrows)
            if (Input.GetKeyDown(arrow))
                return true;

        return false;
    }

    private void ArrowKeys()
    {
        var position = this.transform.position;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            position.x--;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            position.x++;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            position.y++;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            position.y--;
        }

        this.transform.position = position;
    }

}