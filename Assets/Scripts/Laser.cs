using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

#nullable enable
public class Laser : MonoBehaviour
{
    // TODO : revoir comment décrire un laser pour accepter toutes possibilités de transfert d'informations
    public string Type;
    public int Intensity;

    public bool Equals(Laser other)
    {
        return other.Type == Type;
    }
}
