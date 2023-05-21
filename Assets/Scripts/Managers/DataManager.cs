using UnityEngine;

public class DataManager : MonoBehaviour
{
    [field: SerializeField]
    public Rules Rules { get; private set; }
    [field: SerializeField]
    public BoardManager BoardManager { get; private set; }
    [field: SerializeField]
    public LaserManager LaserManager { get; private set; }
    [field: SerializeField]
    public PlayersManager PlayersManager { get; private set; }
    [field: SerializeField]
    public TurnManager TurnManager { get; private set; }

    // Start : on cr�e chaque manager (BoardManager = new BoardManager(this)) etc plut�t que de faire des FindObjectWithTag
    // � l'exception de Rules qui se place manuellement
    private void Awake()
    {
        BoardManager = new BoardManager();
        LaserManager = new LaserManager();
        PlayersManager = new PlayersManager();
        TurnManager = new TurnManager();
    }
}
