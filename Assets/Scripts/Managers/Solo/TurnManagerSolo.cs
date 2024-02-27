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

    protected override IEnumerator RevertEndTurnCoroutine(EndTurnAction action)
    {
        RevertStartTurnPhase();
        yield return new WaitForSeconds(RevertLaserPhaseDuration);
        RevertStartLaserPhase((EyeClosingEndTurnAction)action);

    }


    // Phases

    private void StartLaserPhase(EyeClosingEndTurnAction action)
    {
        UIManagerGame.Instance.UpdateEndTurnButtonState("Pressed");
        GameModeManagerSolo.Instance.RemainingLasers--;
        if (LocalPlayerManager.Instance.IsLocalPlayersTurn()) LocalPlayerManager.Instance.ResetSourceTile();
        BoardManager.Instance.DisplayEndTurnLaser();
        GameModeManagerSolo.Instance.CloseActivatedEyes(action);
        // TODO : ResetPiecesPlayedThisTurn(); mais faire en sorte qu'on puisse les déreset avec un UNDO
    }

    private void StartTurnPhase()
    {
        TurnNumber++;
        ((UIManagerGame)UIManager.Instance).UpdateEndTurnButtonState("Unpressed");
        BoardManager.Instance.DisplayPredictionLaser();
    }

    private void RevertStartTurnPhase()
    {
        //BoardManager.Instance.DisplayEndTurnLaser();
        ((UIManagerGame)UIManager.Instance).UpdateEndTurnButtonState("Pressed");
        TurnNumber--;
    }

    private void RevertStartLaserPhase(EyeClosingEndTurnAction action)
    {
        GameModeManagerSolo.Instance.ReopenActivatedEyes(action);
        BoardManager.Instance.DisplayPredictionLaser();
        if (LocalPlayerManager.Instance.IsLocalPlayersTurn()) LocalPlayerManager.Instance.ResetSourceTile();
        GameModeManagerSolo.Instance.RemainingLasers++;
        ((UIManagerGame)UIManager.Instance).UpdateEndTurnButtonState("Unpressed");
    }
}
