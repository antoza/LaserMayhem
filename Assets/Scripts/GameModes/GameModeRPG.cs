using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#nullable enable

public class GameModeRPG : GameMode
{
    public HashSet<int> playersAlive;

    public override void Initialise()
    {
        playersAlive = new HashSet<int>();
        for (int i = 0; i < DM.Rules.NumberOfPlayers; i++)
        {
            playersAlive.Add(i);
        }
    }

    public void PlayerDied(int playerID)
    {
        playersAlive.Remove(playerID);
    }

    public override bool CheckGameOver()
    {
        if (playersAlive.Count() == 0)
        {
            TriggerGameOver(null);
            return true;
        }
        else if (playersAlive.Count() == 1)
        {
            TriggerGameOver(playersAlive.Single());
            return true;
        }
        return false;
    }

    public override bool MoveToDestinationTile(Tile? sourceTile, Tile destinationTile, PlayerData playerData)
    {
        if (sourceTile == null)
        {
            return false;
        }
        switch ((sourceTile!, destinationTile))
        {
            case (SelectionTile, BoardTile):
                SelectionTile selectionTile = (SelectionTile)sourceTile;
                int cost = selectionTile.cost;
                if (!playerData.PlayerEconomy.HasEnoughMana(cost))
                {
                    Debug.Log("You don't have enough mana");
                    return false;
                }
                if (!playerData.PlayerActions.MovePiece(sourceTile, destinationTile))
                {
                    return false;
                }
                playerData.PlayerEconomy.PayForPlacement(cost);
                return true;
#if DEBUG
            case (InfiniteTile, BoardTile):
                if (!playerData.PlayerActions.CopyPiece(sourceTile, destinationTile))
                {
                    return false;
                }
                return true;
#endif
            case (BoardTile, BoardTile):
                if (!playerData.PlayerEconomy.HasEnoughManaForMovement())
                {
                    Debug.Log("You don't have enough mana");
                    return false;
                }
                if (!((BoardTile)destinationTile).IsCloseEnoughFrom((BoardTile)sourceTile, 1))
                {
                    Debug.Log("You can't move a piece too far away");
                    return false;
                }
                if (!playerData.PlayerActions.MovePiece(sourceTile, destinationTile))
                {
                    return false;
                }
                playerData.PlayerEconomy.PayForMovement();
                return true;

            case (BoardTile, TrashTile):
                if (!playerData.PlayerEconomy.HasEnoughManaForDeletion())
                {
                    Debug.Log("You don't have enough mana");
                    return false;
                }
                if (!playerData.PlayerActions.DeletePiece(sourceTile))
                {
                    return false;
                }
                playerData.PlayerEconomy.PayForDeletion();
                return true;

            default:
                return false;
        }
    }
}
