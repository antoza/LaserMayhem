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

        if (GameModeManager.Instance.CheckGameOver()) yield break;
        StartTurnPhase();
    }

    protected override IEnumerator EndTurnCoroutine(EndTurnAction action)
    {
        StartLaserPhase((EyeClosingEndTurnAction)action);
        yield return new WaitForSeconds(LaserPhaseDuration);

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
        IsWaitingForPlayerAction = false;
#if !DEDICATED_SERVER
        UIManagerGame.Instance.UpdateEndTurnButtonState("Pressed");
        if (LocalPlayerManager.Instance.IsLocalPlayersTurn()) LocalPlayerManager.Instance.ResetSourceTile();
#endif
        GameModeManagerSolo.Instance.RemainingLasers--;
        BoardManager.Instance.DisplayEndTurnLaser();
        GameModeManagerSolo.Instance.CloseActivatedEyes(action);
        ClearPiecesPlayedThisTurn(action);
    }

    private void StartTurnPhase()
    {
        TurnNumber++;
#if !DEDICATED_SERVER
        UIManagerGame.Instance.UpdateEndTurnButtonState("Unpressed");
#endif
        BoardManager.Instance.DisplayPredictionLaser();
        IsWaitingForPlayerAction = true;
    }

    private void RevertStartTurnPhase()
    {
        IsWaitingForPlayerAction = false;
        //BoardManager.Instance.DisplayEndTurnLaser();
#if !DEDICATED_SERVER
        UIManagerGame.Instance.UpdateEndTurnButtonState("Pressed");
#endif
        TurnNumber--;
    }

    private void RevertStartLaserPhase(EyeClosingEndTurnAction action)
    {
        RevertClearPiecesPlayedThisTurn(action);
        GameModeManagerSolo.Instance.ReopenActivatedEyes(action);
        BoardManager.Instance.DisplayPredictionLaser();
        GameModeManagerSolo.Instance.RemainingLasers++;
#if !DEDICATED_SERVER
        if (LocalPlayerManager.Instance.IsLocalPlayersTurn()) LocalPlayerManager.Instance.ResetSourceTile();
        UIManagerGame.Instance.UpdateEndTurnButtonState("Unpressed");
#endif
        IsWaitingForPlayerAction = true;
    }
}
