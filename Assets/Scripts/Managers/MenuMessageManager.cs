using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MenuMessageManager : MonoBehaviour
{
    [field: SerializeField]
    private Rules Rules;

    public MenuMessageManager()
    {
    }

    public class Notification : MonoBehaviour//MessageBase
    {
        public string oui;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetGameInitialParameters()
    {
        string[] names = { "moi", "adversaire" };
        SetGameInitialParameters(Rules, names);
    }

    public void SetGameInitialParameters(Rules rules, string[] names)
    {
        GameInitialParameters.Rules = rules;
        GameInitialParameters.PlayerNames = names;
    }
}
