using Mono.CompilerServices.SymbolWriter;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

#nullable enable

public class GameModeManagerShredder : GameModeManager
{
    public static new GameModeManagerShredder Instance => (GameModeManagerShredder)GameModeManager.Instance;

    private void Start()
    {
    }

    public override bool CheckGameOver()
    {
        //TriggerGameOver(playersAlive!.Single());
        return false;
    }

    public void GeneratePiecesOnTopConveyors(int TurnNumber)
    {
        int conveyorWidth = BoardManagerShredder.Instance.ConveyorWidth;
        List<PieceName> pieces = Enumerable.Repeat(PieceName.None, conveyorWidth).ToList();

        int rd = Random.Range(0, conveyorWidth);
        pieces[rd] = PieceName.Orb;
        BoardManagerShredder.Instance.SpawnOnTopConveyors(pieces);
    }

    public override bool VerifyAction(PlayerAction action)
    {
        if (!base.VerifyAction(action)) return false;
        switch (action)
        {
            case MovePieceAction:
                return VerifyMovePieceAction((MovePieceAction)action);
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
            case EndTurnAction:
                TurnManager.Instance.EndTurn((EndTurnAction)action);
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
            case (SelectionTile, ConveyorBoardTile):
                if (!VerifyPlacement(playerData, (SelectionTile)sourceTile, (ConveyorBoardTile)targetTile)) return false;
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
            case (SelectionTile, ConveyorBoardTile):
                ExecutePlacement(playerData, (SelectionTile)sourceTile, (ConveyorBoardTile)targetTile);
                break;
            default:
                // TODO : On peut rajouter un Throw Exception
                break;
        }

        BoardManager.Instance.DisplayPredictionLaser();
    }

    public bool VerifyPlacement(PlayerData playerData, SelectionTile sourceTile, ConveyorBoardTile targetTile)
    {
        if (sourceTile.Piece == null) return false;
        if (targetTile.Piece != null) return false;
        return true;
    }

    public void ExecutePlacement(PlayerData playerData, SelectionTile sourceTile, ConveyorBoardTile targetTile)
    {
        targetTile.InstantiatePiece(sourceTile.Piece!);
        LocalPlayerManager.Instance.CreateAndVerifyEndTurnAction();
    }
}
