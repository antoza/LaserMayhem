using UnityEngine;

public class PlayerData : ScriptableObject
{
    public PlayerHealth PlayerHealth { get; private set; }
    public PlayerEconomy PlayerEconomy { get; private set; }
    public PlayerActions PlayerActions { get; private set; }
    public int m_playerID;
    public string m_name;

    public PlayerData(int playerID)
    {
        m_playerID = playerID;
        PlayerHealth = new PlayerHealth(this);
        PlayerEconomy = new PlayerEconomy();
        PlayerActions = new PlayerActions(this);
    }
}
