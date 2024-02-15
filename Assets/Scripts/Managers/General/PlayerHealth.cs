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
#if !DEDICATED_SERVER
            ((UIManagerGame)UIManager.Instance).UpdateHealth(PlayerData.m_playerID, value);
#endif
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
            GameModeManagerRPG.Instance.PlayerDied(PlayerData.m_playerID);
        }
    }
}
