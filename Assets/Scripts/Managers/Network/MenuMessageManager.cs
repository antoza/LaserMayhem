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
    private bool isTcpReady = false;
    // TODO : supprimer
    [field: SerializeField]
    private Rules Rules;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        ConnectToServer();
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
#if DEBUG || DEDICATED_SERVER
            tcpClient = new TcpClient("127.0.0.1", 11586);
#else
            tcpClient = new TcpClient("92.167.126.212", 11586);
#endif
            stream = tcpClient.GetStream();
            Debug.Log("Connected to the API");
            ReceiveMessages();
            PingServerRegularly();
        }
        catch (Exception e)
        {
            Debug.LogError("Error trying to connect to the API: " + e.Message);
#if DEDICATED_SERVER
            Application.Quit();
#endif
        }
        isTcpReady = true;
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
                    string receivedMessagesRaw = Encoding.ASCII.GetString(message, 0, bytesRead);
                    string[] receivedMessages = receivedMessagesRaw.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                    foreach (string receivedMessage in receivedMessages)
                    {
                        Debug.Log("Message received from the API: " + receivedMessage);
                        ReceiverManager.CallAppropriateFunction(receivedMessage);
                    }

                }
            }
            catch (Exception e)
            {
                Debug.LogError("Error trying to receive a message from the API: " + e.Message);
                break;
            }
            await Task.Yield();
        }
    }

    private async void PingServerRegularly()
    {
        while (tcpClient.Connected)
        {
            await Task.Delay(1000); // Pings the server every second
            try
            {
                byte[] outStream = Encoding.ASCII.GetBytes("0\n");
                stream.Write(outStream, 0, outStream.Length);
                stream.Flush();
            }
            catch
            {
                DisconnectFromServer();
            }
        }
    }

    private void DisconnectFromServer()
    {
        if (tcpClient != null)
        {
            tcpClient.Close();
            Debug.Log("Disconnected from the API");
#if DEDICATED_SERVER
            Application.Quit();
#endif
        }
    }

    public async void SendRequest(string message)
    {
        while (!isTcpReady) await Task.Delay(100);
        if (tcpClient != null && tcpClient.Connected)
        {
            try
            {
                byte[] outStream = Encoding.ASCII.GetBytes(message + '\n');
                stream.Write(outStream, 0, outStream.Length);
                stream.Flush();
            }
            catch (Exception e)
            {
                Debug.LogError("Error trying to send a message to the API: " + e.Message);
            }
        }
    }

    public void StartServer(string SceneName, List<int> playerSecrets)
    {
        GameInitialParameters.playerSecrets = playerSecrets;
        GetServerGameInitialParameters();
        NetworkManager.Singleton.StartServer();
        NetworkManager.Singleton.SceneManager.LoadScene(SceneName, LoadSceneMode.Single);
    }

    public void StartClient(int playerSecret)
    {
        GameInitialParameters.playerSecret = playerSecret;
        GetGameInitialParameters();
        NetworkManager.Singleton.StartClient();
    }

    // TODO : supprimer ces fonctions inutiles
    public void GetGameInitialParameters()
    {
        SetGameInitialParameters(Rules, PlayerID.playerID);
    }

    public void GetServerGameInitialParameters()
    {
        SetGameInitialParameters(Rules, -1);
    }

    public void SetGameInitialParameters(Rules rules, int localPlayerID)
    {
        GameInitialParameters.Rules = rules;
        GameInitialParameters.localPlayerID = localPlayerID;
    }
}
