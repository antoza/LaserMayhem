using Mono.CompilerServices.SymbolWriter;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

#nullable enable
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


    [ServerRpc(RequireOwnership = false)]
    public void TryActionServerRPC(int playerID, Action action, ServerRpcParams serverRpcParams = default)
    {

    }


    // TODO : IDEE : faire une seule fonction où on envoie au serveur une Action. Le serveur dit "ok" si ça lui va.
    // Dans ce cas il faut trouver comment sérialiser une Action

    [ServerRpc(RequireOwnership = false)]
    public void TrySkipTurnServerRPC(int playerID, ServerRpcParams serverRpcParams = default)
    {
        PlayerData playerData = DM.PlayersManager.GetPlayer(playerID);
        if (!playerData.PlayerActions.m_CanPlay) return;
        SkipTurnClientRPC(playerID);
        playerData.PlayerActions.EndTurn();
        DM.RewindManager.ClearAllActions();
        DM.TurnManager.StartLaserPhase();
    }

    [ClientRpc]
    private void SkipTurnClientRPC(int playerID)
    {
        PlayerData playerData = DM.PlayersManager.GetPlayer(playerID);
        playerData.PlayerActions.EndTurn();
        DM.RewindManager.ClearAllActions();
        DM.TurnManager.StartLaserPhase();
    }

    [ServerRpc(RequireOwnership = false)]
    public void TryRevertLastActionServerRPC(int playerID, ServerRpcParams serverRpcParams = default)
    {
        PlayerData playerData = DM.PlayersManager.GetPlayer(playerID);
        if (!playerData.PlayerActions.m_CanPlay) return;
        RevertLastActionClientRPC(playerID);
        DM.RewindManager.RevertLastAction();
    }

    [ClientRpc]
    private void RevertLastActionClientRPC(int playerID)
    {
        DM.RewindManager.RevertLastAction();
    }

    [ServerRpc(RequireOwnership = false)]
    public void TryClearAllActionsServerRPC(int playerID, ServerRpcParams serverRpcParams = default)
    {
        PlayerData playerData = DM.PlayersManager.GetPlayer(playerID);
        if (!playerData.PlayerActions.m_CanPlay) return;
        ClearAllActionsClientRPC(playerID);
        DM.RewindManager.ClearAllActions();
    }

    [ClientRpc]
    private void ClearAllActionsClientRPC(int playerID)
    {
        DM.RewindManager.ClearAllActions();
    }
}
