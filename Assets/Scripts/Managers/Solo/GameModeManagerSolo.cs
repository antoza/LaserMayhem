using Mono.CompilerServices.SymbolWriter;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

#nullable enable

public class GameModeManagerSolo : GameModeManager
{
    public static new GameModeManagerSolo Instance => (GameModeManagerSolo)GameModeManager.Instance;

    public int ChallengeNumber { get; private set; }

    [SerializeField]
    private int _startingLasersCount;

    private int _remainingLasers;
    public int RemainingLasers
    {
        get => _remainingLasers;
        set
        {
            _remainingLasers = value;
#if !DEDICATED_SERVER
            UIManagerGame.Instance.UpdateTurnCount(value);
#endif
        }
    }

    private int _totalEyes;
    private int _closedEyes = 0; // TODO : En faire une propri�t� qui, dans le set, modifie le score automatiquement, au lieu d'appeler UpdateScore

    private void Start()
    {
        int.TryParse(SceneManager.GetActiveScene().name.Split("Challenge")[1], out int number);
        ChallengeNumber = number;
#if !DEDICATED_SERVER
        UIManagerGame.Instance.UpdateChallengeTitle(number);
#endif

        RemainingLasers = _startingLasersCount;
        _totalEyes = BoardManager.Instance.GetReceivers().Count();
        UpdateScore();
    }

    public void UpdateScore()
    {
#if !DEDICATED_SERVER
        UIManagerGame.Instance.UpdateScoreFraction(_closedEyes, _totalEyes);
#endif
    }

    public override bool CheckGameOver()
    {
        if (_closedEyes == _totalEyes)
        {
            TriggerGameOver(0);
            return true;
        }
        if (RemainingLasers == 0) {
            TriggerGameOver(1);
            return true;
        }
        return false;
    }

    public override void TriggerGameOver(int? winner)
    {
        int oldNumber = PlayerPrefs.GetInt("unlockedChallenges");
        if (oldNumber <= ChallengeNumber) PlayerPrefs.SetInt("unlockedChallenges", ChallengeNumber + 1);
        base.TriggerGameOver(winner);
    }

    public override bool VerifyAction(PlayerAction action)
    {
        if (!base.VerifyAction(action)) return false;
        switch (action)
        {
            case MovePieceAction:
                return VerifyMovePieceAction((MovePieceAction)action);
            case UndoAction:
            case UndoEverythingAction:
                return !RewindManager.Instance.IsEmpty();
            case EndTurnAction:
                return true;
            default:
                return false;
        }
    }

    public override void ExecuteAction(Action action)
    {
        switch (action)
        {
            case MovePieceAction:
                ExecuteMovePieceAction((MovePieceAction)action);
                break;
            case UndoAction:
                RewindManager.Instance.RevertLastAction();
                break;
            case UndoEverythingAction:
                RewindManager.Instance.RevertAllActions();
                break;
            case EyeClosingEndTurnAction:
                RewindManager.Instance.AddAction(action);
                TurnManager.Instance.EndTurn((EyeClosingEndTurnAction)action);
                break;
            default:
                // TODO : On peut rajouter un Throw Exception
                break;
        }
    }

    public override void RevertAction(Action action)
    {
        switch (action)
        {
            case MovePieceAction:
                RevertMovePieceAction((MovePieceAction)action);
                break;
            case EyeClosingEndTurnAction:
                TurnManager.Instance.RevertEndTurn((EyeClosingEndTurnAction)action);
                // TODO : d�reset les pi�ces jou�es ce tour
                // TODO : redonner un "remaining laser"
                break;
            default:
                // TODO : On peut rajouter un Throw Exception
                break;
        }
    }

    public bool VerifyMovePieceAction(MovePieceAction action)
    {
        Tile sourceTile = action.SourceTile;
        Tile targetTile = action.TargetTile;
        PlayerData playerData = action.PlayerData;

        if (sourceTile == null)
        {
            return false;
        }

        switch ((sourceTile!, targetTile))
        {
            case (SelectionTile, NormalBoardTile):
                if (!VerifyPlacement(playerData, (SelectionTile)sourceTile, (NormalBoardTile)targetTile)) return false;
                break;
            case (NormalBoardTile, NormalBoardTile):
                if (!VerifyMovement(playerData, (NormalBoardTile)sourceTile, (NormalBoardTile)targetTile)) return false;
                break;
            case (NormalBoardTile, SelectionTile):
                if (!VerifyMoveBackToSelection(playerData, (NormalBoardTile)sourceTile, (SelectionTile)targetTile)) return false;
                break;
            case (NormalBoardTile, TrashTile):
                if (!VerifyDeletion(playerData, (NormalBoardTile)sourceTile)) return false;
                break;
            default:
                return false;
        }
        return true;
    }

