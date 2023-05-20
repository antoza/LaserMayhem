using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPieceUpdate : MonoBehaviour
{
    [SerializeField]
    private GameObject[] m_PiecesSelection;

    [SerializeField]
    private PiecesData m_PiecesData;

    public void UpdatePieces()
    {
        //Move pieces
        for(int i = 0; i < m_PiecesSelection.Length; i++)
        {
            if (!m_PiecesSelection[i].activeSelf)
            {
                for (int j = i+1; j < m_PiecesSelection.Length; j++)
                {
                    if (m_PiecesSelection[j].activeSelf)
                    {
                        m_PiecesSelection[i].SetActive(true);
                        m_PiecesSelection[i].GetComponent<UISelectPiece>().UpdatePiece(m_PiecesSelection[j].GetComponent<UISelectPiece>().GetPiece());
                        m_PiecesSelection[j].SetActive(false);
                        break;
                    }
                    
                }
            }
            
        }

        //Add new ones
        for(int i = 0; i < m_PiecesSelection.Length; i++)
        {
            if (!m_PiecesSelection[i].activeSelf)
            {
                m_PiecesSelection[i].SetActive(true);
                m_PiecesSelection[i].GetComponent<UISelectPiece>().UpdatePiece(m_PiecesData.GetRandomPiece());
            }
        }
    }
}
