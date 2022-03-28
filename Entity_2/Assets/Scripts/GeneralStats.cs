using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralStats : MonoBehaviour
{
    public string characterName;
    public int HP; //Health
    public int SPD; //Speed; determines turn order

    public enum StatType
    {
        None,
        Willpower,
        SocialSkills,
        MentalStability,
    }
}
