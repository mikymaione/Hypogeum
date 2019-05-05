/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Carrarini Andrea
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using UnityEngine;

public class CarCollisionManager : MonoBehaviour
{
    private Car playerCar;

    public void SetPlayerCar()
    {
        playerCar = GetComponent<Car>();
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "IlMezzo(Clone)")
        {
            Debug.Log("Collision with: " + collision.gameObject.name);
            CalculateCollisionDamage(collision);
            Debug.Log("Health left: " + playerCar.Health);
        }

        if (collision != null)
        {
            Debug.Log("Collision with: " + collision.gameObject.name);
        }
    }

    public void CalculateCollisionDamage(Collision collision)
    {
        //not considering a minimum speed threshold might be bad
        playerCar.Health -= 10 * (collision.gameObject.GetComponent<Car>().Defense);
    }
}