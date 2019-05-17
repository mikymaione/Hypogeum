using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager1 : NetworkManager
{
	public CharacterSelector characterSelector;
	public Character[] characters;
	private Character selectedCharacter;

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		characterSelector = GameObject.FindGameObjectWithTag("UI").GetComponent<CharacterSelector>();
		playerPrefab = Instantiate(Resources.Load(characterSelector.serverFactionName)) as GameObject;
		Debug.Log("Player name: " + playerPrefab.name);

		var player = (GameObject)GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

		var coolDownButtons = GetComponentsInChildren<AbilityCoolDown>();
		//var selectedCharacter = characters[characterChoice];

		for (var i = 0; i < coolDownButtons.Length; i++)
		{
			/*
			 * if you don't create the abilities in scriptable objects and assign them to the character in unity
			 * it goes out of range
			 */
			coolDownButtons[i].Initialize(selectedCharacter.characterAbilities[i], player);
		}

		Debug.Log("Client has requested to get his player added to the game");

	}

	public void SetCharacters(int characterChoice)
	{
		selectedCharacter = characters[characterChoice];
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
