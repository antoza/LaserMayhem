using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class MenuMessageManager : MonoBehaviour
{
    public static MenuMessageManager Instance { get; private set; }

    private TcpClient tcpClient;
    private NetworkStream stream;

    private string instanceID;

    [field: SerializeField]
    private Rules Rules;

    [field: SerializeField]
    private string sceneName1 = "SampleScene";
    [field: SerializeField]
    private string sceneName2 = "BranchSimon";

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ConnectToServer();
        instanceID = "p0";
#if DEDICATED_SERVER
        instanceID = "s0";
        RegisterAsServer();
#endif
    }
    /*
    public void SendRequest(string request)
    {
        try
        {
            NetworkStream serverStream = clientSocket.GetStream();
            byte[] outStream = Encoding.ASCII.GetBytes(request);
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }*/

    private void ConnectToServer()
    {
        try
        {
            tcpClient = new TcpClient("127.0.0.1", 11586);
            //tcpClient = new TcpClient("92.167.126.212", 11586);
            tcpClient.SendTimeout = 600000;
            tcpClient.ReceiveTimeout = 600000;
            stream = tcpClient.GetStream();
            Debug.Log("Connected to the API");
            ReceiveMessages();
            Task.Run(() => { Task.Delay(10000); SendRequest("trtr"); });
        }
        catch (Exception e)
        {
            Debug.LogError("Error trying to connect to the API: " + e.Message);
        }
    }

    private async void ReceiveMessages()
    {
        byte[] message = new byte[1024];
        int bytesRead;
        while (tcpClient.Connected)
        {
            try
            {
                if (stream.DataAvailable)
                {
                    bytesRead = stream.Read(message, 0, 1024);
                    string receivedMessage = Encoding.ASCII.GetString(message, 0, bytesRead);
                    Debug.Log("Message received from the API: " + receivedMessage);
                    APIMessageParser.CallAppropriateFunction(receivedMessage);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error trying to receive a message from the API: " + e.Message);
                break;
            }
            await Task.Yield();
        }

        DisconnectFromServer();
    }

    private void DisconnectFromServer()
    {
        if (tcpClient != null)
        {
            tcpClient.Close();
            Debug.Log("Disconnected from the API");
        }
    }

    public void SendRequest(string message)
    {
        if (tcpClient != null && tcpClient.Connected)
        {
            try
            {
                byte[] outStream = Encoding.ASCII.GetBytes(instanceID + "+" + message);
                stream.Write(outStream, 0, outStream.Length);
                stream.Flush();
            }
            catch (Exception e)
            {
                Debug.LogError("Error trying to send a message to the API: " + e.Message);
            }
        }
    }

#if DEDICATED_SERVER
    private void RegisterAsServer()
    {
        SendRequest("RegisterAsServer");
    }
#endif

    public void SetInstanceID(string instanceID)
    {
        this.instanceID = instanceID;
    }

    public void StartHost(string SceneName)
    {
        NetworkManager.Singleton.StartHost();
        NetworkManager.Singleton.SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
        GetGameInitialParameters();
    }

    public void StartServer(string SceneName)
    {
        NetworkManager.Singleton.StartServer();
        NetworkManager.Singleton.SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
        GetServerGameInitialParameters();
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        GetGameInitialParameters();
    }

    public void GetGameInitialParameters()
    {
        string[] names = { "Player 0", "Player 1" };
        Debug.Log("Connecting with id player : " + PlayerID.playerID); //TODO : Delete this
        SetGameInitialParameters(Rules, PlayerID.playerID, names);
    }

    public void GetServerGameInitialParameters()
    {
        string[] names = { "Player 0", "Player 1" };
        SetGameInitialParameters(Rules, -1, names);
    }

    public void SetGameInitialParameters(Rules rules, int localPlayerID, string[] names)
    {
        GameInitialParameters.Rules = rules;
        GameInitialParameters.localPlayerID = localPlayerID;
        GameInitialParameters.playerNames = names;
    }
}