    public void ExecuteMovePieceAction(MovePieceAction action)
    {
        // TODO : je n'aime pas cette approche, il faudrait que Clear / DisplayLaser soient appel�s lors d'une modification du board
        BoardManager.Instance.ClearLaser();

        Tile sourceTile = action.SourceTile;
        Tile targetTile = action.TargetTile;
        PlayerData playerData = action.PlayerData;

        switch ((sourceTile!, targetTile))
        {
            case (SelectionTile, NormalBoardTile):
                ExecutePlacement(playerData, (SelectionTile)sourceTile, (NormalBoardTile)targetTile);
                break;
            case (NormalBoardTile, NormalBoardTile):
                ExecuteMovement(playerData, (NormalBoardTile)sourceTile, (NormalBoardTile)targetTile);
                break;
            case (NormalBoardTile, SelectionTile):
                ExecuteMoveBackToSelection(playerData, (NormalBoardTile)sourceTile, (SelectionTile)targetTile);
                break;
            case (NormalBoardTile, TrashTile):
                action.SourcePiece = sourceTile.Piece;
                ExecuteDeletion(playerData, (NormalBoardTile)sourceTile);
                break;
            default:
                // TODO : On peut rajouter un Throw Exception
                break;
        }

        RewindManager.Instance.AddAction(action);
        BoardManager.Instance.DisplayPredictionLaser();
    }

    public void RevertMovePieceAction(MovePieceAction action)
    {
        BoardManager.Instance.ClearLaser();

        Tile sourceTile = action.SourceTile;
        Tile targetTile = action.TargetTile;
        PlayerData playerData = action.PlayerData;

        switch ((sourceTile!, targetTile))
        {
            case (SelectionTile, NormalBoardTile):
                targetTile.Piece!.IsPlayedThisTurn = false;
                sourceTile.Piece = targetTile.Piece;
                break;
            case (NormalBoardTile, NormalBoardTile):
                sourceTile.Piece = targetTile.Piece;
                break;
            case (NormalBoardTile, SelectionTile):
                targetTile.Piece!.IsPlayedThisTurn = true;
                sourceTile.Piece = targetTile.Piece;
                break;

            case (NormalBoardTile, TrashTile):
                sourceTile.Piece = action.SourcePiece;
                break;

            default:
                break;
        }

        BoardManager.Instance.DisplayPredictionLaser();
    }

    public bool VerifyPlacement(PlayerData playerData, SelectionTile sourceTile, NormalBoardTile targetTile)
    {
        if (sourceTile.Piece == null) return false;
        if (targetTile.Piece != null) return false;
        return true;
    }

    public void ExecutePlacement(PlayerData playerData, SelectionTile sourceTile, NormalBoardTile targetTile)
    {
        targetTile.Piece = sourceTile.Piece;
        targetTile.Piece!.IsPlayedThisTurn = true;
    }

    public bool VerifyMovement(PlayerData playerData, NormalBoardTile sourceTile, NormalBoardTile targetTile)
    {
        if (sourceTile.Piece == null) return false;
        if (targetTile.Piece != null) return false;
        if (sourceTile == targetTile) return false;
        if (!sourceTile.Piece!.IsPlayedThisTurn) return false;
        return true;
    }

    public void ExecuteMovement(PlayerData playerData, NormalBoardTile sourceTile, NormalBoardTile targetTile)
    {
        targetTile.Piece = sourceTile.Piece;
    }

    public bool VerifyMoveBackToSelection(PlayerData playerData, NormalBoardTile sourceTile, SelectionTile targetTile)
    {
        if (sourceTile.Piece == null) return false;
        if (targetTile.Piece != null) return false;
        if (sourceTile == targetTile) return false;
        if (!sourceTile.Piece!.IsPlayedThisTurn) return false;
        return true;
    }

    public void ExecuteMoveBackToSelection(PlayerData playerData, NormalBoardTile sourceTile, SelectionTile targetTile)
    {
        targetTile.Piece = sourceTile.Piece;
        targetTile.Piece!.IsPlayedThisTurn = false;
    }

    public bool VerifyDeletion(PlayerData playerData, NormalBoardTile sourceTile)
    {
        if (sourceTile.Piece == null) return false;
        return true;
    }

    public void ExecuteDeletion(PlayerData playerData, NormalBoardTile sourceTile)
    {
        sourceTile.Piece = null;
    }

    public void CloseActivatedEyes(EyeClosingEndTurnAction action)
    {
        foreach (Eye eye in BoardManager.Instance.GetReceivers())
        {
            if (!eye.IsClosed && eye.GetReceivedIntensity() >= 1)
            {
                action.Eyes.Add(eye);
                eye.Close();
                _closedEyes++;
            }
        }
        UpdateScore();
    }

    public void ReopenActivatedEyes(EyeClosingEndTurnAction action)
    {
        foreach (Eye eye in action.Eyes)
        {
            eye.Open();
            _closedEyes--;
        }
        UpdateScore();
    }
}
