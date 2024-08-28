using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;
using UnityEngine.Video;

#nullable enable
public class TNT : Receiver
{
    public int HP;
    [SerializeField]
    private GameObject _explosionPrefab;

    public override void ReceiveLaser(Laser? laser, Vector2Int inDirection)
    {
        base.ReceiveLaser(laser, inDirection);

        ((BoardTile)ParentTile!).TransferLaser(laser, inDirection);
    }

    public override int GetReceivedIntensity()
    {
        return directions.Values.Sum();
    }

    public void UpdateState()
    {
        int receivedIntensity = GetReceivedIntensity();
        if (receivedIntensity > 0)
        {
            HP -= receivedIntensity;
            if (HP < 0) HP = 0;
            Explode();
        }
    }
    public void Explode()
    {
        // This.Color = black
        StartCoroutine(ExplosionCoroutine());
    }

    protected IEnumerator ExplosionCoroutine()
    {
        GameObject explosion = Instantiate(_explosionPrefab, transform);

        while (!explosion.GetComponent<VideoPlayer>().isPrepared)
            yield return new WaitForEndOfFrame();
        //yield return new WaitForSeconds(.05f);
        explosion.transform.position = transform.position;
    }

    public void Destroy()
    {
        ParentTile!.Piece = null;
        Destroy(this);
    }
}
