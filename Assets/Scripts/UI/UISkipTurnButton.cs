using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkipTurnButton : MonoBehaviour
{
    public DataManager DataManager;
    public TurnManager TurnManager;

    private float m_CurrentCooldown;
    Button m_TurnButton;

    private void Awake()
    {
        DataManager = FindObjectOfType<DataManager>();
        m_TurnButton = GetComponent<Button>();
    }

    public void OnClick()
    {
        DataManager.PlayersManager.GetCurrentPlayer().PlayerActions.EndTurn();
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
            TurnManager.EndOfLaser();
        }
        else
        {
            TurnManager.m_CanSkipTurn = true;
        }

        
    }

    public void StartCoRoutineCooldownFromScriptable(float cooldown, bool laser)
    {
        StartCoroutine(Cooldown(cooldown, laser));
    }
}
