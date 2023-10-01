using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : ScriptableObject
{
    private PlayerData PlayerData;
    public int m_health { get; private set; }

    public PlayerHealth(PlayerData playerData)
    {
        PlayerData = playerData;
        m_health = DataManager.Instance.Rules.InitialHealth;
    }

    public bool IsDead()
    {
        return m_health <= 0;
    }

    public void TakeDamage(int damage)
    {
        m_health -= damage;
        if (m_health <= 0) {
            m_health = 0;
            ((GameModeRPG)DataManager.Instance.GameMode).PlayerDied(PlayerData.m_playerID);
        }
    }
}
