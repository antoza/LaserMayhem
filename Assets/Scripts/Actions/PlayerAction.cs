using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerAction : GameAction
{
    public PlayerData PlayerData;

    public PlayerAction() : base()
    {
    }
    
    public PlayerAction(PlayerData playerData) : base()
    {
        PlayerData = playerData;
    }

    public override string SerializeAction()
    {
        return base.SerializeAction() + "+" + PlayerData.PlayerID;
    }
    public override bool DeserializeSubAction(Queue<string> parsedString)
    {
        try
        {
            PlayerData = PlayersManager.Instance.GetPlayer(int.Parse(parsedString.Dequeue()));
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}