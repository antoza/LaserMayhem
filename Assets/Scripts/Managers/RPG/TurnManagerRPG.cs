using UnityEngine;
using System;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
#nullable enable

public sealed class TurnManagerRPG : TurnManager
{
    public static new TurnManagerRPG Instance => (TurnManagerRPG)TurnManager.Instance;

    [SerializeField]
    public int AnnouncementPhaseDuration { get; private set; } = 3;

#if DEDICATED_SERVER
    private SelectionTilesUpdate m_SelectionTilesUpdate;
#endif

    protected override void Start()
    {
#if DEDICATED_SERVER
        m_SelectionTilesUpdate = FindObjectOfType<SelectionTilesUpdate>();
#endif
        base.Start();
    }

    protected override IEnumerator StartTurnCoroutine()
    {
        yield return null;
        if (GameModeManager.Instance.CheckGameOver()) yield break;

        StartAnnouncementPhase();
        yield return new WaitForSeconds(AnnouncementPhaseDuration);

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
#if !DEDICATED_SERVER
        ((UIManagerGame)UIManager.Instance).UpdateEndTurnButtonState("Pressed");
        if (LocalPlayerManager.Instance.IsLocalPlayersTurn()) LocalPlayerManager.Instance.ResetSourceTile();
#endif
        RewindManager.Instance.ClearAllActions();
        BoardManager.Instance.DisplayEndTurnLaser();
        ClearPiecesPlayedThisTurn(action);

        // TODO : A mettre dans une fonction spéciale qu'on pourrait mettre en abstract dans TurnManager (du style ProcessReceivers), ou alors dans GameModeManager
        foreach (Receiver receiver in BoardManager.Instance.GetReceivers())
        {
            int receivedDamage = receiver.GetReceivedIntensity();
            if (receivedDamage > 0)
            {
#if !DEDICATED_SERVER
                ((UIManagerGame)UIManager.Instance).DisplayHealthLoss(receivedDamage, receiver.transform.position);
#endif  
                ((Weakness)receiver).WeakPlayer.PlayerHealth.TakeDamage(receivedDamage);
            }
        }
    }

    private void StartAnnouncementPhase()
    {
        TurnNumber++;
        PlayersManager.Instance.StartNextPlayerTurn(TurnNumber);
#if !DEDICATED_SERVER
        ((UIManagerGame)UIManager.Instance).TriggerPlayerTurnAnnouncement(AnnouncementPhaseDuration);
#endif
        BoardManager.Instance.ClearLaser();
        BoardManagerRPG.Instance.SwitchWeakSides(TurnNumber);
        BoardManager.Instance.DisplayPredictionLaser();
#if DEDICATED_SERVER
        if (GameInitialParameters.localPlayerID == -1) m_SelectionTilesUpdate.ServerUpdateSelectionPieces(); // TODO : Ligne à modifier
#endif
    }
    private void StartTurnPhase()
    {
#if !DEDICATED_SERVER
        ((UIManagerGame)UIManager.Instance).UpdateEndTurnButtonState("Unpressed");
#endif
    }
}
