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
#if DEDICATED_SERVER
            case "SetServerID":
                SetServerID(parsedMessage.Dequeue());
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
        MenuMessageManager.Instance.StartServer(scene);
    }
}
