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

    public void StartAnimation(string playerName, float duration)
    {
        announcementAnimator.SetTrigger("NewTurn");
        announcementAnimator.speed = 2 / duration;
        announcementTMP.text = m_BaseStringStart + playerName + m_BaseStringEnd;
    }
}
