using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public abstract class Tile : MonoBehaviour
{
    protected DataManager m_DataManager;
    public float positionX, positionY;
    public float scaleWidth, scaleHeight;
    [field: SerializeField]
    private Piece? m_startingPiece;
    public Piece? m_Piece { get; private set; }

    void Start()
    {
        m_DataManager = FindObjectOfType<DataManager>();
        transform.position = Vector2.right * positionX + Vector2.up * positionY;
        transform.localScale = Vector2.right * scaleWidth + Vector2.up * scaleHeight;
        SetColor();
        UpdatePiece(m_startingPiece);
    }

    public abstract void SetColor();

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (m_Piece) m_DataManager.PlayersManager.GetCurrentPlayer().PlayerActions.SetSourceTile(this);
        };
        if (Input.GetMouseButtonUp(0))
        {
            m_DataManager.PlayersManager.GetCurrentPlayer().PlayerActions.CmdDoAction(this);
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
            m_Piece.transform.SetParent(transform);
            m_Piece.name = transform.name + "'s_Piece";
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
