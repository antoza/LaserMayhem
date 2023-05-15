using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class PiecesData : ScriptableObject
{
    [SerializeField]
    Piece[] m_Pieces;



    public Piece GetRandomPiece()
    {
        int rd = Random.Range(0, m_Pieces.Length);
        return m_Pieces[rd];
    }
}