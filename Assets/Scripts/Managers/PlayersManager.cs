using System;
using UnityEngine;
using UnityEngine.Assertions;
#nullable enable

public sealed class PlayersManager : MonoBehaviour
{
    public static PlayersManager Instance { get; private set; }

    public int numberOfPlayers { get; private set; }
    public int currentPlayerID { get; private set; }
    private PlayerData[] playerList;

    private void Awake()
    {
        Instance = this;
        numberOfPlayers = DataManager.Instance.Rules.NumberOfPlayers;
        currentPlayerID = numberOfPlayers - 1;
        playerList = new PlayerData[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            playerList[i] = new PlayerData(i);
        }
        SetPlayerNames(GameInitialParameters.playerNames);
    }

    private void Start()
    {
    }

    public void SetPlayerNames(string[] playerNames)
    {
        for (int i = 0; i < playerNames.Length; i++)
        {
            GetPlayer(i).m_name = playerNames[i];
        }
    }

    public void StartNextPlayerTurn(int turnNumber)
    {
        currentPlayerID = (currentPlayerID + 1) % numberOfPlayers;
        playerList[currentPlayerID].PlayerActions.StartTurn(turnNumber);
    }

    public PlayerData GetPlayer(int id)
    {
        if (id >= numberOfPlayers)
        {
            Debug.Log("This id does not correspond to a player");
            return playerList[0];
        }
        return playerList[id];
    }

    public PlayerData GetCurrentPlayer()
    {
        return GetPlayer(currentPlayerID);
    }

    public PlayerData GetLocalPlayer()
    {
        Assert.IsFalse(GameInitialParameters.localPlayerID == -1, "You are the server");
        return GetPlayer(GameInitialParameters.localPlayerID);
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
}
