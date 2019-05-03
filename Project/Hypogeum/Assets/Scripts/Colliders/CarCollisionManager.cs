using System.Collections;
using System.Collections.Generic;
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
