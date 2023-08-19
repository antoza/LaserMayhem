using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceUpdate : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_PiecesSelection;
    [SerializeField]
    private DataManager m_DataManager;

    [SerializeField]
    private PiecesData m_PiecesData;

    private void Start()
    {
        foreach(GameObject pieceSelection in m_PiecesSelection)
        {
            pieceSelection.GetComponent<SelectPiece>().UpdatePiece(m_PiecesData.GetRandomPiece().GetComponent<Piece>());
        }
    }

    public void UpdatePieces()
    {
        //Move pieces
        for(int i = 0; i < m_PiecesSelection.Length; i++)
        {
            Tile currentTile = m_PiecesSelection[i].GetComponent<SelectPiece>().m_Tile;
            for (int j = i+1; j < m_PiecesSelection.Length; j++)
            {
                Tile otherTile = m_PiecesSelection[j].GetComponent<SelectPiece>().m_Tile;
                currentTile.UpdatePiece(otherTile.m_Piece);
                otherTile.setColor();
                break;
                    
            }
            
        }

        //Add new ones
        for(int i = 0; i < m_PiecesSelection.Length; i++)
        {
            Tile currentTile = m_PiecesSelection[i].GetComponent<SelectPiece>().m_Tile;
            currentTile.UpdatePiece(m_PiecesData.GetRandomPiece().GetComponent<Piece>());
        }
    }
}
