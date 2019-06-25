using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class RadarManager : NetworkBehaviour
{
	private GameObject[] cars;

    // Start is called before the first frame update
    void Start()
    {
		cars = GameObject.FindGameObjectsWithTag("car");
		for (int i = 0; i < cars.Length; i++)
		{
			if (GB.Animal.Value == cars[i].GetComponent<GeneralCar>().AnimalType)
			{
				cars[i].transform.Find("Sphere").GetComponent<MeshRenderer>().enabled = false;
			}
			else
			{
				cars[i].transform.Find("cono").GetComponent<MeshRenderer>().enabled = false;
			}
		}

		var rawImage = gameObject.GetComponentInChildren<RawImage>().texture = GB.getAnimalRenderTexture(GB.Animal.Value, GB.GameType.Value);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
