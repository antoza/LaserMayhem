using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RevertLastActionAction : Action
{
    public RevertLastActionAction(DataManager dataManager, PlayerData playerData) : base(dataManager, playerData)
    {
    }
}
