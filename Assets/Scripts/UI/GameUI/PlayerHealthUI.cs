using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_HealthText;

    [SerializeField]
    private int m_PlayerID;

    void Update()
    {
        m_HealthText.text = PlayersManager.Instance.GetHealth(m_PlayerID).ToString();
    }
}
