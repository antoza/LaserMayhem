using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action
{
    public Action()
    {
    }
    
    public static Action DeserializeAction(string serializedAction) // TODO : peut-on sérialiser une Queue<string> directement ?
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
}
