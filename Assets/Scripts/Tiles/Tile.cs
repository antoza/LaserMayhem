using UnityEngine;

#nullable enable
public abstract class Tile : MonoBehaviour
{
    public float positionX, positionY;
    public float scaleWidth, scaleHeight;
    [field: SerializeField]
    private Piece? m_startingPiece;
    public Piece? m_Piece { get; private set; }
    public GameObject? m_PieceGameObject { get; private set; }
    [field: SerializeField]
    private GameObject? m_MouseOver;
    public int m_id { get; private set; }


    void Start()
    {
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
            PlayersManager.GetInstance().GetLocalPlayer().PlayerActions.SetSourceTile(this);
            if (m_Piece != null)
            {
                m_Piece.m_Prefab!.GetComponent<Animator>().SetTrigger("PieceClicked");
            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            PlayersManager.GetInstance().GetLocalPlayer().PlayerActions.CreateAndVerifyMovePieceAction(this);
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

    public void UpdatePiece(Piece? piece)
    {
        if (m_Piece != null)
        {
            Destroy(m_PieceGameObject);
        }
        if (piece != null)
        {
            m_Piece = piece;
            GameObject newPieceGameObject = Instantiate(piece!.m_Prefab!);
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
