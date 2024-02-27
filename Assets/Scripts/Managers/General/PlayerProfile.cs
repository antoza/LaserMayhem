using UnityEngine;

public class PlayerProfile : ScriptableObject
{
    private PlayerData PlayerData;

    private string _username;
    public string Username
    {
        get => _username;
        set
        {
            _username = value;
#if !DEDICATED_SERVER
            UIManagerGame.Instance.UpdateUsername(PlayerData.PlayerID, value);
#endif
        }
    }

    public PlayerProfile(PlayerData playerData, string username)
    {
        PlayerData = playerData;
        Username = username;
    }
}