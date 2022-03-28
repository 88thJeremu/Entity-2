using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject actionsList;
    public GameObject attacksList;
    public Stats partyMember;
    public void OpenAttacks()
    {
        actionsList.SetActive(false);
        attacksList.SetActive(true);
    }
    public void OpenActions()
    {
        actionsList.SetActive(true);
        attacksList.SetActive(false);
    }
}
