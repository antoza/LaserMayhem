using UnityEngine;
using UnityEngine.Assertions;

#nullable enable
public abstract class Tile : MonoBehaviour
{
    public float positionX, positionY;
    public float scaleWidth, scaleHeight;
    [field: SerializeField]
    private PieceName m_startingPiece;
    public Piece? m_Piece { get; private set; }
    [field: SerializeField]
    private GameObject? m_MouseOver;
    public int m_id { get; private set; }

    void Start()
    {
        SetColor();
        InitTilePositions();
        InstantiatePiece(m_startingPiece);
    }
    
    public virtual void InitTilePositions()
    {
        transform.position = Vector2.right * positionX + Vector2.up * positionY;
        transform.localScale = Vector2.right * scaleWidth + Vector2.up * scaleHeight;
    }

    public abstract void SetColor();

    protected virtual void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            PlayersManager.Instance.GetLocalPlayer().PlayerActions.SetSourceTile(this);
            if (m_Piece)
            {
                m_Piece!.GetComponent<Animator>().SetTrigger("PieceClicked");
            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            PlayersManager.Instance.GetLocalPlayer().PlayerActions.CreateAndVerifyMovePieceAction(this);
        }
    }

    private void OnMouseEnter()
    {
        m_MouseOver!.SetActive(true);
    }

    private void OnMouseExit()
    {
        m_MouseOver!.SetActive(false);
    }

    public void InstantiatePiece(PieceName pieceName)
    {
        if (pieceName != PieceName.None)
        {
            Assert.IsNull(m_Piece);
            UpdatePiece(PiecePrefabs.Instance.GetPiece(pieceName).InstantiatePiece());
        }
    }

    public void InstantiatePiece(Piece piece)
    {
        InstantiatePiece(PiecePrefabs.Instance.GetPieceNameFromPiece(piece));
    }

    public void DestroyPiece()
    {
        Assert.IsNotNull(m_Piece);
        Destroy(m_Piece!.gameObject);
        m_Piece = null;
    }

    public void UpdatePiece(Piece? piece)
    {
        if (m_Piece)
        {
            m_Piece!.gameObject.SetActive(false);
        }
        m_Piece = piece;
        if (piece != null)
        {
            piece.gameObject.SetActive(true);
            piece.transform.SetParent(transform);
            piece.name = transform.name + "'s_Piece";
            piece.transform.position = Vector2.right * positionX + Vector2.up * positionY;
            piece.transform.localScale = Vector2.right * scaleWidth + Vector2.up * scaleHeight;
            piece.GetComponent<Animator>().SetTrigger("PiecePlaced");
            SoundManager.Instance.PlayPlacePieceSound();
        }
    }

    public void TakePieceFromTile(Tile otherTile)
    {
        Piece? piece = otherTile.m_Piece;
        otherTile.UpdatePiece(null);
        UpdatePiece(piece);
    }
}
