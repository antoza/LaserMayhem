using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu]
public class PiecesData : ScriptableObject
{
    [SerializeField]
    GameObject[] m_Tiles;



    public GameObject GetRandomPiece()
    {
        int rd = Random.Range(0, m_Tiles.Length);
        return m_Tiles[rd];
    }
}