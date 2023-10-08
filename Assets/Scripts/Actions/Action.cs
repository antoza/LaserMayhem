using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Action : INetworkSerializable
{
    public int PlayerID;
    public PlayerData PlayerData => PlayersManager.GetInstance().GetPlayer(PlayerID);

    public static Action DeserializeAction(string serializedAction)
    {
        string[] parsedString = serializedAction.Split('+');
        Type type = Type.GetType(parsedString[0]);
        if (type != null && type.IsSubclassOf(typeof(Action)))
        {
            Action action = (Action)Activator.CreateInstance(type);
            if (action.Initialize(parsedString)) return action;
        }
        Debug.Log("Given action is incorrect");
        return null;
    }

    public virtual string SerializeAction()
    {
        return GetType().Name + '+' + PlayerID;
    }

    public Action()
    {
        PlayerID = 0;
    }

    public Action(PlayerData playerData)
    {
        PlayerID = playerData.m_playerID;
    }

    public virtual bool Initialize(string[] parsedString)
    {
        try
        {
            PlayerID = int.Parse(parsedString[1]);
            return true;
        }
        catch (Exception ex) when (ex is FormatException || ex is IndexOutOfRangeException)
        {
            return false;
        }
    }
    /*
    public virtual bool Verify()
    {
        if (PlayerData.PlayerActions.m_CanPlay) return false;
        // TODO : On pourrait ajouter qu'on n'autorise pas le joueur à jouer si le laser n'a pas fini son animation
        if (DataManager.Instance.GameMode.VerifyAction(this)) // TODO : Rendre GameMode singleton
        return true;
    }

    public abstract void Execute();*/

    public virtual void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref PlayerID);
    }
}
