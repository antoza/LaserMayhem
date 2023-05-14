using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    public abstract (int, int)[] computeNewDirections((int, int) sourceDirection);
}
