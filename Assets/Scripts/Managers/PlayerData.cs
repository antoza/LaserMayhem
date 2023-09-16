using UnityEngine;

public class PlayerData : ScriptableObject
{
    public DataManager DM { get; private set; }
    public PlayerHealth PlayerHealth { get; private set; }
    public PlayerEconomy PlayerEconomy { get; private set; }
    public PlayerActions PlayerActions { get; private set; }
    public int m_playerID;
    public string m_name;

    public PlayerData(DataManager dataManager, int playerID)
    {
        DM = dataManager;
        m_playerID = playerID;
        PlayerHealth = new PlayerHealth(DM, this);
        PlayerEconomy = new PlayerEconomy();
        PlayerActions = new PlayerActions(this);
    }
}
