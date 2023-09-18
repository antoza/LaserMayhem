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
        if (m_DataManager.PlayersManager.GetCurrentPlayer().PlayerEconomy.HasEnoughMana(cost))
        {
            base.OnMouseOver();
        }
        else
        {
            //There will be a sound or anim to show it's not possible
            Debug.Log("You don't have enough money");
        }
        
    }
}
