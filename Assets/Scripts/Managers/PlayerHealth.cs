using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : ScriptableObject
{
    private DataManager m_DataManager;
    public int m_health { get; private set; }

    void Start()
    {
        m_DataManager = FindObjectOfType<DataManager>();
        m_health = m_DataManager.Rules.InitialHealth;
    }

    public bool IsDead()
    {
        return m_health <= 0;
    }

    public void TakeDamage(int damage)
    {
        m_health -= damage;
        if (m_health < 0) { m_health = 0; }
    }
}
