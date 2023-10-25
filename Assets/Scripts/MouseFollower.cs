using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class MouseFollower : MonoBehaviour
{
    [field: SerializeField]
    private SpriteRenderer? m_FollowingTileRenderer;

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        worldPosition = new Vector3(worldPosition.x, worldPosition.y, -5);
        this.transform.position = worldPosition;
        if (Input.GetMouseButtonUp(0))
        {
            PlayersManager.Instance.GetLocalPlayer().PlayerActions.ResetSourceTile();
        }
    }

    public void ChangeFollowingTile(Tile? tile)
    {
        if (tile != null && tile.m_Piece!)
        {
            m_FollowingTileRenderer!.sprite = tile!.m_Piece!.GetSprite();
        }
        else
        {
            m_FollowingTileRenderer!.sprite = null;
        }
    }
}
