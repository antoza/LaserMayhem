using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndTurnButton : GameButton
{
#if !DEDICATED_SERVER
    Button m_TurnButton;

    protected override void Awake()
    {
        base.Awake();
        m_TurnButton = GetComponent<Button>();
    }

    public override void OnClick()
    {
        if (LocalPlayerManager.Instance.TryToPlay())
        {
            base.OnClick();
            LocalPlayerManager.Instance.CreateAndVerifyEndTurnAction();
        }
    }

    public void SetAsPressed()
    {
        m_Animator.SetBool("Pressed", true);
        m_TurnButton.interactable = false;
    }

    public void SetAsUnpressed()
    {
        m_Animator.SetBool("Pressed", false);
        m_TurnButton.interactable = true;
    }
    /*
    // TODO : à mettre dans TurnManager, le bouton ne doit s'occuper que de son affichage et d'appeler la bonne fonction lors d'un clic
    public void BeginCooldown(bool laser)
    {
        if ((!PlayersManager.Instance.IsMyTurn()) || (PlayersManager.Instance.IsMyTurn() && laser))
        {
            Debug.Log("I press");
            m_Animator.SetBool("Pressed", true);
        m_TurnButton.interactable = false;
        }
        else
        {
            Debug.Log("I am unpressed");
            m_Animator.SetBool("Pressed", false);
        m_TurnButton.interactable = true;
        }
    }

    public void EndCooldown()
    {
    }*/
#endif
}
