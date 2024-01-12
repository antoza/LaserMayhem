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
                ((EndTurnAction)action).PlayerData.PlayerActions.EndTurn();
                RewindManager.Instance.ClearAllActions();
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
                action.SourcePiece = sourceTile.m_Piece;
                ExecuteDeletion(playerData, (BoardTile)sourceTile);
                break;
            default:
                // TODO : On peut rajouter un Throw Exception
                break;
        }

        RewindManager.Instance.AddAction(action);
        LaserManager.Instance.UpdateLaser(true);
    }

    public void RevertMovePieceAction(MovePieceAction action)
    {
        Tile sourceTile = action.SourceTile;
        Tile targetTile = action.TargetTile;
        PlayerData playerData = action.PlayerData;

        switch ((sourceTile!, targetTile))
        {
            case (SelectionTile, BoardTile):
                int cost = ((SelectionTile)sourceTile).cost;
                sourceTile.TakePieceFromTile(targetTile);
                playerData.PlayerEconomy.RefundPlacement(cost);
                break;
#if DEBUG
            case (InfiniteTile, BoardTile):
                targetTile.DestroyPiece();
                break;
#endif
            case (BoardTile, BoardTile):
                sourceTile.TakePieceFromTile(targetTile);
                playerData.PlayerEconomy.RefundMovement();
                break;

            case (BoardTile, TrashTile):
                sourceTile.UpdatePiece(action.SourcePiece);
                playerData.PlayerEconomy.RefundDeletion();
                break;

            default:
                break;
        }

        LaserManager.Instance.UpdateLaser(true);
    }

    public bool VerifyPlacement(PlayerData playerData, SelectionTile sourceTile, BoardTile targetTile)
    {
        if (sourceTile.m_Piece == null) return false;
        if (targetTile.m_Piece != null) return false;
        if (!playerData.PlayerEconomy.HasEnoughMana(sourceTile.cost))
        {
            Debug.Log("You don't have enough mana");
            return false;
        }
        return true;
    }

    public void ExecutePlacement(PlayerData playerData, SelectionTile sourceTile, BoardTile targetTile)
    {
        targetTile.TakePieceFromTile(sourceTile);
        playerData.PlayerEconomy.PayForPlacement(sourceTile.cost);
    }

#if DEBUG
    public bool VerifyCheatPlacement(InfiniteTile sourceTile, BoardTile targetTile)
    {
        if (sourceTile.m_Piece == null) return false;
        if (targetTile.m_Piece != null) return false;
        return true;
    }

    public void ExecuteCheatPlacement(InfiniteTile sourceTile, BoardTile targetTile)
    {
        targetTile.InstantiatePiece(sourceTile.m_Piece!);
    }
#endif

    public bool VerifyMovement(PlayerData playerData, BoardTile sourceTile, BoardTile targetTile)
    {
        if (sourceTile.m_Piece == null) return false;
        if (targetTile.m_Piece != null) return false;
        if (sourceTile == targetTile) return false;
        if (!playerData.PlayerEconomy.HasEnoughManaForMovement())
        {
            Debug.Log("You don't have enough mana");
            return false;
        }
        if (!targetTile.IsCloseEnoughFrom(sourceTile, 1))
        {
            Debug.Log("You can't move a piece too far away");
            return false;
        }
        return true;
    }

    public void ExecuteMovement(PlayerData playerData, BoardTile sourceTile, BoardTile targetTile)
    {
        targetTile.TakePieceFromTile(sourceTile);
        playerData.PlayerEconomy.PayForMovement();
    }

    public bool VerifyDeletion(PlayerData playerData, BoardTile sourceTile)
    {
        if (sourceTile.m_Piece == null) return false;
        if (!playerData.PlayerEconomy.HasEnoughManaForDeletion())
        {
            Debug.Log("You don't have enough mana");
            return false;
        }
        return true;
    }

    public void ExecuteDeletion(PlayerData playerData, BoardTile sourceTile)
    {
        sourceTile.UpdatePiece(null);
        playerData.PlayerEconomy.PayForDeletion();
    }
}
