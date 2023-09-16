using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : ScriptableObject
{
    private DataManager DM;
    private PlayerData PlayerData;
    public int m_health { get; private set; }

    public PlayerHealth(DataManager dataManager, PlayerData playerData)
    {
        DM = dataManager;
        PlayerData = playerData;
        m_health = DM.Rules.InitialHealth;
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
            ((GameModeRPG)DM.GameMode).PlayerDied(PlayerData.m_playerID);
        }
    }
}
