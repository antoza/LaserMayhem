using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UINamePlayer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI m_NameText;

    [SerializeField]
    private int m_PlayerID;

    // Update is called once per frame
    void Update()
    {
        m_NameText.text = PlayersManager.GetInstance().GetPlayer(m_PlayerID).m_name;
    }
}
