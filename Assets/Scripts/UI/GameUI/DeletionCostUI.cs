using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class DeletionCostUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_CostText;

    void Update()
    {
        m_CostText.text = "Deletion: " + PlayersManager.Instance.GetLocalPlayer().PlayerEconomy.m_deletionCost.ToString();
    }
}
