using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class SenderManager : MonoBehaviour
{
    // TODO : SenderManager est un singleton alors que ReceiverManager est statique, à régler
    public static SenderManager Instance { get; private set; }

    private string instanceID;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
#if DEDICATED_SERVER
        instanceID = "s0";
        RegisterAsServer();
#else
        instanceID = "p0";
#endif
    }

    public void SetInstanceID(string instanceID)
    {
        this.instanceID = instanceID;
    }

    private void SendRequest(string message)
    {
        MenuMessageManager.Instance.SendRequest($"{instanceID}+{message}");
    }

#if DEDICATED_SERVER

    // Server requests

    private void RegisterAsServer()
    {
        SendRequest("RegisterAsServer");
    }

    public void SendRelayCode(string relayCode)
    {
        SendRequest($"SendRelayCode+{relayCode}");
    }

#else

    // Player requests

    public void SignUp(string username, string password)
    {
        Assert.IsTrue(instanceID == "p0");
        SendRequest($"SignUp+{username}+{password}");
    }

    public void LogIn(string username, string password)
    {
        Assert.IsTrue(instanceID == "p0");
        SendRequest($"LogIn+{username}+{password}");
    }

    public void LogOut()
    {
        SendRequest($"LogOut");
    }

    public void SearchGame(string gamemode)
    {
        SendRequest($"SearchGame+{gamemode}");
    }

    public void CancelMatchmaking()
    {
        SendRequest($"CancelMatchmaking");
    }

#endif
}
