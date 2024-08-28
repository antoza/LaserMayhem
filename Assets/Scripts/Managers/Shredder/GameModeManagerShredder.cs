using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SocialPlatforms.Impl;

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
        TinySauce.OnGameStarted(0);
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
                _score += crystal.Value;
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
        TinySauce.OnGameFinished(_score);
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

        /*switch(turnNumber)
        {
            case < 10:
                pieces[0] = PieceName.GreenGem;
                if (turnNumber % 5 == 4) pieces[1] = PieceName.GreenGem;
                break;
            case < 20:
                pieces[0] = PieceName.GreenGem;
                if (turnNumber % 2 == 0) pieces[1] = PieceName.GreenGem;
                if (turnNumber % 5 == 0) pieces[2] = PieceName.TNT;
                break;
            case < 30:
                pieces[0] = PieceName.GreenGem;
                if (turnNumber % 5 == 0) pieces[1] = PieceName.PurpleGem;
                if (turnNumber % 4 == 0) pieces[2] = PieceName.TNT;
                break;
            case < 40:
                pieces[0] = PieceName.GreenGem;
                if (turnNumber % 5 == 0) pieces[1] = PieceName.PurpleGem;
                if (turnNumber % 4 == 0) pieces[2] = PieceName.TNT;
                break;
            case < 60:
                pieces[0] = PieceName.GreenGem;
                if (turnNumber % 2 == 0) pieces[1] = PieceName.PurpleGem;
                if (turnNumber % 4 == 0) pieces[2] = PieceName.TNT;
                break;
            case < 100:
                pieces[0] = PieceName.GreenGem;
                pieces[1] = PieceName.YellowGem;
                if (turnNumber % 4 == 0) pieces[2] = PieceName.TNT;
                break;
            default:
                break;
        }*/

        int[] piecesInt;
        switch (turnNumber)
        {
            case 00: piecesInt = new int[] { 1 }; break;
            case 01: piecesInt = new int[] { 1 }; break;
            case 02: piecesInt = new int[] { 1 }; break;
            case 03: piecesInt = new int[] { 1 }; break;
            case 04: piecesInt = new int[] { 1, 1 }; break;
            case 05: piecesInt = new int[] { 1 }; break;
            case 06: piecesInt = new int[] { 1 }; break;
            case 07: piecesInt = new int[] { 1, 1 }; break;
            case 08: piecesInt = new int[] { 1 }; break;
            case 09: piecesInt = new int[] { 1, 1 }; break;

            case 10: piecesInt = new int[] { 1, 0 }; break;
            case 11: piecesInt = new int[] { 1 }; break;
            case 12: piecesInt = new int[] { 1, 1 }; break;
            case 13: piecesInt = new int[] { 1, 0 }; break;
            case 14: piecesInt = new int[] { 1, 1 }; break;
            case 15: piecesInt = new int[] { 1 }; break;
            case 16: piecesInt = new int[] { 1, 1 }; break;
            case 17: piecesInt = new int[] { 1, 0 }; break;
            case 18: piecesInt = new int[] { 1, 1 }; break;
            case 19: piecesInt = new int[] { 1, 0 }; break;

            case 20: piecesInt = new int[] { 2 }; break;
            case 21: piecesInt = new int[] { 1, 0 }; break;
            case 22: piecesInt = new int[] { 1, 1 }; break;
            case 23: piecesInt = new int[] { 2, 0 }; break;
            case 24: piecesInt = new int[] { 1, 1 }; break;
            case 25: piecesInt = new int[] { 1, 1, 0 }; break;
            case 26: piecesInt = new int[] { 1, 2 }; break;
            case 27: piecesInt = new int[] { 1, 0 }; break;
            case 28: piecesInt = new int[] { 1, 2 }; break;
            case 29: piecesInt = new int[] { 1, 1, 0 }; break;

            case 30: piecesInt = new int[] { 2, 2 }; break;
            case 31: piecesInt = new int[] { 1, 0 }; break;
            case 32: piecesInt = new int[] { 1, 2 }; break;
            case 33: piecesInt = new int[] { 1, 0, 0 }; break;
            case 34: piecesInt = new int[] { 1, 1, 1 }; break;
            case 35: piecesInt = new int[] { 2, 2 }; break;
            case 36: piecesInt = new int[] { 1, 1, 0 }; break;
            case 37: piecesInt = new int[] { 1, 2 }; break;
            case 38: piecesInt = new int[] { 2, 0, 0 }; break;
            case 39: piecesInt = new int[] { 1, 2 }; break;

            case 40: piecesInt = new int[] { 1, 3 }; break;
            case 41: piecesInt = new int[] { 2, 0 }; break;
            case 42: piecesInt = new int[] { 1, 2 }; break;
            case 43: piecesInt = new int[] { 3, 0 }; break;
            case 44: piecesInt = new int[] { 2, 2 }; break;
            case 45: piecesInt = new int[] { 3, 0, 0 }; break;
            case 46: piecesInt = new int[] { 2, 3 }; break;
            case 47: piecesInt = new int[] { 1, 1, 1, 1 }; break;
            case 48: piecesInt = new int[] { 3, 0, 0 }; break;
            case 49: piecesInt = new int[] { 2, 3 }; break;

            case 50: piecesInt = new int[] { 1, 1, 0, 0, 0 }; break;
            case 51: piecesInt = new int[] { 3, 3 }; break;
            case 52: piecesInt = new int[] { 2, 2, 0, 0 }; break;
            case 53: piecesInt = new int[] { 1, 2, 3 }; break;
            case 54: piecesInt = new int[] { 2, 2, 0 }; break;
            case 55: piecesInt = new int[] { 1, 3, 3 }; break;
            case 56: piecesInt = new int[] { 2, 3, 0 }; break;
            case 57: piecesInt = new int[] { 2, 3, 3 }; break;
            case 58: piecesInt = new int[] { 1, 2, 2, 3, 3 }; break;
            case 59: piecesInt = new int[] { 2, 3, 0, 0, 0 }; break;

            case < 1000: piecesInt = new int[] { 3, 3, 3, 3, 3 }; break;

            default: piecesInt = new int[] { }; break;
        }

        for (int i = 0; i < piecesInt.Length; i++)
        {
            switch (piecesInt[i])
            {
                case 0:
                    pieces[i] = PieceName.TNT; break;
                case 1:
                    pieces[i] = PieceName.GreenGem; break;
                case 2:
                    pieces[i] = PieceName.PurpleGem; break;
                case 3:
                    pieces[i] = PieceName.YellowGem; break;
            }
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

    public override void ExecuteAction(GameAction action)
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
        LocalPlayerManager.Instance.ResetSourceTile();
    }

    public override void RevertAction(GameAction action)
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
        // TODO : je n'aime pas cette approche, il faudrait que Clear / DisplayLaser soient appel�s lors d'une modification du board
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
