using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionTile : Tile
{

    [field: SerializeField]
    public int cost { get; set; }
    public override void SetColor()
    {
        GetComponent<SpriteRenderer>().color = Color.black;
    }

    protected override void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (m_DataManager.PlayersManager.GetCurrentPlayer().PlayerEconomy.HasEnoughMana(cost))
            {
                if (m_Piece) m_DataManager.PlayersManager.GetLocalPlayer().PlayerActions.SetSourceTile(this);
            }
            else
            {
                Debug.Log("You Don't have enough money peasant !");
            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            m_DataManager.PlayersManager.GetLocalPlayer().PlayerActions.CmdDoAction(this);
        }
    }
}
