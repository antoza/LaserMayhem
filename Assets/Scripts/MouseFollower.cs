using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#nullable enable
public class MouseFollower : MonoBehaviour
{
    [field: SerializeField]
    private DataManager DM;
    [field: SerializeField]
    private SpriteRenderer m_FollowingTileRenderer;

    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        worldPosition = new Vector3(worldPosition.x, worldPosition.y, -5);
        this.transform.position = worldPosition;
        if (Input.GetMouseButtonUp(0))
        {
            DM.PlayersManager.GetLocalPlayer().PlayerActions.ResetSourceTile();
        }
    }

    public void ChangeFollowingTile(Tile? tile)
    {
        if (tile && tile!.m_Piece)
        {
            m_FollowingTileRenderer.sprite = tile!.m_Piece!.m_Sprite!;
        }
        else
        {
            m_FollowingTileRenderer.sprite = null;
        }
    }
}
