using Mono.CompilerServices.SymbolWriter;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

// TODO : rendre Manager

#nullable enable
public class SelectionTilesUpdate : MonoBehaviour
{
    [SerializeField]
    private SelectionTile[] m_SelectionTiles;

    [SerializeField]
    private RandomPieceGenerator m_PiecesData;

    public void ServerUpdateSelectionPieces()
    {
        Assert.AreEqual(GameInitialParameters.localPlayerID, -1, "This function is reserved for the server");
        int firstEmptySelectionTileIndex = MovePiecesToTheLeft();
        int missingPiecesCount = m_SelectionTiles.Length - firstEmptySelectionTileIndex;
        List<PieceName> piecesList = new List<PieceName>(missingPiecesCount);

        List<PieceName> excludedPieces = new List<PieceName>();
        foreach (SelectionTile tile in m_SelectionTiles)
        {
            if (tile.Piece != null)
            {
                excludedPieces.Add(tile.Piece!.GetPieceName());
            }
        }
        for (int i = 0; i < missingPiecesCount; i++)
        {
            PieceName randomPiece = m_PiecesData.GetRandomButUniquePiece(excludedPieces);
            piecesList.Add(randomPiece);
            excludedPieces.Add(randomPiece);
        }

        ServerSendPiecesListAction action = new ServerSendPiecesListAction(piecesList);
        AddNewPieces(firstEmptySelectionTileIndex, action.PiecesList);

        ((ServerSendActionsManager)SendActionsManager.Instance).ExecuteActionAndSendItToAllPlayers(action);
    }

    public void ClientUpdateSelectionPieces(ServerSendPiecesListAction action)
    {
        Assert.AreNotEqual(GameInitialParameters.localPlayerID, -1, "This function is reserved for the clients");
        int firstEmptySelectionTileIndex = MovePiecesToTheLeft();
        AddNewPieces(firstEmptySelectionTileIndex, action.PiecesList);
    }

    private int MovePiecesToTheLeft()
    {
        int firstEmptySelectionTileIndex = 0;

        for (int i = 0; i < m_SelectionTiles.Length; i++)
        {
            Tile currentTile = m_SelectionTiles[i];
            if (currentTile.Piece)
            {
                firstEmptySelectionTileIndex++;
            }
            else
            {
                for (int j = i + 1; j < m_SelectionTiles.Length; j++)
                {
                    Tile otherTile = m_SelectionTiles[j];
                    if (otherTile.Piece)
                    {
                        currentTile.Piece = otherTile.Piece;
                        firstEmptySelectionTileIndex++;
                        break;
                    }
                }
            }
        }
        return firstEmptySelectionTileIndex;
    }

    private void AddNewPieces(int firstEmptySelectionTileIndex, List<PieceName> piecesList)
    {
        Assert.AreEqual(piecesList.Count(), m_SelectionTiles.Length - firstEmptySelectionTileIndex);
        for(int i = 0; i < piecesList.Count(); i++)
        {
            Tile currentTile = m_SelectionTiles[firstEmptySelectionTileIndex + i];
            currentTile.InstantiatePiece(piecesList.ElementAt(i));
        }
    }
}
