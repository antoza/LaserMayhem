using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardTile : MonoBehaviour
{
    private DataManager m_DataManager;
    public int x, y;
    public float positionX, positionY;
    public float scaleWidth, scaleHeight;
    // Start is called before the first frame update
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

    // Update is called once per frame
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_DataManager.PlayersManager.GetCurrentPlayer().PlayerActions.ProcessTileClicked(x, y);
        };
    }
}
