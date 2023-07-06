using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public abstract class Piece : MonoBehaviour
{
    [field: SerializeField]
    protected Sprite m_Sprite;
    private DataManager m_DataManager;
    public Tile? parentTile;

    void Start()
    {
        m_DataManager = FindObjectOfType<DataManager>();
    }

    public abstract (int, int)[] ComputeNewDirections((int, int) sourceDirection);

    public Sprite GetSprite()
    {
        return m_Sprite;
    }
}
