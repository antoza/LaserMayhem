using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RevertAllActionsAction : Action
{
    public RevertAllActionsAction(DataManager dataManager, PlayerData playerData) : base(dataManager, playerData)
    {
    }
}
