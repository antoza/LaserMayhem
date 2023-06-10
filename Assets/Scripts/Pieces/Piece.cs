using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
    [field: SerializeField]
    protected Sprite m_Sprite;
    private DataManager m_DataManager;

    void Start()
    {
        m_DataManager = FindObjectOfType<DataManager>();
    }

    public abstract (int, int)[] ComputeNewDirections((int, int) sourceDirection);

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_DataManager.PlayersManager.GetCurrentPlayer().PlayerActions.m_SelectedPiece = this;
        }
    }

    public Sprite GetSprite()
    {
        return m_Sprite;
    }
}
