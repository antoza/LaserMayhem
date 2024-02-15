using System;
using Unity.VisualScripting;
using UnityEngine;

public sealed class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }
    public Rules Rules { get; private set; }
    //public GameMode GameMode { get; private set; }

    //Laser Templates

    private void Awake()
    {
        Instance = this;

        // TODO : mettre tous ces managers en MonoBehavior, et dans la scène. Ils ne doivent plus être générés par le DM
        //BoardManager.SetInstance(new GameObject("Board"));
        //LaserManager.SetInstance(LaserTemplate, LaserPredictionTemplate, LaserContainer);
        //PlayersManager.SetInstance();
        //TurnManager.SetInstance();
        //RewindManager.SetInstance();
        //SendActionsManager.SetInstance();
        Rules = GameInitialParameters.Rules;
        //CreateGameMode();
    }
    /*
    private void CreateGameMode()
    {
        Type type = Type.GetType("GameMode" + Rules.GameModeName);
        if (type != null && type.IsSubclassOf(typeof(GameMode)))
        {
            GameMode = (GameMode)Activator.CreateInstance(type);
            GameMode.Initialise();
        }
        else
        {
            Debug.Log("La classe spécifiée n'est pas valide.");
        }
    }*/

    private void Start()
    {
    }
}
