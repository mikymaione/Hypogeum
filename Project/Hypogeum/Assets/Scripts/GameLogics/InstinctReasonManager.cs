using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InstinctReasonManager : NetworkBehaviour
{

	private GameObject none, instinct, reason, damage, control;

	//private GameObject reasonCoin;

    // Start is called before the first frame update
    void Start()
    {
		none = GameObject.Find("None");
		instinct = GameObject.Find("Instinct");
		reason = GameObject.Find("Reason");
		damage = GameObject.Find("Damage");
		control = GameObject.Find("Control");

		instinct.SetActive(false);
		reason.SetActive(false);
		damage.SetActive(false);
		control.SetActive(false);

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

			//Changing buttons image
			none.SetActive(false);
			instinct.SetActive(true);

			//Showing power-up
			damage.SetActive(true);

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

			//Changing buttons image
			none.SetActive(false);
			instinct.SetActive(true);

			//Showing power-up
			damage.SetActive(true);
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

			//Changing buttons image
			none.SetActive(false);
			reason.SetActive(true);

			//Showing power-up
			control.SetActive(true);

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

			//Changing buttons image
			none.SetActive(false);
			reason.SetActive(true);

			//Showing power-up
			control.SetActive(true);
		}
	}

}
