using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

#nullable enable
public abstract class Piece : MonoBehaviour
{
    [field: SerializeField]
    public GameObject m_Prefab { get; protected set; }     // Peut-être ne plus rendre la pièce singleton, mais avoir une fonction "GetOriginalPiece"
    //public Sprite m_Sprite { get; protected set; }
    public static Piece Instance { get; protected set; }

    public static T GetInstance<T>() where T : Piece, new()
    {
        if (Instance == null)
        {
            Instance = new T();
        }

        return Instance as T;
    }

    public Sprite GetSprite()
    {
        return m_Prefab.GetComponent<SpriteRenderer>().sprite;
    }

    public abstract IEnumerable<Vector2Int> ComputeNewDirections(Vector2Int sourceDirection);

    public static implicit operator bool(Piece piece) { return piece != null; }
}
