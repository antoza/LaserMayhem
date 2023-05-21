using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[RequireComponent(typeof(TextMeshProUGUI))]
public class UIPlayerTurnAnnouncement : MonoBehaviour
{
    private DataManager m_DataManager;

    [SerializeReference, TextArea(1, 2)]
    private string m_BaseString;

    //Text 
    private TextMeshProUGUI m_TurnAnnouncementText;

    //TurnManager
    private TurnManager m_TurnManager;

    private void Awake()
    {
        m_TurnAnnouncementText = GetComponent<TextMeshProUGUI>();
        m_TurnManager = FindObjectOfType<TurnManager>();
    }

    private void Start()
    {
        m_DataManager = FindObjectOfType<DataManager>();
    }


    public IEnumerator TurnAnnouncementFade(float duration)
    {
        Color c = m_TurnAnnouncementText.color;
        c.a = 1;
        m_TurnAnnouncementText.color = c;
        m_TurnAnnouncementText.text = m_BaseString + m_DataManager.PlayersManager.m_CurrentPlayerID;



        while(c.a > 0)
        {
            c.a = c.a - (1 / duration)*Time.deltaTime;
            m_TurnAnnouncementText.color = c;
            yield return null;
        }
    }
}
