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
public abstract class Weakness : Receiver // TODO : renommer en Receiver
{
    private PlayerData _weakPlayer;
    public PlayerData WeakPlayer {
        get => _weakPlayer;
        set {
            _weakPlayer = value;
            GetComponent<SpriteRenderer>().color = value.PlayerID == 0 ? new Color(.2f, .4f, 1f, 1f) : new Color(1f, .2f, .2f, 1f);
        }
    }

    public override int GetReceivedIntensity()
    {
        return directions.Values.Sum();
    }
}
