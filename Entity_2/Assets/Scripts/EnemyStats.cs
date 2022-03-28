using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : GeneralStats
{
    public int DMG; //Attack power; damage dealt
    public int ACC;
    public BaseAttack[] attacks;
    public StatType[] strengths;
    public StatType[] weaknesses;
}
