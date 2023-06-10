using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Piece : ScriptableObject
{
    [field : SerializeField]
    protected Sprite m_Sprite;
    [field : SerializeField]
    public GameObject m_Prefab { get; private set; }

    public abstract (int, int)[] ComputeNewDirections((int, int) sourceDirection);

    public Sprite GetSprite()
    {
        return m_Sprite;
    }
}
