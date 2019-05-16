/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Carrarini Andrea
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;

public class CharacterSelector : NetworkBehaviour
{
    public GameObject player;
    public Vector3 playerSpawnPosition = new Vector3(0, 1, -7);
    public Character[] characters;

    public GameObject characterSelectPanel;
    public GameObject abilityPanel;
	public NetworkManager networkingComponent;
	//public GameObject chosenFactionCar;

	public string serverFactionName;
	public string clientFactionName;

	public string getFactionName(int characterChoice)
	{
		string factionName;

		switch (characterChoice)
		{
			case 0:
				factionName = "EaglesCar";
				return factionName;
			case 1:
				factionName = "RhinosCar";
				return factionName;
		}
		return null;
	}

	//[Command]
	//private void foo(string factionName)
	//{
	//	networkingComponent = GameObject.FindGameObjectWithTag("Networking").GetComponent<NetworkManager>();
	//	player = Instantiate(Resources.Load(factionName)) as GameObject;
	//	networkingComponent.playerPrefab = player;
	//}

    public void OnCharacterSelect(int characterChoice)
    {
        characterSelectPanel.SetActive(false);
        abilityPanel.SetActive(true);

		string factionName = getFactionName(characterChoice);

		if (isLocalPlayer)
			clientFactionName = factionName;
		else if (isServer)
		{
			serverFactionName = factionName;
		}

		//GameObject spawnedPlayer = Instantiate(player, playerSpawnPosition, Quaternion.identity);
		//WeaponMarker weaponMarker = spawnedPlayer.GetComponentInChildren<WeaponMarker>();

		var coolDownButtons = GetComponentsInChildren<AbilityCoolDown>();
        var selectedCharacter = characters[characterChoice];

        for (var i = 0; i < coolDownButtons.Length; i++)
        {
			/*
			 * if you don't create the abilities in scriptable objects and assign them to the character in unity
			 * it goes out of range
			 */
            coolDownButtons[i].Initialize(selectedCharacter.characterAbilities[i], player);
        }
    }

}