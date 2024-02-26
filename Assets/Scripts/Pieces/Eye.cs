using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UIElements;

#nullable enable
public class Eye : Receiver
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private Sprite openEyeSprite;
    [SerializeField]
    private Sprite closedEyeSprite;

    public bool IsClosed = false;

    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override int GetReceivedIntensity()
    {
        return directions.Values.Sum() == 0 ? 0 : 1;
    }

    public void Close()
    {
        IsClosed = true;
        _spriteRenderer.sprite = closedEyeSprite;
    }

    public void Open()
    {
        IsClosed = false;
        _spriteRenderer.sprite = openEyeSprite;
    }
}
