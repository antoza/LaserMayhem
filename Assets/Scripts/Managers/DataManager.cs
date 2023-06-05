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


    // Start : on crée chaque manager (BoardManager = new BoardManager(this)) etc plutôt que de faire des FindObjectWithTag
    // à l'exception de Rules qui se place manuellement
    private void Awake()
    {
        Board = GameObject.FindGameObjectWithTag("Board");
        BoardManager = new BoardManager();
        LaserManager = new LaserManager();
        PlayersManager = new PlayersManager();
        TurnManager = FindObjectOfType<TurnManager>();
    }
}
