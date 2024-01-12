using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkipTurnButton : GameButton
{
    Button m_TurnButton;

    protected override void Awake()
    {
        base.Awake();
        m_TurnButton = GetComponent<Button>();
    }

    public override void OnClick()
    {
        Debug.Log("Is it my turn ? :" + PlayersManager.Instance.IsMyTurn());
        if(PlayersManager.Instance.IsMyTurn())
        {
            base.OnClick();
            PlayersManager.Instance.GetCurrentPlayer().PlayerActions.CreateAndVerifyEndTurnAction();
        }
    }

    // TODO : à mettre dans TurnManager, le bouton ne doit s'occuper que de son affichage et d'appeler la bonne fonction lors d'un clic
    public void BeginCooldown(bool laser)
    {
        Debug.Log("Begin Cooldown");
        Debug.Log(PlayersManager.Instance.IsMyTurn());
        Debug.Log(laser);
        if ((!PlayersManager.Instance.IsMyTurn()) || (PlayersManager.Instance.IsMyTurn() && laser))
        {
            Debug.Log("I press");
            m_Animator.SetBool("Pressed", true);
        }
        else
        {
            Debug.Log("I am unpressed");
            m_Animator.SetBool("Pressed", false);
        }
        m_TurnButton.interactable = false;
    }

    public void EndCooldown()
    {
        m_TurnButton.interactable = true;
    }
}
