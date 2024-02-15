using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#nullable enable

public abstract class GameModeManager : Manager<GameModeManager>
{
    public abstract bool CheckGameOver();

    public virtual bool VerifyAction(PlayerAction action)
    {
        if (!PlayersManager.Instance.GetCurrentPlayer() == action.PlayerData) return false;
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
#if DEDICATED_SERVER
        SenderManager.Instance.SaveResults();
        MenuMessageManager.Instance.StopServer();
        SceneManager.LoadScene("ServerMenu");
        return;
#else
        if (winner == null)
        {
            ((UIManagerGame)UIManager.Instance).TriggerDraw();
        }
        else
        {
            if (winner! == GameInitialParameters.localPlayerID)
            {
                ((UIManagerGame)UIManager.Instance).TriggerVictory();
            }
            else
            {
                ((UIManagerGame)UIManager.Instance).TriggerDefeat();
            }
        }
#endif
    }
}
