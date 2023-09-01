using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceUpdate : MonoBehaviour
{
    [SerializeField]
    private SelectPiece[] m_PiecesSelection;
    [SerializeField]
    private DataManager m_DataManager;

    [SerializeField]
    private PiecesData m_PiecesData;

    private void Start()
    {
        foreach(SelectPiece pieceSelection in m_PiecesSelection)
        {
            pieceSelection.UpdatePiece(m_PiecesData.GetRandomPiece().GetComponent<Piece>());
        }
    }

    public void UpdatePieces()
    {
        //Move pieces
        for(int i = 0; i < m_PiecesSelection.Length; i++)
        {
            Tile currentTile = m_PiecesSelection[i].m_Tile;
            if (!currentTile.m_Piece)
            {
                for (int j = i + 1; j < m_PiecesSelection.Length; j++)
                {
                    Tile otherTile = m_PiecesSelection[j].m_Tile;
                    if (otherTile.m_Piece)
                    {
                        currentTile.UpdatePiece(otherTile.m_Piece);
                        otherTile.UpdatePiece(null);
                        break;
                    }
                }
            }
        }

        //Add new ones
        for(int i = 0; i < m_PiecesSelection.Length; i++)
        {
            Tile currentTile = m_PiecesSelection[i].m_Tile;
            if (!currentTile.m_Piece)
            {
                currentTile.UpdatePiece(m_PiecesData.GetRandomPiece().GetComponent<Piece>());
            }
        }
    }
}
