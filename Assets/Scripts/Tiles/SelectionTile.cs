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

    public override void InitTilePositions()
    {
        transform.position = Vector2.right * positionX + Vector2.up * positionY;
        transform.localScale = Vector2.right * scaleWidth + Vector2.up * scaleHeight;
        if(GameInitialParameters.localPlayerID == 1)
        {
            transform.localScale = new(transform.localScale.x*-1, transform.localScale.y*-1);
        }
    }

#if !DEDICATED_SERVER
    protected override bool VerifyOnMouseButtonDown()
    {
        if (!base.VerifyOnMouseButtonDown()) return false;
        if (!LocalPlayerManager.Instance.LocalPlayer.PlayerEconomy.HasEnoughMana(cost))
        {
            UIManager.Instance.DisplayError("You don't have enough mana to buy this piece.");
            return false;
        }
        return true;
    }
#endif
}
