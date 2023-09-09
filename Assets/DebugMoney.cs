using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugMoney : MonoBehaviour
{
    [field: SerializeField]
    private DataManager DM;

#if !DEBUG
    private void Start()
    {
        this.gameObject.SetActive(false);
    }
#endif

#if DEBUG
    public void OnClick()
    {
        DM.PlayersManager.AddInfiniteMana();
    }
#endif
}
    