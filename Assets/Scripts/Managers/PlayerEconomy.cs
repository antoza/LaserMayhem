using UnityEngine;

public class PlayerEconomy : ScriptableObject
{
    public int m_mana { get; private set; }
    public int m_deletionCost { get; private set; }
    public int m_movingCost { get; private set; }

    private void Awake()
    {
        m_mana = 0;
        m_deletionCost = -9999;
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

    private bool IsActionPossible(int cost)
    {
        return m_mana >= -cost;
    }

    public bool PayForPlacement(int cost)
    {
        if (IsActionPossible(cost))
        {
            m_mana += cost;
            return true;
        }
        return false;
    }

    public bool PayForDeletion()
    {
        if (IsActionPossible(m_deletionCost))
        {
            m_mana += m_deletionCost;
            m_deletionCost++;
            return true;
        }
        return false;
    }

    public bool PayForMovement()
    {
        if (IsActionPossible(m_movingCost))
        {
            m_mana += m_movingCost;
            return true;
        }
        return false;
    }
}
