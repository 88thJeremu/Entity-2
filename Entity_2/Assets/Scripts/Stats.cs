using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public string characterName;
    public int HP; //Health
    public int ATK; //Attack power; damage dealt
    public int DEF; //Defense; subtracted from enemy ATK when attacked
    public int SPD; //Speed; determines turn order
    public Attack[] attacks;
}
