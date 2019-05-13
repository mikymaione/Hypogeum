﻿/*
MIT License
Copyright (c) 2019 Team Lama: Carrarini Andrea, Cerrato Loris, De Cosmo Andrea, Maione Michele
Author: Carrarini Andrea
Contributors: 
Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so, subject to the following conditions: The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE. 
*/
using UnityEngine;
using UnityEngine.Networking;

public class CharacterSelector : NetworkBehaviour
{

    public NetworkManager networkingComponent;
    public GameObject player, characterSelectPanel, chosenFactionCar, abilityPanel;
    public Vector3 playerSpawnPosition = new Vector3(0, 1, -7);
    public Character[] characters;


    public void OnCharacterSelect(int characterChoice)
    {
        characterSelectPanel.SetActive(false);
        abilityPanel.SetActive(true);

        networkingComponent = GameObject.FindGameObjectWithTag("Networking").GetComponent<NetworkManager>();
        networkingComponent.playerPrefab = Instantiate(Resources.Load("EaglesCar")) as GameObject;

        var spawnedPlayer = Instantiate(player, playerSpawnPosition, Quaternion.identity);
        //var weaponMarker = spawnedPlayer.GetComponentInChildren<WeaponMarker>();

        var coolDownButtons = GetComponentsInChildren<AbilityCoolDown>();
        var selectedCharacter = characters[characterChoice];

        for (var i = 0; i < coolDownButtons.Length; i++)
            coolDownButtons[i].Initialize(selectedCharacter.characterAbilities[i], spawnedPlayer);
    }

}