using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerManaUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_EcoText;

    [SerializeField]
    private int m_PlayerID;

    void Update()
    {
        m_EcoText.text = PlayersManager.Instance.GetMana(m_PlayerID).ToString();
    }
}
