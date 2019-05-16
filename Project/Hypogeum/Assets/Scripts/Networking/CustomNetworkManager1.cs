using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager1 : NetworkManager
{
	public CharacterSelector characterSelector = GameObject.FindGameObjectWithTag("UI").GetComponent<CharacterSelector>();

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
	{
		playerPrefab = Instantiate(Resources.Load(characterSelector.serverFactionName)) as GameObject;

		var player = (GameObject)GameObject.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);

		NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);

		Debug.Log("Client has requested to get his player added to the game");

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
