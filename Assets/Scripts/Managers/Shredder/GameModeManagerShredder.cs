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

    [SerializeField]
    private SelectionTile _dividerTile;
    [SerializeField]
    private int _dividerCooldown;
    private int _dividerRemainingCooldown;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("bestScore"))
        {
            PlayerPrefs.SetInt("bestScore", 0);
        }
        ResetDividerCooldown();
    }

    public void UpdateCrystalsAndBombsState()
    {
        foreach (Gem crystal in BoardManagerShredder.Instance.GetAllCrystals())
        {
            crystal.UpdateState();
        }
        foreach (TNT bomb in BoardManagerShredder.Instance.GetAllBombs())
        {
            bomb.UpdateState();
        }
    }

    public void UpdateScore()
    {
        foreach (Gem crystal in BoardManagerShredder.Instance.GetAllCrystals())
        {
            if (crystal.HP == 0)
            {
                _score++;
                crystal.Destroy();
                DecrementDividerCooldown(1);
            }
        }

#if !DEDICATED_SERVER
        UIManagerGame.Instance.UpdateScoreInt(_score);
#endif
    }

    public void DecrementDividerCooldown(int amount)
    {
        if (_dividerRemainingCooldown == 0) return;
        _dividerRemainingCooldown -= amount;
        if (_dividerRemainingCooldown < 0) _dividerRemainingCooldown = 0;
        UIManagerGame.Instance.UpdateDividerCooldownIndicator(_dividerRemainingCooldown);
        if (_dividerRemainingCooldown == 0) _dividerTile.InstantiatePiece(PieceName.MirrorDivider);
    }

    public void ResetDividerCooldown()
    {
        _dividerRemainingCooldown = _dividerCooldown;
        UIManagerGame.Instance.UpdateDividerCooldownIndicator(_dividerRemainingCooldown);
    }

    public override bool CheckGameOver() // Unused
    {
        return CheckGameOverConveyorPhase() || CheckGameOverLaserPhase();
    }

    public bool CheckGameOverLaserPhase()
    {
        bool isGameOver = false;
        foreach (TNT bomb in BoardManagerShredder.Instance.GetAllBombs())
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

    public override void TriggerGameOver(int? winner)
    {
        int bestScore = PlayerPrefs.GetInt("bestScore");
        if (bestScore < _score)
        {
            PlayerPrefs.SetInt("bestScore", _score);
            bestScore = _score;
        }
        UIManagerGame.Instance.TriggerGameOverShredder(_score, bestScore);
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

    public bool IsTurnSkipped(int turnNumber)
    {
        if (turnNumber < 3) return true;
        return false;
    }

    public void GeneratePiecesOnTopConveyors(int turnNumber)
    {
        int conveyorWidth = BoardManagerShredder.Instance.ConveyorWidth;
        List<PieceName> pieces = Enumerable.Repeat(PieceName.None, conveyorWidth).ToList();

        switch(turnNumber)
        {
            case < 10:
                pieces[0] = PieceName.GreenGem;
                if (turnNumber == 5) pieces[1] = PieceName.GreenGem;
                break;
            case < 20:
                pieces[0] = PieceName.GreenGem;
                if (turnNumber % 2 == 0) pieces[1] = PieceName.GreenGem;
                if (turnNumber == 15) pieces[2] = PieceName.TNT;
                break;
            case < 40:
                pieces[0] = PieceName.GreenGem;
                pieces[1] = PieceName.GreenGem;
                if (turnNumber % 4 == 0) pieces[2] = PieceName.TNT;
                break;
            case < 60:
                pieces[0] = PieceName.GreenGem;
                pieces[1] = PieceName.PurpleGem;
                if (turnNumber % 4 == 0) pieces[2] = PieceName.TNT;
                break;
            case < 100:
                pieces[0] = PieceName.GreenGem;
                pieces[1] = PieceName.YellowGem;
                if (turnNumber % 4 == 0) pieces[2] = PieceName.TNT;
                break;
            default:
                break;
        }

        do { Shuffle(pieces); } while (pieces[BoardManagerShredder.Instance.ConveyorWidth / 2] == PieceName.TNT);
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
            case (InfiniteTile, ConveyorBoardTile):
                ExecutePlacement(playerData, (InfiniteTile)sourceTile, (ConveyorBoardTile)targetTile);
                break;
            case (SelectionTile, ConveyorBoardTile):
                ExecuteDividerPlacement(playerData, (SelectionTile)sourceTile, (ConveyorBoardTile)targetTile);
                break;
            default:
                // TODO : On peut rajouter un Throw Exception
                break;
        }
    }

    public bool VerifyPlacement(PlayerData playerData, SelectionTile sourceTile, ConveyorBoardTile targetTile)
    {
        if (sourceTile.Piece == null) return false;
        if (targetTile.Piece != null) return false;
        return true;
    }

    public void ExecutePlacement(PlayerData playerData, InfiniteTile sourceTile, ConveyorBoardTile targetTile)
    {
        targetTile.Piece = sourceTile.Piece!;
        targetTile.Piece!.GetComponent<Animator>().SetTrigger("PiecePlaced");
        sourceTile.InstantiatePiece(targetTile.Piece!);
        LocalPlayerManager.Instance.CreateAndVerifyEndTurnAction();
    }

    public void ExecuteDividerPlacement(PlayerData playerData, SelectionTile sourceTile, ConveyorBoardTile targetTile)
    {
        targetTile.Piece = sourceTile.Piece!;
        targetTile.Piece!.GetComponent<Animator>().SetTrigger("PiecePlaced");
        ResetDividerCooldown();
        LocalPlayerManager.Instance.CreateAndVerifyEndTurnAction();
    }
}
