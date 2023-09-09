using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersManager : ScriptableObject
{
    private DataManager DM;
    public int numberOfPlayers { get; private set; }
    public int currentPlayerID { get; private set; }
    private PlayerData[] playerList;

    public PlayersManager(DataManager dataManager)
    {
        DM = dataManager;
        numberOfPlayers = DM.Rules.NumberOfPlayers;
        currentPlayerID = numberOfPlayers - 1;
        playerList = new PlayerData[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            playerList[i] = new PlayerData(DM);
        }
    }

    public void StartNextPlayerTurn(int turnNumber)
    {
        currentPlayerID = (currentPlayerID + 1) % numberOfPlayers;
        playerList[currentPlayerID].PlayerActions.StartTurn(turnNumber);
    }

    public void EndCurrentPlayerTurn()
    {
        playerList[currentPlayerID].PlayerActions.EndTurn();
    }

    public PlayerData GetCurrentPlayer()
    {
        return playerList[currentPlayerID];
    }

    public void HitPlayer(int id, int damage)
    {
        playerList[id].PlayerHealth.TakeDamage(damage);
    }

    public int GetHealth(int id)
    {
        return playerList[id].PlayerHealth.m_health;
    }

    public int GetMana(int id)
    {
        return playerList[id].PlayerEconomy.m_mana;
    }

#if DEBUG
    public void AddInfiniteMana()
    {
        playerList[currentPlayerID].PlayerActions.AddInfiniteMana();
    }
#endif
}
