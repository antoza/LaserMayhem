using System;
using UnityEngine;
using UnityEngine.Assertions;
#nullable enable

public class PlayersManagerRPG : PlayersManager
{
    public static new PlayersManagerRPG Instance => (PlayersManagerRPG)PlayersManager.Instance;

    [SerializeField]
    private int initialHealth;

    protected override void InitializePlayer(PlayerData player)
    {
#if !DEDICATED_SERVER
        player.PlayerProfile = new PlayerProfile(player, GameInitialParameters.usernames[player.PlayerID]);
#endif
        player.PlayerHealth = new PlayerHealth(player, initialHealth);
        player.PlayerEconomy = new PlayerEconomy(player);
    }

    public override void StartNextPlayerTurn(int turnNumber)
    {
        base.StartNextPlayerTurn(turnNumber);
        GiveNewTurnMana(turnNumber);
    }

    public void GiveNewTurnMana(int turnNumber)
    {
        /*int manaToGive = 1;
        int manaSum = 1;
        while (manaSum < turnNumber)
        {
            manaToGive++;
            manaSum += manaToGive;
        }
        m_mana += manaToGive;*/

        // TODO : Faire proprement avec le turn manager
        playerList[currentPlayerID].PlayerEconomy.AddMana(turnNumber == 1 ? 1 : 2);
        PlayerData opponent = PlayersManager.Instance.GetPlayer((currentPlayerID + 1) % 2);
        int opponentMana = opponent.PlayerEconomy.Mana;
        if (opponentMana < 0)
        {
            playerList[currentPlayerID].PlayerEconomy.AddMana(-2 * opponentMana);
            opponent.PlayerEconomy.AddMana(-opponentMana);
        }
    }
}
