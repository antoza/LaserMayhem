using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public struct GameMessageState : INetworkSerializable, IEquatable<GameMessageState>
{
    public ulong ClientID;
    public int PlayerID;

    public GameMessageState(ulong clientId, int playerID)
    {
        ClientID = clientId;
        PlayerID = playerID;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientID);
        serializer.SerializeValue(ref PlayerID);
    }

    public bool Equals(GameMessageState other)
    {
        return ClientID == other.ClientID &&
            PlayerID == other.PlayerID;
    }
}
