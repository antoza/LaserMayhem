using Mono.CompilerServices.SymbolWriter;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

#nullable enable
public class GameMessageManager : NetworkBehaviour
{
    public static GameMessageManager Instance { get; private set; }
    private ulong[] playersClientID;
    /*
    public static void SetInstance()
    {
        Instance = new GameNetworkManager();
    }
    
    public static GameNetworkManager GetInstance()
    {
        if (Instance == null)
        {
            Debug.LogError("GameMessageManager has not been instantiated");
        }

        return Instance!;
    }*/

    private void Awake()
    {
        Instance = this;
    }

    // "IsServer" isn't true during Awake, so we have to initialize this in Start
    private void Start()
    {
        if (IsServer)
        {
            playersClientID = new ulong[DataManager.Instance.Rules.NumberOfPlayers];
        }
    }

    private int FindClientsPlayerID(ulong ClientID)
    {
        if (IsServer)
        {
            for (int i = 0; i < playersClientID.Length; i++)
            {
                if (playersClientID[i] == ClientID) return i;
            }
            Debug.Log("ClientID not found");
            return -1;
        }
        return -1;
    }

    // TODO : Utiliser OnServerConnect et OnServerDisconnect (voir doc en ligne) à la place
    // Peut-être que cela signifie de faire en sorte que GameMessageManager hérite de NetworkManager
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

    private void HandleClientConnected(ulong clientID)
    {
        for (int i = 0; i < playersClientID.Length; i++)
        {
            if (playersClientID[i] == 0)
            {
                playersClientID[i] = clientID;
                // TODO : la ligne suivante devrait être appelée par le client dans un OnClientConnect
                ((ServerSendActionsManager)SendActionsManager.Instance).ReceivePlayerRequest(i, 0);
                return;
            }
        }
        Debug.Log("The maximum number of players has already been reached");
        // TODO : dire au client de NetworkManager.Singleton.StopClient();
    }

    private void HandleClientDisconnected(ulong clientID)
    {
        int playerID = FindClientsPlayerID(clientID);
        if (playerID == -1) return;
        playersClientID[playerID] = 0;
    }




    // Functions to be called by SendActionsManager

    public void SendActionToPlayer(Action action, int actionOrder, int playerID)
    {
        if (!IsServer) return;

        ClientRpcParams clientRpcParams = new ClientRpcParams
        {
            Send = new ClientRpcSendParams
            {
                TargetClientIds = new ulong[] { playersClientID[playerID] }
            }
        };
        string serializedAction = action.SerializeAction();

        ReceiveServerActionClientRPC(serializedAction, actionOrder, clientRpcParams);
    }

    public void SendActionToServer(PlayerAction action)
    {
        ReceivePlayerActionServerRPC(action.SerializeAction());
    }

    public void RequestActionToServer(int actionOrder)
    {
        ReceivePlayerRequestServerRPC(actionOrder);
    }


    // RPC functions, do not call them in other files

    [ClientRpc]
    public void ReceiveServerActionClientRPC(string serializedAction, int actionOrder, ClientRpcParams clientRpcParams = default)
    {
        Action action = Action.DeserializeAction(serializedAction);
        ((ClientSendActionsManager)SendActionsManager.Instance).ReceiveAndExecuteAction(action, actionOrder);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ReceivePlayerActionServerRPC(string serializedAction, ServerRpcParams serverRpcParams = default)
    {
        Action action = Action.DeserializeAction(serializedAction);
        int playerID = FindClientsPlayerID(serverRpcParams.Receive.SenderClientId);
        if (playerID == -1) return;
        ((ServerSendActionsManager)SendActionsManager.Instance).ReceivePlayerAction(playerID, action);
    }

    [ServerRpc(RequireOwnership = false)]
    public void ReceivePlayerRequestServerRPC(int actionOrder, ServerRpcParams serverRpcParams = default)
    {
        int playerID = FindClientsPlayerID(serverRpcParams.Receive.SenderClientId);
        if (playerID == -1) return;
        ((ServerSendActionsManager)SendActionsManager.Instance).ReceivePlayerRequest(playerID, actionOrder);
    }
}
