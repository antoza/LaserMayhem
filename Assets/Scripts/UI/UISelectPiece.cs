using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectPiece : MonoBehaviour
{
    private DataManager m_DataManager;

    [SerializeField]
    private int m_PieceID;

    [SerializeField]
    private Piece m_Piece = null;
    private Image m_PieceImage;


    private void Awake()
    {
        m_DataManager = FindObjectOfType<DataManager>();
        m_PieceImage = GetComponent<Image>();
    }

    public void OnClick()
    {
        m_DataManager.PlayersManager.GetCurrentPlayer().PlayerActions.m_SelectedPiece = m_Piece;
        this.gameObject.SetActive(false);
    }

    public void UpdatePiece(Piece newPiece)
    {
        m_Piece = newPiece;
        m_PieceImage.sprite = newPiece.GetSprite();

    }

    //Getter && Setter
    public Piece GetPiece() { return m_Piece; }
}
