using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class MenuMessageSerializer
{
    public static MenuMessageSerializer Instance { get; private set; }

    public string AuthenticationPrefix = "0";

    private void Awake()
    {
        Instance = this;
    }

    public void GenerateAuthenticationPrefix(int playerID/*, string authenticationToken*/)
    {
        AuthenticationPrefix = $"{playerID}";
    }

    // Player requests

    public void CreatePlayer(string username, string password)
    {
        Assert.IsTrue(AuthenticationPrefix == "0");
        MenuMessageManager.Instance.SendRequest($"{AuthenticationPrefix}+CreatePlayer+{username}+{password}");
    }

    public void SearchGame(string gamemode)
    {
        MenuMessageManager.Instance.SendRequest($"{AuthenticationPrefix}+SearchGame+{gamemode}");
    }
}
