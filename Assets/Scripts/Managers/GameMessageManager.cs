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

    public static void SetInstance()
    {
        Instance = new GameMessageManager();
    }

    public static GameMessageManager GetInstance()
    {
        if (Instance == null)
        {
            Debug.LogError("RewindManager has not been instantiated");
        }

        return Instance!;
    }

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

    public void VerifyActionAndSendItToServer(Action action)
    {
        if (DataManager.Instance.GameMode.VerifyAction(action))
        {
            VerifyAndExecuteActionServerRPC(PlayersManager.GetInstance().GetLocalPlayer().m_playerID, action.SerializeAction());
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void VerifyAndExecuteActionServerRPC(int playerID, string serializedAction, ServerRpcParams serverRpcParams = default)
    {
        Action action = Action.DeserializeAction(serializedAction);
        if (DataManager.Instance.GameMode.VerifyAction(action))
        {
            DataManager.Instance.GameMode.ExecuteAction(action);
            ExecuteActionClientRPC(playerID, serializedAction);
        }
    }

    [ClientRpc]
    private void ExecuteActionClientRPC(int playerID, string serializedAction)
    {
        Action action = Action.DeserializeAction(serializedAction);
        DataManager.Instance.GameMode.ExecuteAction(action);
    }

        /*

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
        }*/
    }
