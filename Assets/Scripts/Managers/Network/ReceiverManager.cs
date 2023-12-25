using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceiverManager : MonoBehaviour
{
    public static void CallAppropriateFunction(string message)
    {
        Queue<string> parsedMessage = new Queue<string>(message.Split('+'));
        string request = parsedMessage.Dequeue();
        switch (request)
        {
#if DEDICATED_SERVER
            case "SetServerID":
                SenderManager.Instance.SetInstanceID(parsedMessage.Dequeue());
                break;
            case "ChangeScene":
                TemporaryFunctionToStartServer(parsedMessage.Dequeue(), int.Parse(parsedMessage.Dequeue()), int.Parse(parsedMessage.Dequeue()));
                break;
#else
            case "LogIn":
                TemporaryFunctionToLogIn(parsedMessage.Dequeue(), parsedMessage.Dequeue(), int.Parse(parsedMessage.Dequeue()));
                break;
            case "LogOut":
                TemporaryFunctionToLogOut(int.Parse(parsedMessage.Dequeue()));
                break;
            case "SearchGame":
                MenusManager.Instance.ChangeMenu(Menus.Matchmaking);
                break;
            case "CancelMatchmaking":
                MenusManager.Instance.ChangeMenu(Menus.GameMode);
                break;
            case "JoinGame":
                TemporaryFunctionToJoinGame(parsedMessage.Dequeue(), int.Parse(parsedMessage.Dequeue()), int.Parse(parsedMessage.Dequeue()));
                break;
#endif
            default:
                throw new Exception();
        }
    }

#if DEDICATED_SERVER

    private async static void TemporaryFunctionToStartServer(string scene, int secret1, int secret2)
    {
        await RelayManager.Instance.CreateRelay();
        List<int> playerSecrets = new List<int>();
        playerSecrets.Add(secret1);
        playerSecrets.Add(secret2);
        MenuMessageManager.Instance.StartServer(scene, playerSecrets);
    }

#else

    private static void TemporaryFunctionToLogIn(string playerID, string username, int playerMMR)
    {
        SenderManager.Instance.SetInstanceID(playerID);
        MenusManager.Instance.ChangeMenu(Menus.Main);
    }

    private static void TemporaryFunctionToLogOut(int reasonNumber)
    {
        if (reasonNumber == 34) Debug.Log("Another device has logged into your account.");
        SenderManager.Instance.SetInstanceID("p0");
        MenusManager.Instance.ChangeMenu(Menus.Connection);
    }
    // TODO : corriger la confusion entre le playerID in game et le playerID de la database
    private async static void TemporaryFunctionToJoinGame(string joinCode, int playerID, int playerSecret) 
    {
        PlayerID.playerID = playerID;
        await RelayManager.Instance.JoinRelay(joinCode);
        MenuMessageManager.Instance.StartClient(playerSecret);
    }

#endif
}
