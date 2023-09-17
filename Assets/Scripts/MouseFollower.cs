using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour
{
    [field: SerializeField]
    private SpriteRenderer m_FollowingTileRenderer;
    void Update()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePos);
        worldPosition = new Vector3(worldPosition.x, worldPosition.y, -5);
        this.transform.position = worldPosition;
    }

    public void ChangeFollowingTile(Sprite sprite)
    {
        m_FollowingTileRenderer.sprite = sprite;
    }
}
