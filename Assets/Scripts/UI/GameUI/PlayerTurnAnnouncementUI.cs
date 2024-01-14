using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerTurnAnnouncementUI : MonoBehaviour
{
    [SerializeReference, TextArea(1, 2)]
    private string m_BaseStringStart;
    [SerializeReference, TextArea(1, 2)]
    private string m_BaseStringEnd;

    [field: SerializeField]
    private Animator announcementAnimator;
    [field: SerializeField]
    private TextMeshProUGUI announcementTMP;

    private float m_AnnouncementPhaseDuration;

    private void Start()
    {
        m_AnnouncementPhaseDuration = DataManager.Instance.Rules.AnnouncementPhaseDuration;
    }

    public void StartAnimation(string playerName)
    {
        announcementAnimator.SetTrigger("NewTurn");
        announcementAnimator.speed = 2 / m_AnnouncementPhaseDuration;
        announcementTMP.text = m_BaseStringStart + playerName + m_BaseStringEnd;
    }
}
