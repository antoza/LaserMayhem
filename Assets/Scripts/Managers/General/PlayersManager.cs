using System;
using UnityEngine;
using UnityEngine.Assertions;
#nullable enable

public abstract class PlayersManager : Manager<PlayersManager>
{
    [field: SerializeField]
    public int NumberOfPlayers { get; private set; }
    public int currentPlayerID { get; private set; }
    protected PlayerData[] playerList;

    private void Start()
    {
        currentPlayerID = NumberOfPlayers - 1;
        playerList = new PlayerData[NumberOfPlayers];
        for (int i = 0; i < NumberOfPlayers; i++)
        {
            PlayerData player = new PlayerData(i);
            InitializePlayer(player);
            playerList[i] = player;
        }
    }

    protected virtual void InitializePlayer(PlayerData player)
    {
    }

    public virtual void StartNextPlayerTurn(int turnNumber)
    {
        currentPlayerID = (turnNumber + 1) % NumberOfPlayers;
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
}
