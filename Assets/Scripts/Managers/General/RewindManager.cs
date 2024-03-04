using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;
using System.Linq;
#nullable enable

public sealed class RewindManager : MonoBehaviour
{
    public static RewindManager Instance { get; private set; }

    private Stack<Action> m_actionsList;

    private void Awake()
    {
        Instance = this;
        m_actionsList = new Stack<Action>();
    }
    /*
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
    }*/

    public bool IsEmpty()
    {
        return m_actionsList.Count == 0;
    }

    public void AddAction(Action action)
    {
        m_actionsList.Push(action);
#if !DEDICATED_SERVER
        UIManagerGame.Instance.UpdateUndoButtonState("Unpressed");
#endif
    }

    public void RevertLastAction()
    {
        Assert.IsFalse(IsEmpty());
        Action lastAction = m_actionsList.Pop();
        GameModeManager.Instance.RevertAction(lastAction);
#if !DEDICATED_SERVER
        if (IsEmpty()) UIManagerGame.Instance.UpdateUndoButtonState("Pressed");
        else UIManagerGame.Instance.UpdateUndoButtonState("QuicklyPressed");
#endif
    }

    public void RevertUntilLastEndTurnAction() // TODO : voir ce qui est le mieux
        // Pour le visuel, mettre une pile de "charges" verticale, qui se vide de 1 à chaque laser consommé, et tout en haut avoir un petit bouton undo de tour
    {
        Assert.IsFalse(IsEmpty());
        do RevertLastAction();
        while (m_actionsList.Last() is not EndTurnAction);
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
#if !DEDICATED_SERVER
        UIManagerGame.Instance.UpdateUndoButtonState("Pressed");
#endif
    }
}
