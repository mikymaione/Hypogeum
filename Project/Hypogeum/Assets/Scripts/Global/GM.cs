/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Maione Michele
Contributors: Andrea Carrarini
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using UnityEngine;

public class GM : MonoBehaviour
{
	private GeneralCar generalCar;
	private GB.EAnimal animal;

	void Start()
	{
		generalCar = GameObject.FindGameObjectWithTag("car").GetComponent<AutoGuida>().generalCar;
		//test
		generalCar.Health = 0;
		//RemoveTeam(GB.EAnimal.Lion);
	}

	void Update()
	{
		if (!StillAlive())
		{
			//TODO call to function that disconnects the 2 players of the same team from the game in safe way
			//must call the server that has to send a msg to clients, notifying that all players in the same team
			//TeamHasBeenDefeated();
			RemoveTeam(GB.EAnimal.Lion);
		}
	}

	//Check if the team's car has health left
	public bool StillAlive()
	{
		if (generalCar.Health <= 0)
		{
			return false;
		}
		else return true;
	}

	
	public void RemoveTeam(GB.EAnimal animal)
	{
		foreach (var lobbyPlayer in Prototype.NetworkLobby.LobbyManager.s_Singleton.Players)
		{
			if (lobbyPlayer.Value.animal == animal)
			{
				Prototype.NetworkLobby.LobbyManager.s_Singleton.RemovePlayer(lobbyPlayer.Value);
			}
		}

		//foreach (var p in Prototype.NetworkLobby.LobbyPlayerList._instance._players)
		//{
		//	switch (p.animal)
		//	{
		//		case GB.EAnimal.Eagle:
		//			break;
		//		case GB.EAnimal.Lion:
		//			break;
		//		case GB.EAnimal.Rhino:
		//			break;
		//		case GB.EAnimal.Shark:
		//			break;
		//	}
		//}
	}
   
}