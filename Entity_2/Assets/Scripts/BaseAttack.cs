using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "RPG Scriptable Objects/Enemy Attack", order = 2)]
public class BaseAttack : ScriptableObject
{
    public string attackName; //Name of attack
    public float attackStrength; //Multiplier applied to base ATK
}
