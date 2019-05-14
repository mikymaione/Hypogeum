using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomNetworkManager : NetworkManager
{
	public int chosenCharacter = 0;

	//subclass for sending network messages
	public class NetworkMessage : MessageBase
	{
		public int chosenClass;
	}

	public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
	{
		NetworkMessage message = extraMessageReader.ReadMessage<NetworkMessage>();
		int selectedClass = message.chosenClass;
		Debug.Log("server add with message " + selectedClass);

		if (selectedClass == 0)
		{
			GameObject player = Instantiate(Resources.Load("EaglesCar", typeof(GameObject))) as GameObject;
			NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
		}

		if (selectedClass == 1)
		{
			GameObject player = Instantiate(Resources.Load("RhinosCar", typeof(GameObject))) as GameObject;
			NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
		}
	}

	public override void OnClientConnect(NetworkConnection conn)
	{
		NetworkMessage test = new NetworkMessage();
		test.chosenClass = chosenCharacter;

		ClientScene.AddPlayer(conn, 0, test);
	}


	public override void OnClientSceneChanged(NetworkConnection conn)
	{
		//base.OnClientSceneChanged(conn);
	}

	public void btn1()
	{
		chosenCharacter = 0;
	}

	public void btn2()
	{
		chosenCharacter = 1;
	}
}
