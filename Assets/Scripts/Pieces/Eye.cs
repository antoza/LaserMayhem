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
public class Eye : Receiver // TODO : renommer en Receiver
{
    public override int GetReceivedIntensity()
    {
        return directions.Values.Sum() == 0 ? 0 : 1;
    }
}
