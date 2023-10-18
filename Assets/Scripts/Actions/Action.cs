using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action/* : INetworkSerializable*/
{
    public Action()
    {
    }
    
    public static Action DeserializeAction(string serializedAction)
    {
        Queue<string> parsedString = new Queue<string>(serializedAction.Split('+'));
        Type type = Type.GetType(parsedString.Dequeue());
        if (type != null && type.IsSubclassOf(typeof(Action)))
        {
            Action action = (Action)Activator.CreateInstance(type);
            if (action.DeserializeSubAction(parsedString)) return action;
        }
        Debug.Log("Given action is incorrect");
        return null;
    }

    public virtual string SerializeAction()
    {
        return GetType().Name;
    }

    public virtual bool DeserializeSubAction(Queue<string> parsedString)
    {
        return true;
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
    /*
    public virtual void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
    {
        serializer.SerializeValue(ref PlayerID);
    }*/
}
