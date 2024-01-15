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
        InitTilePositions();
        InstantiatePiece(m_startingPiece);
        InitMouseOver();
        SetColor();
    }

    private void InitMouseOver()
    {
        if (m_MouseOver != null)
        {
            m_MouseOver = Instantiate(m_MouseOver);
            m_MouseOver.transform.position = transform.position;
            m_MouseOver.GetComponent<SpriteRenderer>().sortingLayerName = GetComponent<SpriteRenderer>().sortingLayerName;
        }
    }

    public virtual void InitTilePositions()
    {
        transform.position = Vector2.right * positionX + Vector2.up * positionY;
        transform.localScale = Vector2.right * scaleWidth + Vector2.up * scaleHeight;
    }

    public abstract void SetColor();

#if !DEDICATED_SERVER
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (VerifyOnMouseButtonDown())
            {
                DoOnMouseButtonDown();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            DoOnMouseButtonUp();
        }
    }

    protected virtual bool VerifyOnMouseButtonDown()
    {
        if (!LocalPlayerManager.Instance.TryToPlay()) return false;
        return true;
    }

    protected virtual void DoOnMouseButtonDown()
    {
         LocalPlayerManager.Instance.SetSourceTile(this);
         if (m_Piece) m_Piece!.GetComponent<Animator>().SetTrigger("PieceClicked");
    }

    protected virtual void DoOnMouseButtonUp()
    {
         LocalPlayerManager.Instance.CreateAndVerifyMovePieceAction(this);
    }

    private void OnMouseEnter()
    {
        m_MouseOver!.SetActive(true);
    }

    private void OnMouseExit()
    {
        m_MouseOver!.SetActive(false);
    }
#endif

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
            piece.GetComponent<SpriteRenderer>().sortingLayerName = GetComponent<SpriteRenderer>().sortingLayerName;
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
