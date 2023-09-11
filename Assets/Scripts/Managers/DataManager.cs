using System;
using Unity.VisualScripting;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    [field: SerializeField]
    public Rules Rules { get; private set; }
    public BoardManager BoardManager { get; private set; }
    public LaserManager LaserManager { get; private set; }
    public PlayersManager PlayersManager { get; private set; }
    public TurnManager TurnManager { get; private set; }
    public MessageManager MessageManager { get; private set; }
    public GameMode GameMode { get; private set; }

    //Laser Templates
    [field : SerializeField]
    public GameObject LaserTemplate { get; private set; }
    [field: SerializeField]
    public GameObject LaserPredictionTemplate { get; private set; }
    [field: SerializeField]
    public GameObject LaserContainer { get; private set; }

    private void Awake()
    {
        BoardManager = new BoardManager(this, Instantiate(new GameObject("Board")));
        LaserManager = new LaserManager(this, LaserTemplate, LaserPredictionTemplate, LaserContainer);
        PlayersManager = new PlayersManager(this);
        TurnManager = new TurnManager(this);
        MessageManager = new MessageManager(this);
        CreateGameMode();
        MessageManager.GetInitialData();
    }

    private void CreateGameMode()
    {
        Type type = Type.GetType("GameMode" + Rules.GameModeName);
        if (type != null && type.IsSubclassOf(typeof(GameMode)))
        {
            GameMode = (GameMode)Activator.CreateInstance(type);
            GameMode.SetDamanager(this);
        }
        else
        {
            Debug.Log("La classe spécifiée n'est pas valide.");
        }
    }

    //On ne fait aucune action dans le Awake. Si besoins il suffit de recréer un start dans le scriptable voulu et de l'appeller dans le start du dataManager.
    //Void TurnManager
    private void Start()
    {
        TurnManager.Start();
    }
}
