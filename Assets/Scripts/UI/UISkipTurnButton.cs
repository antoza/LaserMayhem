using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkipTurnButton : MonoBehaviour
{
    private DataManager DM;

    private float m_CurrentCooldown;
    Button m_TurnButton;

    private void Awake()
    {
        DM = FindObjectOfType<DataManager>();
        m_TurnButton = GetComponent<Button>();
    }

    public void OnClick()
    {
        DM.PlayersManager.GetCurrentPlayer().PlayerActions.PrepareEndTurn();
    }

    public IEnumerator Cooldown(float cooldown, bool laser)
    {
        m_CurrentCooldown = cooldown;
        m_TurnButton.interactable = false;
        while (m_CurrentCooldown > 0)
        {
            m_CurrentCooldown -= Time.deltaTime;
            yield return null;
        }
        m_TurnButton.interactable = true;
        if (laser)
        {
            DM.TurnManager.StartAnnouncementPhase();
        }
        else
        {
            DM.TurnManager.StartTurnPhase();
        }
    }

    public void StartCoroutineCooldownFromScriptable(float cooldown, bool laser)
    {
        StartCoroutine(Cooldown(cooldown, laser));
    }
}
