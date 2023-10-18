using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

public abstract class Piece : MonoBehaviour
{
    public Piece InstantiatePiece()
    {
        return Instantiate(this).GetComponent<Piece>();
    }

    public Sprite GetSprite()
    {
        return GetComponent<SpriteRenderer>().sprite;
    }

    public abstract IEnumerable<Vector2Int> ComputeNewDirections(Vector2Int sourceDirection);
}
