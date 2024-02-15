using System;
using UnityEngine;
using UnityEngine.Assertions;
#nullable enable

public abstract class PlayersManager : Manager<PlayersManager>
{
    [field: SerializeField]
    public int NumberOfPlayers { get; private set; }
    public int currentPlayerID { get; private set; }
    private PlayerData[] playerList;

    private void Start()
    {
        currentPlayerID = NumberOfPlayers - 1;
        playerList = new PlayerData[NumberOfPlayers];
        for (int i = 0; i < NumberOfPlayers; i++)
        {
            // TODO : regrouper avec la ligne du dessous pour avoir PlayerData(i, username)
            playerList[i] = new PlayerData(i);
        }
        SetUsernames(GameInitialParameters.usernames); // TODO : le server ne doit pas exécuter ça
    }

    public void SetUsernames(string[] username)
    {
        for (int i = 0; i < username.Length; i++)
        {
            GetPlayer(i).Username = username[i];
        }
    }

    public void StartNextPlayerTurn(int turnNumber)
    {
        currentPlayerID = (currentPlayerID + 1) % NumberOfPlayers;
        //playerList[currentPlayerID].PlayerActions.StartTurn(turnNumber);
        playerList[currentPlayerID].PlayerEconomy.AddNewTurnMana(turnNumber);
    }

    public PlayerData GetPlayer(int id)
    {
        if (id >= NumberOfPlayers)
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

    public int GetHealth(int id)
    {
        return playerList[id].PlayerHealth.Health;
    }

    public int GetMana(int id)
    {
        return playerList[id].PlayerEconomy.Mana;
    }
}
