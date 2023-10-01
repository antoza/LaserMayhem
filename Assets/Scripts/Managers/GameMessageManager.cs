using Mono.CompilerServices.SymbolWriter;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
#nullable enable

#nullable enable
public class GameMessageManager : NetworkBehaviour
{
    public static GameMessageManager? Instance { get; private set; }
    private NetworkList<GameMessageState>? players;

    private void Awake()
    {
        players = new NetworkList<GameMessageState>();

        Instance = this;
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
        players!.Add(new GameMessageState(clientId));
    }

    private void HandleClientDisconnected(ulong clientId)
    {
        for (int i = 0; i < players!.Count; i++)
        {
            if (players[i].ClientId == clientId)
            {
                players.RemoveAt(i);
                break;
            }
        }
    }





    // IDEE : faire une seule fonction où on envoie au serveur une Action. Le serveur dit "ok" si ça lui va.
    // Dans ce cas il faut trouver comment sérialiser une Action

    // GameObject.Find est temporaire, je pense ajouter un dictionnaire de tiles plus tard pour les référencer avec un id
    [ServerRpc(RequireOwnership = false)]
    public void TryMoveToDestinationTileServerRPC(string sourceTileName, string destinationTileName, int playerID, ServerRpcParams serverRpcParams = default)
    {
        // A changer d'endroit. L'Action doit exister avant d'arriver ici.
        PlayerData playerData = PlayersManager.GetInstance().GetPlayer(playerID);
        Tile sourceTile = GameObject.Find(sourceTileName).GetComponent<Tile>();
        Tile destinationTile = GameObject.Find(destinationTileName).GetComponent<Tile>();
        Piece? piece = sourceTile.m_Piece;

        if (!playerData.PlayerActions.m_CanPlay) return;
        if (DataManager.Instance.GameMode.MoveToDestinationTile(sourceTile, destinationTile, playerData))
        {
            MoveToDestinationTileClientRPC(sourceTileName, destinationTileName, playerID);
            RewindManager.GetInstance().AddAction(new MovePiece(playerData, sourceTile, destinationTile, piece!));
        }
    }

    [ClientRpc]
    private void MoveToDestinationTileClientRPC(string sourceTileName, string destinationTileName, int playerID)
    {
        PlayerData playerData = PlayersManager.GetInstance().GetPlayer(playerID);
        Tile sourceTile = GameObject.Find(sourceTileName).GetComponent<Tile>();
        Tile destinationTile = GameObject.Find(destinationTileName).GetComponent<Tile>();
        Piece? piece = sourceTile.m_Piece;

        DataManager.Instance.GameMode.MoveToDestinationTile(sourceTile, destinationTile, playerData);
        RewindManager.GetInstance().AddAction(new MovePiece(playerData, sourceTile, destinationTile, piece!));

    }

    [ServerRpc(RequireOwnership = false)]
    public void TrySkipTurnServerRPC(int playerID, ServerRpcParams serverRpcParams = default)
    {
        PlayerData playerData = PlayersManager.GetInstance().GetPlayer(playerID);
        if (!playerData.PlayerActions.m_CanPlay) return;
        SkipTurnClientRPC(playerID);
        playerData.PlayerActions.EndTurn();
        RewindManager.GetInstance().ClearAllActions();
        TurnManager.GetInstance().StartLaserPhase();
    }

    [ClientRpc]
    private void SkipTurnClientRPC(int playerID)
    {
        PlayerData playerData = PlayersManager.GetInstance().GetPlayer(playerID);
        playerData.PlayerActions.EndTurn();
        RewindManager.GetInstance().ClearAllActions();
        TurnManager.GetInstance().StartLaserPhase();
    }

    [ServerRpc(RequireOwnership = false)]
    public void TryRevertLastActionServerRPC(int playerID, ServerRpcParams serverRpcParams = default)
    {
        PlayerData playerData = PlayersManager.GetInstance().GetPlayer(playerID);
        if (!playerData.PlayerActions.m_CanPlay) return;
        RevertLastActionClientRPC(playerID);
        RewindManager.GetInstance().RevertLastAction();
    }

    [ClientRpc]
    private void RevertLastActionClientRPC(int playerID)
    {
        RewindManager.GetInstance().RevertLastAction();
    }

    [ServerRpc(RequireOwnership = false)]
    public void TryClearAllActionsServerRPC(int playerID, ServerRpcParams serverRpcParams = default)
    {
        PlayerData playerData = PlayersManager.GetInstance().GetPlayer(playerID);
        if (!playerData.PlayerActions.m_CanPlay) return;
        ClearAllActionsClientRPC(playerID);
        RewindManager.GetInstance().ClearAllActions();
    }

    [ClientRpc]
    private void ClearAllActionsClientRPC(int playerID)
    {
        RewindManager.GetInstance().ClearAllActions();
    }
}
