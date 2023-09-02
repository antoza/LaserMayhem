using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UIEcoPlayer : MonoBehaviour
{
    [SerializeField]
    private DataManager m_DataManager;
    [SerializeField]
    private TextMeshProUGUI m_EcoText;

    [SerializeField]
    private int m_CurrentPlayerID;

    // Update is called once per frame
    void Update()
    {
        m_EcoText.text = m_DataManager.PlayersManager.GetMana(m_CurrentPlayerID).ToString();
    }
}
