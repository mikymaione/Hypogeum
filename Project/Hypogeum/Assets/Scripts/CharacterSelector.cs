using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    public GameObject player;
    public Vector3 playerSpawnPosition = new Vector3(0, 1, -7);
    public  Character[] characters;

    public void StartGame(int characterChoice)
    {
        GameObject spawnedPlayer = Instantiate(player, playerSpawnPosition, Quaternion.identity) as GameObject;
        //WeaponMarker weaponMarker = spawnedPlayer.GetComponentInChildren<WeaponMarker>();
        //AbilityCooldown[] coolDownButtons = GetComponentInChildren<AbilityCoolDown>();
        Character selectedCharacter = characters[characterChoice];

    }
}
