using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    private List<Stats> combatants = new List<Stats>();
    private int turnCounter = 0;
    private bool pickingTarget = false;
    private bool waitingForEnemyAttack = false;
    private bool waitingForNextTurn = false;
    private Stats currentAttacker;
    private Stats currentTarget;
    private Attack selectedAttack;

    public GameObject p1Data;
    private Text p1HP;
    public GameObject p2Data;
    private Text p2HP;
    public Text eventText;

    // Start is called before the first frame update
    void Start()
    {
        p1HP = p1Data.transform.GetChild(0).GetComponent<Text>();
        p2HP = p2Data.transform.GetChild(0).GetComponent<Text>();
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
                        currentTarget = hit.transform.gameObject.GetComponent<Stats>();
                        ResolveAttack(currentAttacker, currentTarget);
                    }
                }
            }
            else if (waitingForEnemyAttack)
            {
                waitingForEnemyAttack = false;
                ResolveAttack(currentAttacker, currentTarget);
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
        bool enemiesLeft = false;

        if (turnCounter >= combatants.Count)
        {
            turnCounter = 0;
        }

        foreach(Stats combatant in combatants)
        {
            if (combatant.CompareTag("Enemy"))
            {
                enemiesLeft = true;
            }
        }

        if (enemiesLeft)
        {
            currentAttacker = combatants[turnCounter];

            switch (combatants[turnCounter].characterName)
            {
                case "Player 1":
                    p1Data.SetActive(true);
                    eventText.text = "What will Player 1 do?";
                    break;
                case "Player 2":
                    p2Data.SetActive(true);
                    eventText.text = "What will Player 2 do?";
                    break;
                default:
                    PrepareEnemyAttack(combatants[turnCounter]);
                    break;
            }

            turnCounter++;
        }
        else
        {
            eventText.text = "Battle clear!";
        }
    }

    public void PreparePlayerAttack(Attack attack)
    {
        p1Data.SetActive(false);
        p2Data.SetActive(false);
        eventText.text = "Select a target.";
        pickingTarget = true;
        selectedAttack = attack;
    }

    private void PrepareEnemyAttack(Stats enemy)
    {
        Attack attack = enemy.attacks[Random.Range(0, enemy.attacks.Length)];
        float targetRoll = Random.value;
        Stats target;

        if (targetRoll <= 0.5)
        {
            target = GameObject.Find("Player 1").GetComponent<Stats>();
        }
        else
        {
            target = GameObject.Find("Player 2").GetComponent<Stats>();
        }

        eventText.text = enemy.characterName + " attacked " + target.characterName + " with " + attack.attackName + "!";

        currentAttacker = enemy;
        currentTarget = target;
        waitingForEnemyAttack = true;
    }

    void ResolveAttack(Stats damager, Stats damaged)
    {
        float hitRoll = Random.value;
        if (hitRoll <= selectedAttack.hitChance)
        {
            eventText.text = "The attack hits!";
            damaged.HP -= Mathf.RoundToInt(selectedAttack.attackStrength * damager.ATK) - damaged.DEF;

            //Updates Player HP readings, if necessary
            if (damaged.characterName == "Player 1")
            {
                p1HP.text = "HP: " + damaged.HP.ToString();
            }
            else if (damaged.characterName == "Player 2")
            {
                p2HP.text = "HP: " + damaged.HP.ToString();
            }

            if (damaged.HP <= 0)
            {
                damaged.HP = 0;
                combatants.Remove(damaged);

                if (damaged.gameObject.CompareTag("Enemy"))
                {
                    Destroy(damaged.gameObject);
                }
                else if (damaged.gameObject.CompareTag("Player"))
                {
                    eventText.text = damaged.characterName + " was overwhelmed!";
                }
            }
        }
        else
        {
            eventText.text = "The attack misses!";
        }

        waitingForNextTurn = true;
    }

    static int SortBySpeed(Stats s1, Stats s2)
    {
        return s1.SPD.CompareTo(s2.SPD);
    }
}
