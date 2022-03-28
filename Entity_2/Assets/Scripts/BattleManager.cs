using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    private List<GameObject> combatants = new List<GameObject>();
    private int turnCounter = 0;
    private bool pickingTarget = false;
    private bool waitingForEnemyAttack = false;
    private bool waitingForNextTurn = false;
    private GameObject currentAttacker;
    private GameObject currentTarget;
    private PlayerAttack selectedPlayerAttack;
    private BaseAttack selectedEnemyAttack;

    public GameObject p1Data;
    private Text p1HP;
    public GameObject p2Data;
    private Text p2HP;
    public Text eventText;

    // Start is called before the first frame update
    void Start()
    {
        p1HP = p1Data.transform.GetChild(0).GetComponent<Text>();
        p1HP.text = "HP: " + GameObject.Find("Player 1").GetComponent<PlayerStats>().HP.ToString();
        p2HP = p2Data.transform.GetChild(0).GetComponent<Text>();
        p2HP.text = "HP: " + GameObject.Find("Player 2").GetComponent<PlayerStats>().HP.ToString();
        SetInitiative();
        AdvanceTurn();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (pickingTarget)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.CompareTag("Enemy"))
                    {
                        pickingTarget = false;
                        currentTarget = hit.transform.gameObject;
                        ResolveAttack(currentAttacker.GetComponent<PlayerStats>(), currentTarget.GetComponent<EnemyStats>());
                    }
                }
            }
            else if (waitingForEnemyAttack)
            {
                waitingForEnemyAttack = false;
                ResolveAttack(currentAttacker.GetComponent<EnemyStats>(), currentTarget.GetComponent<PlayerStats>());
            }
            else if (waitingForNextTurn)
            {
                waitingForNextTurn = false;
                AdvanceTurn();
            }
        }
    }

    void SetInitiative()
    {
        foreach (Transform combatant in GameObject.Find("Combatants").transform)
        {
            combatants.Add(combatant.gameObject);
        }

        combatants.Sort(SortBySpeed);
        combatants.Reverse();
    }

    void AdvanceTurn()
    {
        bool enemiesLeft = false;
        bool partyLeft = false;

        if (turnCounter >= combatants.Count)
        {
            turnCounter = 0;
        }

        foreach(GameObject combatant in combatants)
        {
            if (combatant.CompareTag("Enemy"))
            {
                enemiesLeft = true;
            }
            
            if (combatant.CompareTag("Player"))
            {
                partyLeft = true;
            }
        }

        if (enemiesLeft && partyLeft)
        {
            currentAttacker = combatants[turnCounter];

            if (currentAttacker.CompareTag("Player"))
            {
                switch (currentAttacker.GetComponent<PlayerStats>().characterName)
                {
                    case "Player 1":
                        p1Data.SetActive(true);
                        eventText.text = "What will Player 1 do?";
                        break;
                    case "Player 2":
                        p2Data.SetActive(true);
                        eventText.text = "What will Player 2 do?";
                        break;
                }
            }
            else if (currentAttacker.CompareTag("Enemy"))
            {
                PrepareEnemyAttack(combatants[turnCounter].GetComponent<EnemyStats>());
            }

            turnCounter++;
        }
        else if (!enemiesLeft)
        {
            eventText.text = "Battle clear!";
        }
        else if (!partyLeft)
        {
            eventText.text = "Game over!";
        }
    }

    public void PreparePlayerAttack(PlayerAttack attack)
    {
        p1Data.SetActive(false);
        p2Data.SetActive(false);
        eventText.text = "Select a target.";
        pickingTarget = true;
        selectedPlayerAttack = attack;
    }

    private void PrepareEnemyAttack(EnemyStats enemy)
    {
        BaseAttack attack = enemy.attacks[Random.Range(0, enemy.attacks.Length)];
        GeneralStats target;

        if (Random.value <= 0.5)
        {
            target = GameObject.Find("Player 1").GetComponent<PlayerStats>();
        }
        else
        {
            target = GameObject.Find("Player 2").GetComponent<PlayerStats>();
        }

        eventText.text = enemy.characterName + " attacked " + target.characterName + " with " + attack.attackName + "!";

        currentAttacker = enemy.gameObject;
        currentTarget = target.gameObject;
        selectedEnemyAttack = attack;
        waitingForEnemyAttack = true;
    }

    void ResolveAttack(EnemyStats damager, PlayerStats damaged)
    {
        if (Random.value <= damager.ACC * 0.01f)
        {
            eventText.text = "The attack hits!";

            int damageAmount = Mathf.RoundToInt(selectedEnemyAttack.attackStrength * currentAttacker.GetComponent<EnemyStats>().DMG * 0.1f);

            if (damager.strengths.Contains(damaged.setStat))
            {
                damageAmount = Mathf.RoundToInt(damageAmount * 1.5f);
            }

            damaged.HP -= damageAmount;

            if (damaged.HP <= 0)
            {
                damaged.HP = 0;
                combatants.Remove(damaged.gameObject);

                eventText.text = damaged.characterName + " was overwhelmed!";
            }

            //Updates Player HP readings, if necessary
            if (damaged.characterName == "Player 1")
            {
                p1HP.text = "HP: " + damaged.HP.ToString();
            }
            else if (damaged.characterName == "Player 2")
            {
                p2HP.text = "HP: " + damaged.HP.ToString();
            }
        }
        else
        {
            eventText.text = "The attack misses!";
        }

        waitingForNextTurn = true;
    }
    void ResolveAttack(PlayerStats damager, EnemyStats damaged)
    {
        damager.setStat = selectedPlayerAttack.attackType;

        if (Random.value <= selectedPlayerAttack.hitChance)
        {
            eventText.text = "The attack hits!";

            int damageAmount;
            int damageStat = 1;

            //Base damage
            switch (damager.setStat)
            {
                case GeneralStats.StatType.Willpower:
                    damageStat = damager.willpower;
                    break;
                case GeneralStats.StatType.SocialSkills:
                    damageStat = damager.socialSkills;
                    break;
                case GeneralStats.StatType.MentalStability:
                    damageStat = damager.mentalStablity;
                    break;
            }

            damageAmount = Mathf.RoundToInt(selectedPlayerAttack.attackStrength * damageStat * 0.1f);
            Debug.Log(damageAmount);

            //Check weaknesses
            if (damaged.weaknesses.Contains(damager.setStat))
            {
                damageAmount = Mathf.RoundToInt(damageAmount * 1.5f);
            }

            //Crit chance
            if (Random.value <= 0.05f)
            {
                eventText.text = "Critical hit!";
                damageAmount = Mathf.RoundToInt(damageAmount * 1.5f);
            }

            damaged.HP -= damageAmount;

            if (damaged.HP <= 0)
            {
                damaged.HP = 0;
                combatants.Remove(damaged.gameObject);

                Destroy(damaged.gameObject);
            }
        }
        else
        {
            eventText.text = "The attack misses!";
        }

        waitingForNextTurn = true;
    }

    static int SortBySpeed(GameObject combatant1, GameObject combatant2)
    {
        return combatant1.GetComponent<GeneralStats>().SPD.CompareTo(combatant2.GetComponent<GeneralStats>().SPD);
    }
}
