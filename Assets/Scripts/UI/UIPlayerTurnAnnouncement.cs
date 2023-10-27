using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


[RequireComponent(typeof(TextMeshProUGUI))]
public class UIPlayerTurnAnnouncement : MonoBehaviour
{

    [SerializeReference, TextArea(1, 2)]
    private string m_BaseStringStart;
    [SerializeReference, TextArea(1, 2)]
    private string m_BaseStringEnd;

    //Text 
    private TextMeshProUGUI m_TurnAnnouncementText;

    private void Awake()
    {
        m_TurnAnnouncementText = GetComponent<TextMeshProUGUI>();
    }


    public IEnumerator TurnAnnouncementFade(float duration)
    {
        Color c = m_TurnAnnouncementText.color;
        c.a = 1;
        m_TurnAnnouncementText.color = c;
        m_TurnAnnouncementText.text = m_BaseStringStart + PlayersManager.Instance.GetCurrentPlayer().m_name + m_BaseStringEnd;



        while(c.a > 0)
        {
            c.a = c.a - (1 / duration)*Time.deltaTime;
            m_TurnAnnouncementText.color = c;
            yield return null;
        }
    }

    public void StartCoroutineTurnAnnouncementFadeFromScriptable(float duration)
    {
        StartCoroutine(TurnAnnouncementFade(duration));
    }
}
