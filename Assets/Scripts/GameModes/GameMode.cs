using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#nullable enable

public abstract class GameMode : ScriptableObject
{
    public virtual void Initialise()
    {
    }

    public void ProcessLeavingLasers(List<int> leavingLasersRight, List<int> leavingLasersLeft, List<int> leavingLasersTop, List<int> leavingLasersBot)
    {
        PlayersManager.GetInstance().HitPlayer(0, leavingLasersBot.Count);
        PlayersManager.GetInstance().HitPlayer(1, leavingLasersTop.Count);
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
            Debug.Log(PlayersManager.GetInstance().GetPlayer((int)winner).m_name + " wins !");
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
