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
            transform.localScale = new(transform.localScale.x, transform.localScale.y*-1);
        }
    }

    protected override void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (PlayersManager.Instance.GetLocalPlayer().PlayerEconomy.HasEnoughMana(cost))
            {
                PlayersManager.Instance.GetLocalPlayer().PlayerActions.SetSourceTile(this);
                if (m_Piece) m_Piece!.GetComponent<Animator>().SetTrigger("PieceClicked");
            }
            else
            {
                Debug.Log("You Don't have enough money peasant !");
            }

        }
        if (Input.GetMouseButtonUp(0))
        {
            PlayersManager.Instance.GetLocalPlayer().PlayerActions.CreateAndVerifyMovePieceAction(this);
        }
    }
}
