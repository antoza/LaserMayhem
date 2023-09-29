using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionTile : Tile
{

    [field: SerializeField]
    public int cost;
    public override void SetColor()
    {
        GetComponent<SpriteRenderer>().color = Color.black;
    }

    protected override void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (m_DataManager.PlayersManager.GetLocalPlayer().PlayerEconomy.HasEnoughMana(cost))
            {
                if (m_Piece) m_DataManager.PlayersManager.GetLocalPlayer().PlayerActions.SetSourceTile(this);
                m_PieceGameObject.GetComponent<Animator>().SetTrigger("PieceClicked");
            }
            else
            {
                Debug.Log("You Don't have enough money peasant !");
            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            m_DataManager.PlayersManager.GetLocalPlayer().PlayerActions.PrepareMoveToDestinationTile(this);
        }
    }
}
