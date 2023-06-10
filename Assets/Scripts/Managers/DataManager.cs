using UnityEngine;

public class DataManager : MonoBehaviour
{
    [field: SerializeField]
    public Rules Rules { get; private set; }
    [field: SerializeField]
    public BoardManager BoardManager { get; private set; }
    public LaserManager LaserManager { get; private set; }
    [field: SerializeField]
    public PlayersManager PlayersManager { get; private set; }
    [field: SerializeField]
    public TurnManager TurnManager { get; private set; }
    public GameObject Board { get; private set; }

    //Laser Templates
    [field : SerializeField]
    public GameObject LaserTemplate { get; private set; }
    [field: SerializeField]
    public GameObject LaserPredictionTemplate { get; private set; }
    [field: SerializeField]
    public GameObject LaserContainer { get; private set; }
    [field : SerializeField]
    public float LaserCooldown { get; private set; }

    //Turn Manager
    [field : SerializeField]
    public float skipTurnCooldown { get; private set; }






    // Start : on crée chaque manager (BoardManager = new BoardManager(this)) etc plutôt que de faire des FindObjectWithTag
    // à l'exception de Rules qui se place manuellement
    private void Awake()
    {
        Board = GameObject.FindGameObjectWithTag("Board");
        BoardManager = new BoardManager();
        LaserManager = new LaserManager(this, BoardManager, LaserTemplate, LaserPredictionTemplate, LaserContainer);
        PlayersManager = new PlayersManager();
        TurnManager = new TurnManager(this, PlayersManager, skipTurnCooldown, LaserCooldown);
    }

    //On ne fait aucune action dans le Awake. Si besoins il suffit de recréer un start dans le scriptable voulu et de l'appeller dans le start du dataManager.
    //Void TurnManager
    private void Start()
    {
        TurnManager.Start();
    }
}
