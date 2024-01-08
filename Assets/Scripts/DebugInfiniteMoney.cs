using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugInfiniteMoney : MonoBehaviour
{

#if !DEBUG
    private void Start()
    {
        this.gameObject.SetActive(false);
    }
#endif

#if DEBUG
    public void OnClick()
    {
        //PlayersManager.Instance.AddInfiniteMana();
    }
#endif
}