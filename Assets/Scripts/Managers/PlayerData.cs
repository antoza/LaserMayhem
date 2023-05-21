using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public DataManager DataManager { get; private set; }
    public PlayerHealth PlayerHealth { get; private set; }
    public PlayerEconomy PlayerEconomy { get; private set; }
    public PlayerActions PlayerActions { get; private set; }

    void Start()
    {
        DataManager = FindObjectOfType<DataManager>();
        PlayerHealth = new PlayerHealth();
        PlayerEconomy = new PlayerEconomy();
        PlayerActions = new PlayerActions();
    }
}
