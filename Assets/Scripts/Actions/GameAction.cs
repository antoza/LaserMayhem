using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAction // The class used to be called "Action" but I renamed it to avoid conflicts
{
    public GameAction()
    {
    }
    
    public static GameAction DeserializeAction(string serializedAction) // TODO : peut-on sérialiser une Queue<string> directement ?
    {
        Queue<string> parsedString = new Queue<string>(serializedAction.Split('+'));
        Type type = Type.GetType(parsedString.Dequeue());
        if (type != null && type.IsSubclassOf(typeof(GameAction)))
        {
            GameAction action = (GameAction)Activator.CreateInstance(type);
            if (action.DeserializeSubAction(parsedString)) return action;
        }
        Debug.Log($"Received action [{serializedAction}] is incorrect");
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
}
