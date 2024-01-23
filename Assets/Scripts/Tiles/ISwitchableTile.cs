using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;

#nullable enable
public interface ISwitchableTile
{/*
    [field: SerializeField]
    private List<PieceName> _piecesHiddenOnStart;
    [SerializeField]
    protected Vector2Int _spot;
    //public float scaleWidth, scaleHeight;
    [SerializeField]
    private PieceName m_startingPiece;

    private Piece? _piece;
    public Piece? Piece
    {
        get => _piece;
        set
        {
            if (_piece == value) return; // Do nothing to avoid infinite loop
            if (_piece != null)
            {
                _piece!.ParentTile = null;
                _piece!.gameObject.SetActive(false);
                //_piece.transform.SetParent(transform.parent);
            }

            _piece = value;
            if (_piece != null)
            {
                if(_piece.ParentTile != null) _piece.ParentTile.Piece = null;
                _piece!.ParentTile = this;
                _piece.gameObject.SetActive(true);
                _piece.transform.SetParent(transform);
                _piece.name = transform.name + "'s_Piece";
                _piece.transform.position = transform.position;
                //_piece.transform.localScale = transform.localScale;
                _piece.GetComponent<SpriteRenderer>().sortingLayerName = GetComponent<SpriteRenderer>().sortingLayerName;
                _piece.GetComponent<Animator>().SetTrigger("PiecePlaced");
                SoundManager.Instance.PlayPlacePieceSound();
            }
        }
    }

    [field: SerializeField]
    private GameObject? _mouseOverIndicatorPrefab;
    private GameObject? _mouseOverIndicator;
    [field: SerializeField]
    private GameObject? _pulsatingIndicatorPrefab;
    private GameObject? _pulsatingIndicator;
    public int m_id { get; private set; }

    void Start()
    {
        InitTilePositions();
        InstantiatePiece(m_startingPiece);
#if !DEDICATED_SERVER
        InitMouseOverIndicator();
        InitPulsatingIndicator();
#endif
        SetColor();
    }

    private void RegisterTile()
    {
        BoardManager.Instance.RegisterTile(_spot, this);
    }

    protected virtual void InitTilePositions()
    {
        if (!belongsToBoard) return;
        int sign = GameInitialParameters.localPlayerID == 1 ? -1 : 1;
        transform.position = sign * (Vector2.right * _spot.x + Vector2.up * _spot.y);
        //transform.localScale = Vector2.right * scaleWidth + Vector2.up * scaleHeight;
    }

    public virtual void SetColor() { }

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
         if (Piece) Piece!.GetComponent<Animator>().SetTrigger("PieceClicked");
    }

    protected virtual void DoOnMouseButtonUp()
    {
         LocalPlayerManager.Instance.CreateAndVerifyMovePieceAction(this);
    }

    private void InitMouseOverIndicator()
    {
        if (_mouseOverIndicatorPrefab != null)
        {
            _mouseOverIndicator = Instantiate(_mouseOverIndicatorPrefab, transform);
            _mouseOverIndicator.transform.position = transform.position;
            _mouseOverIndicator.GetComponent<SpriteRenderer>().sortingLayerName = GetComponent<SpriteRenderer>().sortingLayerName;
            _mouseOverIndicator!.SetActive(false);
        }
    }

    private void OnMouseEnter()
    {
        if (_mouseOverIndicator != null)
        {
            _mouseOverIndicator!.SetActive(true);
        }
    }

    private void OnMouseExit()
    {
        if (_mouseOverIndicator != null)
        {
            _mouseOverIndicator!.SetActive(false);
        }
    }

    private void InitPulsatingIndicator()
    {
        if (_pulsatingIndicatorPrefab != null)
        {
            _pulsatingIndicator = Instantiate(_pulsatingIndicatorPrefab, transform);
            _pulsatingIndicator.transform.position = transform.position;
            _pulsatingIndicator.GetComponent<SpriteRenderer>().sortingLayerName = GetComponent<SpriteRenderer>().sortingLayerName;
            _pulsatingIndicator!.SetActive(false);
        }
    }

    public void StartPulsating()
    {
        if (_pulsatingIndicator != null)
        {
            _pulsatingIndicator!.SetActive(true);
            AnimationState anim = _pulsatingIndicator!.GetComponent<Animation>()["PulsatingIndicator"];
            anim.time = Time.time % anim.length;
            //_pulsatingIndicator!.GetComponent<Animator>().Play(0, -1, _pulsatingIndicatorPrefab!.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime);
        }
    }

    public void StopPulsating()
    {
        if (_pulsatingIndicator != null)
        {
            _pulsatingIndicator!.SetActive(false);
        }
    }
#endif

    public void InstantiatePiece(PieceName pieceName)
    {
        if (pieceName != PieceName.None)
        {
            Assert.IsNull(Piece);
            Piece = PiecePrefabs.Instance.GetPiece(pieceName).InstantiatePiece();
        }
    }

    public void InstantiatePiece(Piece piece)
    {
        InstantiatePiece(PiecePrefabs.Instance.GetPieceNameFromPiece(piece));
    }

    public void DestroyPiece()
    {
        Assert.IsNotNull(Piece);
        Destroy(Piece!.gameObject);
        Piece = null;
    }*/
    /*
    public void TakePieceFromTile(Tile otherTile)
    {
        Piece? piece = otherTile.Piece;
        otherTile.Piece = null;
        Piece = piece;
    }*/
}
