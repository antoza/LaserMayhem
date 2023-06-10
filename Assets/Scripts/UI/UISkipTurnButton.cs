using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkipTurnButton : MonoBehaviour
{
    private TurnManager TurnManager;
    private DataManager DataManager;

    private float m_CurrentCooldown;

    private void Awake()
    {
        DataManager = FindObjectOfType<DataManager>();
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
        while (m_CurrentCooldown > 0)
        {
            m_CurrentCooldown -= Time.deltaTime;
            yield return null;
        }
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
