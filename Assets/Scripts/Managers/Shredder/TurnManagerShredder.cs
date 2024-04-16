using UnityEngine;
using System;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
#nullable enable

public sealed class TurnManagerShredder : TurnManager
{
    public static new TurnManagerShredder Instance => (TurnManagerShredder)TurnManager.Instance;

    [field: SerializeField]
    public int ConveyorPhaseDuration { get; private set; }

    protected override IEnumerator StartTurnCoroutine()
    {
        yield return null;
        if (GameModeManagerShredder.Instance.CheckGameOverLaserPhase()) yield break;
        StartConveyorPhase();
        yield return new WaitForSeconds(ConveyorPhaseDuration);

        if (GameModeManagerShredder.Instance.CheckGameOverConveyorPhase()) yield break;
        StartTurnPhase();
    }

    protected override IEnumerator EndTurnCoroutine(EndTurnAction action)
    {
        StartLaserPhase(action);
        yield return new WaitForSeconds(LaserPhaseDuration);

        yield return StartCoroutine(StartTurnCoroutine());
    }

    protected override IEnumerator RevertEndTurnCoroutine(EndTurnAction action)
    {
        yield return null;
    }


    // Phases

    private void StartLaserPhase(EndTurnAction action)
    {
        IsWaitingForPlayerAction = false;
#if !DEDICATED_SERVER
        if (LocalPlayerManager.Instance.IsLocalPlayersTurn()) LocalPlayerManager.Instance.ResetSourceTile();
#endif
        BoardManager.Instance.DisplayEndTurnLaser();
        GameModeManagerShredder.Instance.UpdateCrystalsAndBombsState();
    }

    private void StartConveyorPhase()
    {
        GameModeManagerShredder.Instance.UpdateScore();
        BoardManager.Instance.ClearLaser();
        BoardManagerShredder.Instance.OperateConveyors();
        GameModeManagerShredder.Instance.GeneratePiecesOnTopConveyors(TurnNumber);
    }

    private void StartTurnPhase()
    {
        TurnNumber++;
        BoardManagerShredder.Instance.ShredPiecesOnShreddingTiles();
        BoardManager.Instance.DisplayPredictionLaser();
        IsWaitingForPlayerAction = true;
    }
}
