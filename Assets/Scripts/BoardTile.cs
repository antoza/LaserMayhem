using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class BoardTile : MonoBehaviour
{
    private DataManager m_DataManager;
    public int x, y;
    public float positionX, positionY;
    public float scaleWidth, scaleHeight;
    public GameObject? piecePrefab { get; private set; }

    void Start()
    {
        m_DataManager = FindObjectOfType<DataManager>();
        this.transform.position = Vector2.right * positionX + Vector2.up * positionY;
        this.transform.localScale = Vector2.right * scaleWidth + Vector2.up * scaleHeight;
        if ((x + y) % 2 == 0)
        {
            this.GetComponent<SpriteRenderer>().color = Color.black;
        }
        else
        {
            this.GetComponent<SpriteRenderer>().color = Color.grey;
        }
    }

    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_DataManager.PlayersManager.GetCurrentPlayer().PlayerActions.ProcessTileClicked(x, y);
        };
    }

    public void UpdatePiece(GameObject? prefab)
    {
        Debug.Log("tozr");
        if (prefab)
        {
            Debug.Log("tozr2");
            piecePrefab = Instantiate(prefab!);
            piecePrefab.transform.SetParent(this.transform);
            //piecePrefab.name = "Piece_" + x + "_" + y;
            piecePrefab.transform.position = Vector2.right * positionX + Vector2.up * positionY;
            piecePrefab.transform.localScale = Vector2.right * scaleWidth + Vector2.up * scaleHeight;
        }
        else
        {
            piecePrefab = null;
        }
    }
}
