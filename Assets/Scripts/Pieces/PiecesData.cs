using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class PiecesData : ScriptableObject
{
    [SerializeField]
    PieceName[] m_Pieces;



    public PieceName GetRandomPiece()
    {
        int rd = Random.Range(0, m_Pieces.Length);
        return m_Pieces[rd];
    }
}