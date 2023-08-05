using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIHealthPlayer : MonoBehaviour
{

    [SerializeField]
    private DataManager m_DataManager;
    [SerializeField]
    private TextMeshProUGUI m_HealthText;

    [SerializeField]
    private int m_CurrentPlayerID;

    // Update is called once per frame
    void Update()
    {
        m_HealthText.text = m_DataManager.PlayersManager.GetHealth(m_CurrentPlayerID).ToString();
    }
}
