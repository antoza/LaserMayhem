using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RevertableAction : Action
{
    public RevertableAction(PlayerData playerData) : base(playerData)
    {
    }

    public abstract void Revert();
}