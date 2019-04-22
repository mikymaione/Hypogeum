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

    public float thrust;
    public Rigidbody rb;
    public float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        try
        {
            var partite = HdbC.ListaPartite(System.DateTime.Now.AddMonths(-1), System.DateTime.Now);

            foreach (var p in partite)
            {

            }
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
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            rb.AddForce(0, 0, -20f);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            rb.AddForce(0, 0, 20f);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            rb.AddForce(speed * -6, 0, 0);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            rb.AddForce(speed * 6, 0, 0);
        }
    }

}