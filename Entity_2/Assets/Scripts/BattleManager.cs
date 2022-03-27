using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BattleManager : MonoBehaviour
{
    private List<Stats> combatants = new List<Stats>();
    private int turnCounter = 0;

    public GameObject p1Data;
    public GameObject p2Data;
    public GameObject eventText;

    // Start is called before the first frame update
    void Start()
    {
        SetInitiative();

        foreach (Stats combatant in combatants)
        {
            Debug.Log(combatant.characterName);
        }

        AdvanceTurn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetInitiative()
    {
        foreach (Transform combatant in GameObject.Find("Combatants").transform)
        {
            Stats m_stats = combatant.gameObject.GetComponent<Stats>();
            
            if (m_stats != null)
            {
                combatants.Add(m_stats);
            }   
        }

        combatants.Sort(SortBySpeed);
        combatants.Reverse();
    }

    void AdvanceTurn()
    {
        switch (combatants[turnCounter].characterName)
        {
            case "Player 1":
                p1Data.SetActive(true);
                p2Data.SetActive(false);
                break;
            case "Player 2":
                p2Data.SetActive(true);
                p1Data.SetActive(false);
                break;
            default:
                p1Data.SetActive(false);
                p2Data.SetActive(false);
                break;
        }

        turnCounter++;
        
        if (turnCounter == combatants.Count)
        {
            turnCounter = 0;
        }
    }

    static int SortBySpeed(Stats s1, Stats s2)
    {
        return s1.SPD.CompareTo(s2.SPD);
    }
}
