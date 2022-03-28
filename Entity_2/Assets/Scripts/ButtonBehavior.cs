using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehavior : MonoBehaviour
{
    public string description;
    public BattleManager battleManager;
    private string temp;

    public void DisplayButtonDescription()
    {
        temp = battleManager.eventText.text;
        battleManager.eventText.text = description;
    }

    public void RemoveButtonDescription()
    {
        battleManager.eventText.text = temp;
    }
}
