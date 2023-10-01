using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPiece : MonoBehaviour
{
    private DataManager m_DataManager;

    [SerializeField]
    private int m_PieceID;
    [SerializeField]
    public SelectionTile m_Tile;

    public void UpdatePiece(Piece newPiece)
    {
        m_Tile.UpdatePiece(newPiece);
    }
}
