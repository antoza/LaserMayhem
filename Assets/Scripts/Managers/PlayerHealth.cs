using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : ScriptableObject
{
    private PlayerData PlayerData;

    private int _health;
    public int Health
    {
        get => _health;
        private set
        {
            _health = value;
            UIManager.Instance.UpdateHealth(PlayerData.m_playerID, value);
        }
    }

    public PlayerHealth(PlayerData playerData)
    {
        PlayerData = playerData;

        Health = DataManager.Instance.Rules.InitialHealth;
    }

    public bool IsDead()
    {
        return Health <= 0;
    }

    public void TakeDamage(int damage)
    {
        Health -= damage;
        if (Health <= 0) {
            Health = 0;
            ((GameModeRPG)DataManager.Instance.GameMode).PlayerDied(PlayerData.m_playerID);
        }
    }
}
