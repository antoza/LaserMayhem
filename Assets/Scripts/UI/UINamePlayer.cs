using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UINamePlayer : MonoBehaviour
{
    [SerializeField]
    private DataManager m_DataManager;
    [SerializeField]
    private TextMeshProUGUI m_NameText;

    [SerializeField]
    private int m_PlayerID;

    // Update is called once per frame
    void Update()
    {
        m_NameText.text = m_DataManager.PlayersManager.GetPlayer(m_PlayerID).m_name;
    }
}
