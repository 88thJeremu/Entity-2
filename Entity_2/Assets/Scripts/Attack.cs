using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "RPG Scriptable Objects/Attack", order = 1)]
public class Attack : ScriptableObject
{
    
    public string attackName; //Name of attack
    public string attackDescription; //Description; appears when button is hovered over
    public float attackStrength; //Multiplier applied to base ATK
    public float hitChance; //Chance of hitting opponent
    public enum Type //Used to differentiate between player and enemy attacks
    {
        player,
        enemy,
    };
    public Type attackType;
}
