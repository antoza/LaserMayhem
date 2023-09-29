using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RevertableAction : Action
{
    public RevertableAction(DataManager dataManager, PlayerData playerData) : base(dataManager, playerData)
    {
    }

    public abstract void Revert();
}