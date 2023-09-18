using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameMessageManager : MonoBehaviour
{
    private DataManager DM;

    public GameMessageManager(DataManager dataManager)
    {
        DM = dataManager;
    }

    public class Notification : MonoBehaviour//MessageBase
    {
        public string oui;
    }
}
