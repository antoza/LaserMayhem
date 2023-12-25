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
        if(GameInitialParameters.localPlayerID == 1)
        {
            PlayersManager.Instance.HitPlayer(1, leavingLasersBot.Count);
            PlayersManager.Instance.HitPlayer(0, leavingLasersTop.Count);
            PlayersManager.Instance.HitPlayer(PlayersManager.Instance.currentPlayerID, leavingLasersLeft.Count + leavingLasersRight.Count);
        }
        else
        {
            PlayersManager.Instance.HitPlayer(0, leavingLasersBot.Count);
            PlayersManager.Instance.HitPlayer(1, leavingLasersTop.Count);
            PlayersManager.Instance.HitPlayer(PlayersManager.Instance.currentPlayerID, leavingLasersLeft.Count + leavingLasersRight.Count);
        }
        
    }

    public abstract bool CheckGameOver();

    public virtual bool VerifyAction(PlayerAction action)
    {
        // TODO : a supprimer : if (action is not PlayerAction) return false;
        if (!((PlayerAction)action).PlayerData.PlayerActions.m_CanPlay) return false;
        // TODO : On pourrait ajouter qu'on n'autorise pas le joueur à jouer si le laser n'a pas fini son animation
        return true;
    }
    public abstract void ExecuteAction(Action action);
    public abstract void RevertAction(Action action);

    // TODO : A supprimer
    /*public abstract bool MoveToDestinationTile(Tile? sourceTile, Tile destinationTile, PlayerData playerData);
    public abstract bool RevertMove(Tile sourceTile, Tile destinationTile, Piece piece, PlayerData playerData);*/

    // TODO : A METTRE DANS UNE NOUVELLE CLASSE QUI GERE LE DEBUT / LA FIN DE PARTIE
    public void TriggerGameOver(int? winner)
    {
        if (winner == null)
        {
            Debug.Log("Draw !");
        }
        else
        {
            if (winner! == GameInitialParameters.localPlayerID)
            {
                UIManager.Instance.TriggerVictory();
            }
            else
            {
                UIManager.Instance.TriggerDefeat();
            }
            Debug.Log(PlayersManager.Instance.GetPlayer((int)winner).m_name + " wins !");
        }


        //Pour demain : mettre un bouton "ok" après l'annonce de la victoire / défaite
        //corriger le fait que certains managers comme relaymanager ne redéclenchent leur start quand on revient au menu après une game

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
