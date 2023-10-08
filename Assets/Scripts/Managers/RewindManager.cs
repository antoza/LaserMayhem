using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;
#nullable enable

public sealed class RewindManager : ScriptableObject
{
    public static RewindManager? Instance { get; private set; }

    private Stack<Action> m_actionsList;

    public RewindManager()
    {
        m_actionsList = new Stack<Action>();
    }

    public static void SetInstance()
    {
        Instance = new RewindManager();
    }

    public static RewindManager GetInstance()
    {
        if (Instance == null)
        {
            Debug.LogError("RewindManager has not been instantiated");
        }

        return Instance!;
    }

    public bool IsEmpty()
    {
        return m_actionsList.Count == 0;
    }

    public void AddAction(Action action)
    {
        m_actionsList.Push(action);
    }

    public void RevertLastAction()
    {
        Assert.IsFalse(IsEmpty());
        Action lastAction = m_actionsList.Pop();
        DataManager.Instance.GameMode.RevertAction(lastAction);
    }

    public void RevertAllActions()
    {
        Assert.IsFalse(IsEmpty());
        while(IsEmpty())
        {
            RevertLastAction();
        }
    }

    public void ClearAllActions()
    {
        m_actionsList.Clear();
    }
}
