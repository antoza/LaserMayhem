using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;

public class EndTurnAction : PlayerAction
{
    public List<Piece> PiecesPlayedThisTurn = new List<Piece>();

    public EndTurnAction() : base()
    {
    }

    public EndTurnAction(PlayerData playerData) : base(playerData)
    {
    }
}
