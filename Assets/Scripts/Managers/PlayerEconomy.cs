using UnityEngine;

public class PlayerEconomy : ScriptableObject
{
    public int m_mana { get; private set; }
    public int m_deletionCost { get; private set; }
    public int m_movingCost { get; private set; }

    private void Awake()
    {
        m_mana = 0;
        m_deletionCost = 1;
        m_movingCost = 0;
    }

    public void AddNewTurnMana(int turnNumber)
    {
        int manaToGive = 1;
        int manaSum = 1;
        while (manaSum < turnNumber)
        {
            manaToGive++;
            manaSum += manaToGive;
        }
        m_mana += manaToGive;
    }

    public bool HasEnoughMana(int cost)
    {
        return m_mana >= cost;
    }

    public bool HasEnoughManaForDeletion()
    {
        return HasEnoughMana(m_deletionCost);
    }

    public bool HasEnoughManaForMovement()
    {
        return HasEnoughMana(m_movingCost);
    }

    public bool PayForPlacement(int cost)
    {
        if (HasEnoughMana(cost))
        {
            m_mana -= cost;
            return true;
        }
        return false;
    }

    public bool PayForDeletion()
    {
        if (HasEnoughManaForDeletion())
        {
            m_mana -= m_deletionCost;
            m_deletionCost++;
            return true;
        }
        return false;
    }

    public bool PayForMovement()
    {
        if (HasEnoughManaForMovement())
        {
            m_mana -= m_movingCost;
            return true;
        }
        return false;
    }
}
