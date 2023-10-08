using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class EndTurnAction : Action
{
    public EndTurnAction() : base()
    {
    }

    public EndTurnAction(PlayerData playerData) : base(playerData)
    {
    }
}
