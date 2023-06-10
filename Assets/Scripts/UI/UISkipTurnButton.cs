using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkipTurnButton : MonoBehaviour
{
    private TurnManager TurnManager;
    private DataManager DataManager;

    private float m_CurrentCooldown;
    Button m_TurnButton;

    private void Awake()
    {
        DataManager = FindObjectOfType<DataManager>();
        m_TurnButton = GetComponent<Button>();
    }

    private void Start()
    {
        TurnManager = DataManager.TurnManager;
    }


    public void OnClick()
    {
        TurnManager.TrySkipTurn(false);
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
            TurnManager.EndOfLaser(false);
        }
        else
        {
            TurnManager.m_CanSkipTurn = true;
        }

        
    }
}
