using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MessageManager : MonoBehaviour
{
    private DataManager DM;

    public MessageManager(DataManager dataManager)
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

    public void GetInitialData()
    {
        SetInitialData("moi", "adversaire");
    }

    public void SetInitialData(string myName, string opponentName)
    {
        DM.PlayersManager.GetPlayer(0).m_name = myName;
        DM.PlayersManager.GetPlayer(1).m_name = opponentName;
    }
}
