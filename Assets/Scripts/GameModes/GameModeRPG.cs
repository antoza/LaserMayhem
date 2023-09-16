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

    public override void MoveToDestinationTile(Tile? sourceTile, Tile destinationTile, PlayerData playerData)
    {
        if (sourceTile == null)
        {
            return;
        }
        switch ((sourceTile!, destinationTile))
        {
            case (SelectionTile, BoardTile):
                SelectPiece selectPiece = sourceTile.transform.parent.GetComponent<SelectPiece>(); //mettre cost dans selectionTile
                int cost = selectPiece.GetCost();
                if (!playerData.PlayerEconomy.HasEnoughMana(cost))
                {
                    Debug.Log("You don't have enough mana");
                    return;
                }
                if (!playerData.PlayerActions.MovePiece(sourceTile, destinationTile))
                {
                    return;
                }
                playerData.PlayerEconomy.PayForPlacement(cost);
                break;
#if DEBUG
            case (InfiniteTile, BoardTile):
                if (!playerData.PlayerActions.CopyPiece(sourceTile, destinationTile))
                {
                    return;
                }
                break;
#endif
            case (BoardTile, BoardTile):
                if (!playerData.PlayerEconomy.HasEnoughManaForMovement())
                {
                    Debug.Log("You don't have enough mana");
                    return;
                }
                if (!((BoardTile)destinationTile).IsCloseEnoughFrom((BoardTile)sourceTile, 1))
                {
                    Debug.Log("You can't move a piece too far away");
                    return;
                }
                if (!playerData.PlayerActions.MovePiece(sourceTile, destinationTile))
                {
                    return;
                }
                playerData.PlayerEconomy.PayForMovement();
                break;

            case (BoardTile, TrashTile):
                if (!playerData.PlayerEconomy.HasEnoughManaForDeletion())
                {
                    Debug.Log("You don't have enough mana");
                    return;
                }
                if (!playerData.PlayerActions.DeletePiece(sourceTile))
                {
                    return;
                }
                playerData.PlayerEconomy.PayForDeletion();
                break;

            default:
                break;
        }
    }
}
