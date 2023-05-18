using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Piece : ScriptableObject
{
    public Sprite m_Sprite;

    public abstract (int, int)[] computeNewDirections((int, int) sourceDirection);
}
