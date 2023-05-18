using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISelectPiece : MonoBehaviour
{
    [SerializeField]
    private int m_PieceID;

    public void OnClick()
    {
        Debug.Log("I took piece " + m_PieceID.ToString());
    }
}
