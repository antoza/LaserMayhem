using System;
using Unity.VisualScripting;
using UnityEngine;

public sealed class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    [field: SerializeField]
    public Rules Rules { get; private set; }
    public GameMode GameMode { get; private set; }

    //Laser Templates
    [field: SerializeField]
    public GameObject LaserTemplate { get; private set; }
    [field: SerializeField]
    public GameObject LaserPredictionTemplate { get; private set; }
    [field: SerializeField]
    public GameObject LaserContainer { get; private set; }
    [field: SerializeField]
    public MouseFollower MouseFollower { get; private set; }
    [field: SerializeField]
    public GameObject MouseOverSelectioner { get; private set; }

    private void Awake()
    {
        Instance = this;

        BoardManager.SetInstance(Instantiate(new GameObject("Board")));
        LaserManager.SetInstance(LaserTemplate, LaserPredictionTemplate, LaserContainer);
        PlayersManager.SetInstance();
        TurnManager.SetInstance();
        RewindManager.SetInstance();
        CreateGameMode();
    }

    private void CreateGameMode()
    {
        Type type = Type.GetType("GameMode" + Rules.GameModeName);
        if (type != null && type.IsSubclassOf(typeof(GameMode)))
        {
            GameMode = (GameMode)Activator.CreateInstance(type);
            GameMode.Initialise();
        }
        else
        {
            Debug.Log("La classe spécifiée n'est pas valide.");
        }
    }

    //On ne fait aucune action dans le Awake. Si besoin il suffit de recréer un start dans le scriptable voulu et de l'appeller dans le start du dataManager.
    //Void TurnManager
    private void Start()
    {
        GetInitialParameters();
        TurnManager.GetInstance().Start();
    }

    private void GetInitialParameters()
    {
        if (GameInitialParameters.playerNames != null)
            PlayersManager.GetInstance().SetPlayerNames(GameInitialParameters.playerNames);
        if (GameInitialParameters.playerNames != null)
            Rules = GameInitialParameters.Rules;
    }
}
