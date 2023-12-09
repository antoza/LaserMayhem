using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipTurnButton : GameButton
{
    private float m_CurrentCooldown;
    Button m_TurnButton;

    protected override void Awake()
    {
        base.Awake();
        m_TurnButton = GetComponent<Button>();
    }

    public override void OnClick()
    {
        if(PlayersManager.Instance.GetCurrentPlayer().m_playerID == PlayersManager.Instance.GetLocalPlayer().m_playerID)
        {
            base.OnClick();
            PlayersManager.Instance.GetCurrentPlayer().PlayerActions.CreateAndVerifyEndTurnAction();
        }
    }

    // TODO : à mettre dans TurnManager, le bouton ne doit s'occuper que de son affichage et d'appeler la bonne fonction lors d'un clic
    public IEnumerator Cooldown(float cooldown, bool laser)
    {
        m_Animator.SetBool("Pressed", true);
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
            if (PlayersManager.Instance.GetCurrentPlayer().m_playerID == GameInitialParameters.localPlayerID)
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
}
