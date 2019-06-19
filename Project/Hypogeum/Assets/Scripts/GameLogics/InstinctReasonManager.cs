using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InstinctReasonManager : NetworkBehaviour
{

	//private GameObject instinctCoin;
	//private GameObject reasonCoin;

    // Start is called before the first frame update
    void Start()
    {
		//instinctCoin = GameObject.Find("CoinInstinct(Clone)");
		//reasonCoin = GameObject.Find("CoinReason(Clone)");
    }

    // Update is called once per frame
    void Update()
    {
        
    }



	[Command]
	public void CmdOnInstinctChosen(GB.EAnimal animal)
	{
		if (GB.Animal.Value == animal)
		{
			Destroy(GameObject.Find("CoinReason(Clone)"));
			//assegna obiettivo al player
			Destroy(GameObject.Find("CoinInstinct(Clone)"));
			RpcOnInstinctChosen(animal);
		}
	}

	[ClientRpc]
	public void RpcOnInstinctChosen(GB.EAnimal animal)
	{
		if (GB.Animal.Value == animal)
		{
			Destroy(GameObject.Find("CoinReason(Clone)"));
			//assegna obiettivo al player
			Destroy(GameObject.Find("CoinInstinct(Clone)"));
			Debug.Log("scelto istinto");
		}
	}

	[Command]
	public void CmdOnReasonChosen(GB.EAnimal animal)
	{
		if (GB.Animal.Value == animal)
		{
			Destroy(GameObject.Find("CoinInstinct(Clone)"));
			//assegna obiettivo al player
			Destroy(GameObject.Find("CoinReason(Clone)"));
			RpcOnReasonChosen(animal);
		}
	}

	[ClientRpc]
	public void RpcOnReasonChosen(GB.EAnimal animal)
	{
		if (GB.Animal.Value == animal)
		{
			Destroy(GameObject.Find("CoinInstinct(Clone)"));
			//assegna obiettivo al player
			Destroy(GameObject.Find("CoinReason(Clone)"));
		}
	}

}
