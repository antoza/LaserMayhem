using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class MovementCostUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_CostText;

    void Update()
    {
        m_CostText.text = "Movement: " + PlayersManager.Instance.GetLocalPlayer().PlayerEconomy.m_movementCost.ToString();
    }
}
