using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using System;
using UnityEngine.Assertions;
using System.Net;
using Unity.Collections;

#nullable enable
public class BoardManagerSolo : BoardManager
{
    public static new BoardManagerSolo Instance => (BoardManagerSolo)BoardManager.Instance;
}
