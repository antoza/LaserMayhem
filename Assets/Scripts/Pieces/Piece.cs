using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

#nullable enable
public abstract class Piece : MonoBehaviour
{
    [field: SerializeField]
    public Sprite m_Sprite { get; protected set; }
    private DataManager m_DataManager;
    public Tile? parentTile;

    void Start()
    {
        m_DataManager = FindObjectOfType<DataManager>();
        m_Sprite = this.transform.GetComponent<SpriteRenderer>().sprite;
    }

    public abstract IEnumerable<Vector2Int> ComputeNewDirections(Vector2Int sourceDirection);
}
