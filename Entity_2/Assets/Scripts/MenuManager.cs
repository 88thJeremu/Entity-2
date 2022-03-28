using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject actionsList;
    public GameObject attackTypesList;
    public GameObject willAttacksList;
    public GeneralStats partyMember;
    public void OpenAttackTypes()
    {
        actionsList.SetActive(false);
        attackTypesList.SetActive(true);
        willAttacksList.SetActive(false);
    }

    public void OpenWillAttacks()
    {
        actionsList.SetActive(false);
        attackTypesList.SetActive(false);
        willAttacksList.SetActive(true);
    }

    public void OpenActions()
    {
        actionsList.SetActive(true);
        attackTypesList.SetActive(false);
        willAttacksList.SetActive(false);
    }
}
