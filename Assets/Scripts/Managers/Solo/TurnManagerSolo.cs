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

    protected override IEnumerator EndTurnCoroutine(EndTurnAction action)
    {
        StartLaserPhase((EyeClosingEndTurnAction)action);
        yield return new WaitForSeconds(LaserPhaseDuration);

        if (GameModeManager.Instance.CheckGameOver()) yield break;
        yield return StartCoroutine(StartTurnCoroutine());
    }


    // Phases

    private void StartLaserPhase(EyeClosingEndTurnAction action)
    {
        ((UIManagerGame)UIManager.Instance).UpdateEndTurnButtonState("Pressed");
        if (LocalPlayerManager.Instance.IsLocalPlayersTurn()) LocalPlayerManager.Instance.ResetSourceTile();
        BoardManager.Instance.DisplayEndTurnLaser();
        GameModeManagerSolo.Instance.CloseActivatedEyes(action);
        // TODO : ResetPiecesPlayedThisTurn(); mais faire en sorte qu'on puisse les déreset avec un UNDO
    }

    private void StartTurnPhase()
    {
        ((UIManagerGame)UIManager.Instance).UpdateEndTurnButtonState("Unpressed");
        BoardManager.Instance.DisplayPredictionLaser();
    }
}
