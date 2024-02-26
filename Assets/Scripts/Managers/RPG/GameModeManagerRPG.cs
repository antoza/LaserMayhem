using Mono.CompilerServices.SymbolWriter;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

#nullable enable

public class GameModeManagerRPG : GameModeManager
{
    public static new GameModeManagerRPG Instance => (GameModeManagerRPG)GameModeManager.Instance;

    public HashSet<int>? playersAlive;

    private void Start()
    {
        playersAlive = new();
        for (int i = 0; i < PlayersManager.Instance.NumberOfPlayers; i++)
        {
            playersAlive.Add(i);
        }
    }

    public void PlayerDied(int playerID)
    {
        playersAlive!.Remove(playerID);
    }

    public override bool CheckGameOver()
    {
        if (playersAlive!.Count() == 0)
        {
            TriggerGameOver(null);
            return true;
        }
        else if (playersAlive!.Count() == 1)
        {
            TriggerGameOver(playersAlive!.Single());
            return true;
        }
        return false;
    }

    public override bool VerifyAction(PlayerAction action)
    {
        if (!base.VerifyAction(action)) return false;
        switch (action)
        {
            case MovePieceAction:
                return VerifyMovePieceAction((MovePieceAction)action);
            case RevertLastActionAction:
            case RevertAllActionsAction:
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
            case ServerSpawnPieceAction:
                ((ServerSpawnPieceAction)action).Tile.InstantiatePiece(((ServerSpawnPieceAction)action).PieceName);
                break;
            case RevertLastActionAction:
                RewindManager.Instance.RevertLastAction();
                break;
            case RevertAllActionsAction:
                RewindManager.Instance.RevertAllActions();
                break;
            case EndTurnAction:
                TurnManager.Instance.EndTurn((EndTurnAction)action);
                break;
            case ServerSendPiecesListAction:
                if (GameInitialParameters.localPlayerID == -1) return;
                FindObjectOfType<SelectionTilesUpdate>().ClientUpdateSelectionPieces((ServerSendPiecesListAction)action);
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
            case (PricedSelectionTile, NormalBoardTile):
                if (!VerifyPlacement(playerData, (PricedSelectionTile)sourceTile, (NormalBoardTile)targetTile)) return false;
                break;
#if DEBUG
            case (InfiniteTile, NormalBoardTile):
                if (!VerifyCheatPlacement((InfiniteTile)sourceTile, (NormalBoardTile)targetTile)) return false;
                break;
#endif
            case (NormalBoardTile, NormalBoardTile):
                if (!VerifyMovement(playerData, (NormalBoardTile)sourceTile, (NormalBoardTile)targetTile)) return false;
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
        // TODO : je n'aime pas cette approche, il faudrait que Clear / DisplayLaser soient appelés lors d'une modification du board
        BoardManager.Instance.ClearLaser();

        Tile sourceTile = action.SourceTile;
        Tile targetTile = action.TargetTile;
        PlayerData playerData = action.PlayerData;

        switch ((sourceTile!, targetTile))
        {
            case (PricedSelectionTile, NormalBoardTile):
                ExecutePlacement(playerData, (PricedSelectionTile)sourceTile, (NormalBoardTile)targetTile);
                break;
#if DEBUG
            case (InfiniteTile, NormalBoardTile):
                ExecuteCheatPlacement((InfiniteTile)sourceTile, (NormalBoardTile)targetTile);
                break;
#endif
            case (NormalBoardTile, NormalBoardTile):
                ExecuteMovement(playerData, (NormalBoardTile)sourceTile, (NormalBoardTile)targetTile);
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
            case (PricedSelectionTile, NormalBoardTile):
                playerData.PlayerEconomy.RefundPlacement(((PricedSelectionTile)sourceTile).cost);
                targetTile.Piece!.IsPlayedThisTurn = false;
                sourceTile.Piece = targetTile.Piece;
                break;
#if DEBUG
            case (InfiniteTile, NormalBoardTile):
                targetTile.DestroyPiece();
                break;
#endif
            case (NormalBoardTile, NormalBoardTile):
                if (!targetTile.Piece!.IsPlayedThisTurn) playerData.PlayerEconomy.RefundMovement();
                sourceTile.Piece = targetTile.Piece;
                break;

            case (NormalBoardTile, TrashTile):
                playerData.PlayerEconomy.RefundDeletion();
                sourceTile.Piece = action.SourcePiece;
                break;

            default:
                break;
        }

        BoardManager.Instance.DisplayPredictionLaser();
    }

    public bool VerifyPlacement(PlayerData playerData, PricedSelectionTile sourceTile, NormalBoardTile targetTile)
    {
        if (sourceTile.Piece == null) return false;
        if (targetTile.Piece != null) return false;
        if (!playerData.PlayerEconomy.HasEnoughMana(sourceTile.cost))
        {
#if !DEDICATED_SERVER
            UIManager.Instance.DisplayErrorMessage("You don't have enough coins");
#endif
            return false;
        }
        return true;
    }

    public void ExecutePlacement(PlayerData playerData, PricedSelectionTile sourceTile, NormalBoardTile targetTile)
    {
        targetTile.Piece = sourceTile.Piece;
        targetTile.Piece!.IsPlayedThisTurn = true;
        playerData.PlayerEconomy.PayForPlacement(sourceTile.cost);
    }

#if DEBUG
    public bool VerifyCheatPlacement(InfiniteTile sourceTile, NormalBoardTile targetTile)
    {
        if (sourceTile.Piece == null) return false;
        if (targetTile.Piece != null) return false;
        return true;
    }

    public void ExecuteCheatPlacement(InfiniteTile sourceTile, NormalBoardTile targetTile)
    {
        targetTile.InstantiatePiece(sourceTile.Piece!);
    }
#endif

    public bool VerifyMovement(PlayerData playerData, NormalBoardTile sourceTile, NormalBoardTile targetTile)
    {
        if (sourceTile.Piece == null) return false;
        if (targetTile.Piece != null) return false;
        if (sourceTile == targetTile) return false;
        if (!sourceTile.Piece!.IsPlayedThisTurn)
        {
            if (!playerData.PlayerEconomy.HasEnoughManaForMovement())
            {
#if !DEDICATED_SERVER
                UIManager.Instance.DisplayErrorMessage("You don't have enough coins");
#endif
                return false;
            }
            if (!targetTile.IsCloseEnoughFrom(sourceTile, 1))
            {
#if !DEDICATED_SERVER
                UIManager.Instance.DisplayErrorMessage("You can't move a piece too far away");
#endif
                return false;
            }
        }
        return true;
    }

    public void ExecuteMovement(PlayerData playerData, NormalBoardTile sourceTile, NormalBoardTile targetTile)
    {
        targetTile.Piece = sourceTile.Piece;
        if (!targetTile.Piece!.IsPlayedThisTurn) playerData.PlayerEconomy.PayForMovement();
    }

    public bool VerifyDeletion(PlayerData playerData, NormalBoardTile sourceTile)
    {
        if (sourceTile.Piece == null) return false;
        if (!playerData.PlayerEconomy.HasEnoughManaForDeletion())
        {
#if !DEDICATED_SERVER
            UIManager.Instance.DisplayErrorMessage("You don't have enough coins");
#endif
            return false;
        }
        return true;
    }

    public void ExecuteDeletion(PlayerData playerData, NormalBoardTile sourceTile)
    {
        sourceTile.Piece = null;
        playerData.PlayerEconomy.PayForDeletion();
    }
}
