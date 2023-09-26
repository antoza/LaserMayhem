using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameMessageManager : NetworkBehaviour
{
    [field: SerializeField]
    private DataManager DM;
    private NetworkList<GameMessageState> players;

    private void Awake()
    {
        players = new NetworkList<GameMessageState>();
    }

    private void Start()
    {
        DM = FindObjectOfType<DataManager>();
        DM.GameMessageManager = this;
    }

    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback += HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback += HandleClientDisconnected;

            foreach (NetworkClient client in NetworkManager.Singleton.ConnectedClientsList)
            {
                HandleClientConnected(client.ClientId);
            }
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= HandleClientConnected;
            NetworkManager.Singleton.OnClientDisconnectCallback -= HandleClientDisconnected;
        }
    }

    private void HandleClientConnected(ulong clientId)
    {
        players.Add(new GameMessageState(clientId));
    }

    private void HandleClientDisconnected(ulong clientId)
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (players[i].ClientId == clientId)
            {
                players.RemoveAt(i);
                break;
            }
        }
    }






    public class Notification : MonoBehaviour//MessageBase
    {
        public string oui;
    }

    // GameObject.Find est temporaire, je pense ajouter un dictionnaire de tiles plus tard pour les référencer avec un id
    [ServerRpc(RequireOwnership = false)]
    public void MoveToDestinationTileServerRPC(string sourceTileName, string destinationTileName, int playerID, ServerRpcParams serverRpcParams = default)
    {
        PlayerData playerData = DM.PlayersManager.GetPlayer(playerID);
        if (!playerData.PlayerActions.m_CanPlay) return;
        if (DM.GameMode.MoveToDestinationTile(GameObject.Find(sourceTileName).GetComponent<Tile>(), GameObject.Find(destinationTileName).GetComponent<Tile>(), playerData))
        {
            MoveToDestinationTileClientRPC(sourceTileName, destinationTileName, playerID);
        }
    }

    [ClientRpc]
    private void MoveToDestinationTileClientRPC(string sourceTileName, string destinationTileName, int playerID)
    {
        DM.GameMode.MoveToDestinationTile(GameObject.Find(sourceTileName).GetComponent<Tile>(), GameObject.Find(destinationTileName).GetComponent<Tile>(), DM.PlayersManager.GetPlayer(playerID));
    }

    [ServerRpc(RequireOwnership = false)]
    public void TrySkipTurnServerRPC(int playerID, ServerRpcParams serverRpcParams = default)
    {
        PlayerData playerData = DM.PlayersManager.GetPlayer(playerID);
        if (!playerData.PlayerActions.m_CanPlay) return;
        TrySkipTurnClientRPC(playerID);
        playerData.PlayerActions.EndTurn();
        DM.TurnManager.StartLaserPhase();
    }

    [ClientRpc]
    private void TrySkipTurnClientRPC(int playerID)
    {
        PlayerData playerData = DM.PlayersManager.GetPlayer(playerID);
        playerData.PlayerActions.EndTurn();
        DM.TurnManager.StartLaserPhase();
    }
}
