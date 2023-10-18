using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuMessageManager : MonoBehaviour
{
    [field: SerializeField]
    private Rules Rules;

    [field: SerializeField]
    private string sceneName1 = "SampleScene";
    [field: SerializeField]
    private string sceneName2 = "BranchSimon";

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
