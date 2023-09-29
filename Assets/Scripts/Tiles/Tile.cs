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
    public GameObject? m_PieceGameObject { get; private set; }
    [field: SerializeField]
    private GameObject m_MouseOver;


    void Start()
    {
        m_DataManager = FindObjectOfType<DataManager>();
        transform.position = Vector2.right * positionX + Vector2.up * positionY;
        transform.localScale = Vector2.right * scaleWidth + Vector2.up * scaleHeight;
        SetColor();
        UpdatePiece(m_startingPiece);
    }

    public abstract void SetColor();

    protected virtual void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (m_Piece)
            {
                m_DataManager.PlayersManager.GetLocalPlayer().PlayerActions.SetSourceTile(this);
                m_Piece.m_Prefab.GetComponent<Animator>().SetTrigger("PieceClicked");
            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            m_DataManager.PlayersManager.GetLocalPlayer().PlayerActions.PrepareMoveToDestinationTile(this);
        }
    }

    private void OnMouseEnter()
    {
        m_MouseOver.SetActive(true);
    }

    private void OnMouseExit()
    {
        m_MouseOver.SetActive(false);
    }

    public void UpdatePiece(Piece? piece)
    {
        if (m_Piece != null)
        {
            Destroy(m_PieceGameObject);
        }
        if (piece)
        {
            m_Piece = piece;
            GameObject newPieceGameObject = Instantiate(piece!.m_Prefab);
            m_PieceGameObject = newPieceGameObject;
            newPieceGameObject.transform.SetParent(transform);
            newPieceGameObject.name = transform.name + "'s_Piece";
            newPieceGameObject.transform.position = Vector2.right * positionX + Vector2.up * positionY;
            newPieceGameObject.transform.localScale = Vector2.right * scaleWidth + Vector2.up * scaleHeight;
            newPieceGameObject.GetComponent<Animator>().SetTrigger("PiecePlaced");
        }
        else
        {
            m_Piece = null;
        }
    }
}
