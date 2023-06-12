using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class BoardTile : MonoBehaviour
{
    private DataManager m_DataManager;
    public int x, y;
    public float positionX, positionY;
    public float scaleWidth, scaleHeight;
    public Piece? m_Piece { get; private set; }

    void Start()
    {
        m_DataManager = FindObjectOfType<DataManager>();
        this.transform.position = Vector2.right * positionX + Vector2.up * positionY;
        this.transform.localScale = Vector2.right * scaleWidth + Vector2.up * scaleHeight;
        if ((x + y) % 2 == 0)
        {
            this.GetComponent<SpriteRenderer>().color = Color.black;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().color = Color.grey;
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_DataManager.PlayersManager.GetCurrentPlayer().PlayerActions.ProcessTileClicked((x, y));
        };
    }

    public void UpdatePiece(Piece? piece)
    {
        if (m_Piece)
        {
            Destroy(m_Piece!.gameObject);
        }
        if (piece)
        {
            GameObject newPieceGameObject = Instantiate(piece!.gameObject);
            m_Piece = newPieceGameObject.GetComponent<Piece>();
            m_Piece.transform.SetParent(this.transform);
            m_Piece.name = "Piece_" + x + "_" + y;
            m_Piece.transform.position = Vector2.right * positionX + Vector2.up * positionY;
            m_Piece.transform.localScale = Vector2.right * scaleWidth + Vector2.up * scaleHeight;
            m_Piece.parentTile = this;
        }
        else
        {
            m_Piece = null;
        }
    }
}
