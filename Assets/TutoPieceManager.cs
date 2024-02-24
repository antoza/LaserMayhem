using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoPieceManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> PiecesPages;

    [SerializeField]
    private GameObject PieceFocus;

    private int CurrentPage = 0;


    public void PrintPiece(int pieceID)
    {
        CurrentPage = pieceID;
        PieceFocus.SetActive(true);
        PiecesPages[CurrentPage].SetActive(true);
    }

    public void RemovePrint()
    {
        PieceFocus.SetActive(false);
        PiecesPages[CurrentPage].SetActive(false);
    }
}
