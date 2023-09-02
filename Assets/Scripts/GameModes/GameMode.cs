using System.Collections.Generic;
using UnityEngine;

public abstract class GameMode : ScriptableObject
{
    public DataManager DM { get; private set; }

    public void ProcessLeavingLasers(List<int> leavingLasersRight, List<int> leavingLasersLeft, List<int> leavingLasersTop, List<int> leavingLasersBot)
    {
        DM.PlayersManager.HitPlayer(0, leavingLasersBot.Count);
        DM.PlayersManager.HitPlayer(1, leavingLasersTop.Count);
    }

    public void SetDamanager(DataManager dataManager)
    {
        DM = dataManager;
    }
}
