using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#nullable enable

public abstract class GameMode : ScriptableObject
{
    public DataManager DM { get; private set; }

    public void SetDataManager(DataManager dataManager)
    {
        DM = dataManager;
    }
    
    public virtual void Initialise()
    {
    }

    public void ProcessLeavingLasers(List<int> leavingLasersRight, List<int> leavingLasersLeft, List<int> leavingLasersTop, List<int> leavingLasersBot)
    {
        DM.PlayersManager.HitPlayer(0, leavingLasersBot.Count);
        DM.PlayersManager.HitPlayer(1, leavingLasersTop.Count);
    }

    public abstract bool CheckGameOver();

    public abstract bool MoveToDestinationTile(Tile? sourceTile, Tile destinationTile, PlayerData playerData);
    public abstract bool RevertMove(Tile sourceTile, Tile destinationTile, Piece piece, PlayerData playerData);

    // A METTRE DANS UNE NOUVELLE CLASSE QUI GERE LE DEBUT / LA FIN DE PARTIE
    public void TriggerGameOver(int? winner)
    {
        if (winner == null)
        {
            Debug.Log("Draw !");
        }
        else
        {
            Debug.Log(DM.PlayersManager.GetPlayer((int)winner).m_name + " wins !");
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
