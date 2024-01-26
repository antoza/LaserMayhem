using TMPro;
using UnityEngine;

public class PlayerEconomy : ScriptableObject
{
    private PlayerData PlayerData;

    private int _mana;
    public int Mana
    {
        get => _mana;
        private set
        {
            _mana = value;
#if !DEDICATED_SERVER
            ((UIManagerGame)UIManager.Instance).UpdateMana(PlayerData.m_playerID, value);
#endif
        }
    }

    private int _deletionCost;
    public int DeletionCost
    {
        get => _deletionCost;
        private set
        {
            _deletionCost = value;
#if !DEDICATED_SERVER
            ((UIManagerGame)UIManager.Instance).UpdateDeletionCost(PlayerData.m_playerID, value);
#endif
        }
    }

    private int _movementCost;
    public int MovementCost
    {
        get => _movementCost;
        private set
        {
            _movementCost = value;
#if !DEDICATED_SERVER
            ((UIManagerGame)UIManager.Instance).UpdateMovementCost(PlayerData.m_playerID, value);
#endif
        }
    }

    public PlayerEconomy(PlayerData playerData)
    {
        PlayerData = playerData;

        Mana = 0;
        DeletionCost = 1;
        MovementCost = 1;
    }

    public void AddNewTurnMana(int turnNumber)
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
        Mana += turnNumber == 1 ? 1 : 2;
        PlayerData opponent = PlayersManager.Instance.GetPlayer((PlayerData.m_playerID + 1) % 2);
        if (opponent.PlayerEconomy.Mana < 0)
        {
            Mana += -2 * opponent.PlayerEconomy.Mana;
            opponent.PlayerEconomy.Mana = 0;
        }
    }

    public bool HasEnoughMana(int cost)
    {
        //return Mana >= cost;
        return Mana >= cost - 3;
    }

    public bool HasEnoughManaForDeletion()
    {
        return HasEnoughMana(DeletionCost);
    }

    public bool HasEnoughManaForMovement()
    {
        return HasEnoughMana(MovementCost);
    }

    public bool PayForPlacement(int cost)
    {
        if (HasEnoughMana(cost))
        {
            Mana -= cost;
            return true;
        }
        return false;
    }

    public bool PayForDeletion()
    {
        if (HasEnoughManaForDeletion())
        {
            Mana -= DeletionCost;
            DeletionCost++;
            return true;
        }
        return false;
    }

    public bool PayForMovement()
    {
        if (HasEnoughManaForMovement())
        {
            Mana -= MovementCost;
            MovementCost++;
            return true;
        }
        return false;
    }

    public void RefundPlacement(int cost)
    {
        Mana += cost;
    }

    public void RefundDeletion()
    {
        DeletionCost--;
        Mana += DeletionCost;
    }

    public void RefundMovement()
    {
        MovementCost--;
        Mana += MovementCost;
    }
}
