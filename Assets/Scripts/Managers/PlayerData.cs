using UnityEngine;

public class PlayerData : ScriptableObject
{
    public PlayerHealth PlayerHealth { get; private set; }
    public PlayerEconomy PlayerEconomy { get; private set; }
    public PlayerActions PlayerActions { get; private set; }

    public int m_playerID;

    private string _username;
    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            UIManager.Instance.UpdateUsername(m_playerID, value);
        }
    }


    public PlayerData(int playerID)
    {
        m_playerID = playerID;
        PlayerHealth = new PlayerHealth(this);
        PlayerEconomy = new PlayerEconomy(this);
        PlayerActions = new PlayerActions(this);
    }
}
