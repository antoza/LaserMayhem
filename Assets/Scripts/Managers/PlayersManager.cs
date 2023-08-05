using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersManager : ScriptableObject
{
    private DataManager m_DataManager;
    public int m_NumberOfPlayers { get; private set; }
    public int m_CurrentPlayerID { get; private set; }
    private PlayerData[] m_PlayerList;

    void Awake()
    {
        m_DataManager = FindObjectOfType<DataManager>();
        m_NumberOfPlayers = m_DataManager.Rules.NumberOfPlayers;
        m_CurrentPlayerID = m_NumberOfPlayers - 1;
        m_PlayerList = new PlayerData[m_NumberOfPlayers];
        for (int i = 0; i < m_NumberOfPlayers; i++)
        {
            m_PlayerList[i] = new PlayerData();
        }
    }

    public void StartNextPlayerTurn(int turnNumber)
    {
        m_CurrentPlayerID = (m_CurrentPlayerID + 1) % m_NumberOfPlayers;
        m_PlayerList[m_CurrentPlayerID].PlayerActions.StartTurn(turnNumber);
    }

    public void EndCurrentPlayerTurn()
    {
        m_PlayerList[m_CurrentPlayerID].PlayerActions.EndTurn();
    }

    public PlayerData GetCurrentPlayer()
    {
        return m_PlayerList[m_CurrentPlayerID];
    }

    public void HitPlayer(int id)
    {
        m_PlayerList[id].PlayerHealth.TakeDamage(1);
    }

    public int GetHealth(int id)
    {
        return m_PlayerList[id].PlayerHealth.m_health;
    }

    public int GetMana(int id)
    {
        return m_PlayerList[id].PlayerEconomy.m_mana;
    }
}
