using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkipTurnButton : MonoBehaviour
{
    private float m_CurrentCooldown;
    Button m_TurnButton;

    private void Awake()
    {
        m_TurnButton = GetComponent<Button>();
    }

    public void OnClick()
    {
        PlayersManager.GetInstance().GetCurrentPlayer().PlayerActions.CreateAndVerifyEndTurnAction();
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
            TurnManager.GetInstance().StartAnnouncementPhase();
        }
        else
        {
            TurnManager.GetInstance().StartTurnPhase();
        }
    }

    public void StartCoroutineCooldownFromScriptable(float cooldown, bool laser)
    {
        StartCoroutine(Cooldown(cooldown, laser));
    }

    public void StartCoroutineAEFFACERAPRES()
    {
        StartCoroutine(taertae());
    }

    public IEnumerator taertae()
    {
        yield return new WaitForSeconds(10);
        TurnManager.GetInstance().StartAnnouncementPhase();
    }
}
