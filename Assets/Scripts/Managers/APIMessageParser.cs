using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class APIMessageParser : MonoBehaviour
{
    public static void CallAppropriateFunction(string message)
    {
        Queue<string> parsedMessage = new Queue<string>(message.Split('+'));
        string request = parsedMessage.Dequeue();
        switch (request)
        {
            case "Connect":
                TemporaryFunctionToConnectPlayer(parsedMessage.Dequeue(), parsedMessage.Dequeue(), int.Parse(parsedMessage.Dequeue()));
                break;
            case "JoinGame":
                TemporaryFunctionToJoinGame(parsedMessage.Dequeue(), int.Parse(parsedMessage.Dequeue()));
                break;
#if DEDICATED_SERVER
            case "SetServerID":
                MenuMessageManager.Instance.SetInstanceID(parsedMessage.Dequeue());
                break;
            case "ChangeScene":
                TemporaryFunctionToStartServer(parsedMessage.Dequeue());
                break;
#endif
            default:
                throw new Exception();
        }
    }

    private async static void TemporaryFunctionToStartServer(string scene)
    {
        await RelayManager.Instance.CreateRelay();
        MenuMessageManager.Instance.StartServer("SampleScene");// scene);
    }

    private async static void TemporaryFunctionToConnectPlayer(string playerID, string username, int playerMMR)
    {
        MenuMessageManager.Instance.SetInstanceID(playerID);
        MenuSelection.Instance.ChangeMenu(Menus.Main);
    }
    // TODO : corriger la confusion entre le playerID in game et le playerID de la database
    private async static void TemporaryFunctionToJoinGame(string joinCode, int playerID) 
    {
        PlayerID.playerID = playerID;
        await RelayManager.Instance.JoinRelay(joinCode);
        MenuMessageManager.Instance.StartClient();
    }
}
