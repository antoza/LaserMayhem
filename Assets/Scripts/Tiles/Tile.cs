using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

#nullable enable
public abstract class Tile : MonoBehaviour
{
    [SerializeField]
    private PieceName _startingPiece;

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
                SoundManager.Instance.PlayPlacePieceSound();
            }
        }
    }
    // TODO : Créer les pièces indiquées dans _pieceStorage et les enregistrer dans PieceStorage
    [SerializeField]
    private PieceName _pieceStorage;
    [HideInInspector]
    public List<Piece> PieceStorage = new List<Piece>();

    [field: SerializeField]
    private GameObject? _mouseOverIndicatorPrefab;
    private GameObject? _mouseOverIndicator;
    [field: SerializeField]
    private GameObject? _pulsatingIndicatorPrefab;
    private GameObject? _pulsatingIndicator;
    public int m_id { get; private set; }

    void Start()
    {
        //InitTilePositions();
        InstantiatePiece(_startingPiece);
#if !DEDICATED_SERVER
        InitMouseOverIndicator();
        InitPulsatingIndicator();
#endif
        SetColor();
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

    public Tile InstantiateTile()
    {
        return Instantiate(this).GetComponent<Tile>();
    }

    public TileName GetTileName()
    {
        return TilePrefabs.Instance.GetTileNameFromTile(this);
    }

    public void InstantiatePiece(PieceName pieceName)
    {
        if (pieceName != PieceName.None)
        {
            Piece = PiecePrefabs.Instance.GetPiece(pieceName).InstantiatePiece(gameObject);
        }
    }

    public void InstantiatePiece(Piece piece)
    {
        InstantiatePiece(piece.GetPieceName());
    }

    public void StorePiece(PieceName pieceName)
    {
        if (pieceName != PieceName.None)
        {
            Piece pieceStored = PiecePrefabs.Instance.GetPiece(pieceName).InstantiatePiece(gameObject);
            PieceStorage.Add(pieceStored);
            pieceStored.gameObject.SetActive(false);
        }
    }

    public void DestroyPiece()
    {
        Assert.IsNotNull(Piece);
        Destroy(Piece!.gameObject);
        Piece = null;
    }
}
