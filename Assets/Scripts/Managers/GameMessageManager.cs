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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
