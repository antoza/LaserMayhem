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
            UIManagerGame.Instance.UpdateMana(PlayerData.PlayerID, value);
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
            UIManagerGame.Instance.UpdateDeletionCost(PlayerData.PlayerID, value);
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
            UIManagerGame.Instance.UpdateMovementCost(PlayerData.PlayerID, value);
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

    public void AddMana(int amount)
    {
        Mana += amount;
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
