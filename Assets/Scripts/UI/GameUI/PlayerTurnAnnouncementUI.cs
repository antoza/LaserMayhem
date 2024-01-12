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
    private Animator AnnouncementAnimator;
    [field: SerializeField]
    private TextMeshProUGUI TextBande;

    public void TurnAnnouncementActivation(float duration)
    {
        AnnouncementAnimator.SetTrigger("NewTurn");
        AnnouncementAnimator.speed = 2 / duration;
        TextBande.text = m_BaseStringStart + PlayersManager.Instance.GetCurrentPlayer().Username + m_BaseStringEnd;
    }
}
