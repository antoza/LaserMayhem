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

    public abstract IEnumerable<Vector2Int> ComputeNewDirections(Vector2Int sourceDirection);

    public Sprite GetSprite()
    {
        return m_Sprite;
    }
}
