using Mono.CompilerServices.SymbolWriter;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

#nullable enable

public class GameModeRPG : GameMode
{
    public HashSet<int>? playersAlive;

    public override void Initialise()
    {
        playersAlive = new();
        for (int i = 0; i < DataManager.Instance.Rules.NumberOfPlayers; i++)
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
                TurnManager.Instance.StartLaserPhase();
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
            case (SelectionTile, BoardTile):
                if (!VerifyPlacement(playerData, (SelectionTile)sourceTile, (BoardTile)targetTile)) return false;
                break;
#if DEBUG
            case (InfiniteTile, BoardTile):
                if (!VerifyCheatPlacement((InfiniteTile)sourceTile, (BoardTile)targetTile)) return false;
                break;
#endif
            case (BoardTile, BoardTile):
                if (!VerifyMovement(playerData, (BoardTile)sourceTile, (BoardTile)targetTile)) return false;
                break;
            case (BoardTile, TrashTile):
                if (!VerifyDeletion(playerData, (BoardTile)sourceTile)) return false;
                break;
            default:
                return false;
        }
        return true;
    }

    public void ExecuteMovePieceAction(MovePieceAction action)
    {
        Tile sourceTile = action.SourceTile;
        Tile targetTile = action.TargetTile;
        PlayerData playerData = action.PlayerData;

        switch ((sourceTile!, targetTile))
        {
            case (SelectionTile, BoardTile):
                ExecutePlacement(playerData, (SelectionTile)sourceTile, (BoardTile)targetTile);
                break;
#if DEBUG
            case (InfiniteTile, BoardTile):
                ExecuteCheatPlacement((InfiniteTile)sourceTile, (BoardTile)targetTile);
                break;
#endif
            case (BoardTile, BoardTile):
                ExecuteMovement(playerData, (BoardTile)sourceTile, (BoardTile)targetTile);
                break;
            case (BoardTile, TrashTile):
                action.SourcePiece = sourceTile.Piece;
                ExecuteDeletion(playerData, (BoardTile)sourceTile);
                break;
            default:
                // TODO : On peut rajouter un Throw Exception
                break;
        }

        RewindManager.Instance.AddAction(action);
        //LaserManager.Instance.UpdateLaser(true);
    }

    public void RevertMovePieceAction(MovePieceAction action)
    {
        Tile sourceTile = action.SourceTile;
        Tile targetTile = action.TargetTile;
        PlayerData playerData = action.PlayerData;

        switch ((sourceTile!, targetTile))
        {
            case (SelectionTile, BoardTile):
                playerData.PlayerEconomy.RefundPlacement(((SelectionTile)sourceTile).cost);
                targetTile.Piece!.IsPlayedThisTurn = false;
                sourceTile.Piece = targetTile.Piece;
                break;
#if DEBUG
            case (InfiniteTile, BoardTile):
                targetTile.DestroyPiece();
                break;
#endif
            case (BoardTile, BoardTile):
                if (!targetTile.Piece!.IsPlayedThisTurn) playerData.PlayerEconomy.RefundMovement();
                sourceTile.Piece = targetTile.Piece;
                break;

            case (BoardTile, TrashTile):
                playerData.PlayerEconomy.RefundDeletion();
                sourceTile.Piece = action.SourcePiece;
                break;

            default:
                break;
        }

       // LaserManager.Instance.UpdateLaser(true);
    }

    public bool VerifyPlacement(PlayerData playerData, SelectionTile sourceTile, BoardTile targetTile)
    {
        if (sourceTile.Piece == null) return false;
        if (targetTile.Piece != null) return false;
        if (!playerData.PlayerEconomy.HasEnoughMana(sourceTile.cost))
        {
#if !DEDICATED_SERVER
            UIManager.Instance.DisplayError("You don't have enough mana");
#endif
            return false;
        }
        return true;
    }

    public void ExecutePlacement(PlayerData playerData, SelectionTile sourceTile, BoardTile targetTile)
    {
        targetTile.Piece = sourceTile.Piece;
        targetTile.Piece!.IsPlayedThisTurn = true;
        playerData.PlayerEconomy.PayForPlacement(sourceTile.cost);
    }

#if DEBUG
    public bool VerifyCheatPlacement(InfiniteTile sourceTile, BoardTile targetTile)
    {
        if (sourceTile.Piece == null) return false;
        if (targetTile.Piece != null) return false;
        return true;
    }

    public void ExecuteCheatPlacement(InfiniteTile sourceTile, BoardTile targetTile)
    {
        targetTile.InstantiatePiece(sourceTile.Piece!);
    }
#endif

    public bool VerifyMovement(PlayerData playerData, BoardTile sourceTile, BoardTile targetTile)
    {
        if (sourceTile.Piece == null) return false;
        if (targetTile.Piece != null) return false;
        if (sourceTile == targetTile) return false;
        if (!sourceTile.Piece!.IsPlayedThisTurn)
        {
            if (!playerData.PlayerEconomy.HasEnoughManaForMovement())
            {
#if !DEDICATED_SERVER
                UIManager.Instance.DisplayError("You don't have enough mana");
#endif
                return false;
            }
            if (!targetTile.IsCloseEnoughFrom(sourceTile, 1))
            {
#if !DEDICATED_SERVER
                UIManager.Instance.DisplayError("You can't move a piece too far away");
#endif
                return false;
            }
        }
        return true;
    }

    public void ExecuteMovement(PlayerData playerData, BoardTile sourceTile, BoardTile targetTile)
    {
        targetTile.Piece = sourceTile.Piece;
        if (!targetTile.Piece!.IsPlayedThisTurn) playerData.PlayerEconomy.PayForMovement();
    }

    public bool VerifyDeletion(PlayerData playerData, BoardTile sourceTile)
    {
        if (sourceTile.Piece == null) return false;
        if (!playerData.PlayerEconomy.HasEnoughManaForDeletion())
        {
#if !DEDICATED_SERVER
            UIManager.Instance.DisplayError("You don't have enough mana");
#endif
            return false;
        }
        return true;
    }

    public void ExecuteDeletion(PlayerData playerData, BoardTile sourceTile)
    {
        sourceTile.Piece = null;
        playerData.PlayerEconomy.PayForDeletion();
    }
}
