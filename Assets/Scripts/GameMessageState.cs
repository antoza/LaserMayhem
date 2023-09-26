using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public struct GameMessageState : INetworkSerializable, IEquatable<GameMessageState>
{
    public ulong ClientId;
    public int SourceTileName;
    public int DestinationTileName;

    public GameMessageState(ulong clientId, int sourceTileName = -1, int destinationTileName = -1)
    {
        ClientId = clientId;
        SourceTileName = sourceTileName;
        DestinationTileName = destinationTileName;
    }

    public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref ClientId);
        serializer.SerializeValue(ref SourceTileName);
        serializer.SerializeValue(ref DestinationTileName);
    }

    public bool Equals(GameMessageState other)
    {
        return ClientId == other.ClientId &&
            SourceTileName == other.SourceTileName &&
            DestinationTileName == other.DestinationTileName;
    }
}
