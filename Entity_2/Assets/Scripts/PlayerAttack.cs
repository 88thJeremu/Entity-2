using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "RPG Scriptable Objects/Player Attack", order = 1)]
public class PlayerAttack : BaseAttack
{
    public string attackDescription; //Description; appears when button is hovered over
    public float hitChance; //Chance of attack hitting
    public GeneralStats.StatType attackType;
}
