using UnityEngine;

public class PlayerData : ScriptableObject
{
    public DataManager DM { get; private set; }
    public PlayerHealth PlayerHealth { get; private set; }
    public PlayerEconomy PlayerEconomy { get; private set; }
    public PlayerActions PlayerActions { get; private set; }

    public PlayerData(DataManager dataManager)
    {
        DM = dataManager;
        PlayerHealth = new PlayerHealth();
        PlayerEconomy = new PlayerEconomy();
        PlayerActions = new PlayerActions(this);
    }
}
