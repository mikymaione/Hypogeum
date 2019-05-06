using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Character")]
public class Character : ScriptableObject
{
    public string name = "Default";
    public int StartingHP = 100;
    //TODO watch unity's previous tutorial on abilities
    //public Ablity[] characterAbilities;
    
}
