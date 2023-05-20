using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISelectPiece : MonoBehaviour
{
    [SerializeField]
    private int m_PieceID;

    private Piece m_Piece = null;
    private Image m_PieceImage;


    private void Awake()
    {
        m_PieceImage = this.GetComponent<Image>();
    }

    public void OnClick()
    {
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
