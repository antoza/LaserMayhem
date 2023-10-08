using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UIEcoPlayer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_EcoText;

    [SerializeField]
    private int m_PlayerID;

    // Update is called once per frame
    void Update()
    {
        m_EcoText.text = PlayersManager.GetInstance().GetMana(m_PlayerID).ToString();
    }
}
