using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkipTurnButton : MenuButton
{
    private float m_CurrentCooldown;
    Button m_TurnButton;

    private void Awake()
    {
        m_TurnButton = GetComponent<Button>();
    }

    private void Start()
    {
    }

    public override void OnClick()
    {
        if(PlayersManager.Instance.GetCurrentPlayer().m_playerID == PlayersManager.Instance.GetLocalPlayer().m_playerID)
        {
            base.OnClick();
            PlayersManager.Instance.GetCurrentPlayer().PlayerActions.CreateAndVerifyEndTurnAction();
        }
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
            TurnManager.Instance.StartAnnouncementPhase();
            if (PlayersManager.Instance.GetCurrentPlayer().m_playerID == PlayersManager.Instance.GetLocalPlayer().m_playerID)
            {
                m_Animator.SetBool("Pressed", false);
            }
        }
        else
        {
            TurnManager.Instance.StartTurnPhase();  
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
        yield return new WaitForSeconds(1);
        TurnManager.Instance.StartAnnouncementPhase();

        if (PlayersManager.Instance.GetCurrentPlayer().m_playerID != PlayersManager.Instance.GetLocalPlayer().m_playerID)
        {
            m_Animator.SetBool("Pressed", true);
        }
    }
}
