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

    public int _score = 0;

    private void Start()
    {
    }

    public void UpdateCrystalsAndBombsState()
    {
        foreach (Orb crystal in BoardManagerShredder.Instance.GetAllCrystals())
        {
            crystal.UpdateState();
        }
        foreach (BadOrb bomb in BoardManagerShredder.Instance.GetAllBombs())
        {
            bomb.UpdateState();
        }
    }

    public void UpdateScore()
    {
        foreach (Orb crystal in BoardManagerShredder.Instance.GetAllCrystals())
        {
            if (crystal.HP == 0)
            {
                _score++;
                crystal.Destroy();
            }
        }

#if !DEDICATED_SERVER
        UIManagerGame.Instance.UpdateScoreInt(_score);
#endif
    }

    public override bool CheckGameOver() // Unused
    {
        return CheckGameOverConveyorPhase() || CheckGameOverLaserPhase();
    }

    public bool CheckGameOverLaserPhase()
    {
        bool isGameOver = false;
        foreach (BadOrb bomb in BoardManagerShredder.Instance.GetAllBombs())
        {
            if (bomb.HP == 0)
            {
                isGameOver = true;
                bomb.Destroy();
            }
        }
        if (isGameOver) TriggerGameOver(1);
        return isGameOver;
    }

    public bool CheckGameOverConveyorPhase()
    {
        if (BoardManagerShredder.Instance.GetOrbsOnShreddingTiles().Count() > 0)
        {
            TriggerGameOver(1);
            return true;
        }
        return false;
    }

    public static void Shuffle<T>(IList<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    public void GeneratePiecesOnTopConveyors(int TurnNumber)
    {
        int conveyorWidth = BoardManagerShredder.Instance.ConveyorWidth;
        List<PieceName> pieces = Enumerable.Repeat(PieceName.None, conveyorWidth).ToList();

        switch(TurnNumber)
        {
            case < 10:
                pieces[0] = PieceName.Orb;
                break;
            case < 20:
                pieces[0] = PieceName.Orb;
                pieces[1] = PieceName.Orb;
                break;
            case < 30:
                pieces[0] = PieceName.Orb;
                pieces[1] = PieceName.Orb;
                pieces[2] = PieceName.BadOrb;
                break;
            default:
                break;
        }

        Shuffle(pieces);
        /*
        List<int> ints = new List<int>();
        for (int i = 0; i < BoardManagerShredder.Instance.ConveyorWidth; i++) { ints.Add(i); }
        Shuffle(ints);
        Queue<int> queue = new Queue<int>(ints);

        queue.Dequeue();
        pieces[rd] = PieceName.Orb;*/
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
