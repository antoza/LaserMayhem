using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Piece : ScriptableObject
{
    [field : SerializeField]
    protected Sprite m_Sprite;

    public abstract (int, int)[] computeNewDirections((int, int) sourceDirection);

    public Sprite GetSprite()
    {
        return m_Sprite;
    }
}
