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
    [SerializeField]
    private int m_Cost;

    private void Awake()
    {
        m_DataManager = FindObjectOfType<DataManager>();
        m_Cost = -m_PieceID - 1;
    }

    public void UpdatePiece(Piece newPiece)
    {
        m_Tile.UpdatePiece(newPiece);
    }

    //Getter && Setter
    public int GetCost()
    {
        return m_Cost;
    }
}
