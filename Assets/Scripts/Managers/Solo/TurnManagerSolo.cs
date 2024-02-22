using UnityEngine;
using System;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
#nullable enable

public sealed class TurnManagerSolo : TurnManager
{
    public static new TurnManagerSolo Instance => (TurnManagerSolo)TurnManager.Instance;

    protected override IEnumerator StartTurnCoroutine()
    {
        yield return null;
        StartTurnPhase();
    }

    protected override IEnumerator EndTurnCoroutine()
    {
        StartLaserPhase();
        yield return new WaitForSeconds(LaserPhaseDuration);

        if (GameModeManager.Instance.CheckGameOver()) yield break;
        yield return StartCoroutine(StartTurnCoroutine());
    }


    // Phases

    protected override void StartTurnPhase()
    {
        base.StartTurnPhase();
        BoardManager.Instance.DisplayPredictionLaser();
    }
}
