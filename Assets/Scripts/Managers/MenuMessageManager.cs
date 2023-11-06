using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMessageManager : MonoBehaviour
{
    private TcpClient clientSocket;

    [field: SerializeField]
    private Rules Rules;

    [field: SerializeField]
    private string sceneName1 = "SampleScene";
    [field: SerializeField]
    private string sceneName2 = "BranchSimon";
    
    void Start()
    {
        clientSocket = new TcpClient();
        clientSocket.Connect("127.0.0.1", 8888); // TODO : Mettre les bonnes valeurs ici
    }

    void SendData(string data)
    {
        try
        {
            NetworkStream serverStream = clientSocket.GetStream();
            byte[] outStream = Encoding.ASCII.GetBytes(data);
            serverStream.Write(outStream, 0, outStream.Length);
            serverStream.Flush();
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
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
        SendData("15+SearchGame+RPG");
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
        GetGameInitialParameters();
    }

    public void GetGameInitialParameters()
    {
        string[] names = { "moi", "adversaire" };
        SetGameInitialParameters(Rules, 0, names);
    }

    public void GetServerGameInitialParameters()
    {
        string[] names = { "moi", "adversaire" };
        SetGameInitialParameters(Rules, -1, names);
    }

    public void SetGameInitialParameters(Rules rules, int localPlayerID, string[] names)
    {
        GameInitialParameters.Rules = rules;
        GameInitialParameters.localPlayerID = localPlayerID;
        GameInitialParameters.playerNames = names;
    }
}
