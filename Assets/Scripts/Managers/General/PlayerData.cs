using UnityEngine;

public class PlayerData : ScriptableObject
{
    // TODO : rendre nullable (PlayerHealth?)
    public PlayerProfile PlayerProfile; 
    public PlayerHealth PlayerHealth;
    public PlayerEconomy PlayerEconomy;

    public int PlayerID;

    public PlayerData(int playerID)
    {
        PlayerID = playerID;
    }
}