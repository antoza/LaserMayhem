#if !DEDICATED_SERVER
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DebugFastLog : MenuButton
{
    [SerializeField]
    public string Letter;
    //[SerializeField]
    //public string SceneName;

    public override void DoOnClick()
    {
        if (Letter == null) return;
        string username = Letter;
        string password = Letter + Letter + Letter + Letter + Letter + Letter + Letter + Letter;
        SenderManager.Instance.LogIn(username, password);
        //StartCoroutine(SearchGameCoroutine());
    }
    /*
    public IEnumerator SearchGameCoroutine()
    {
        yield return new WaitForSeconds(3);
        if (SceneName != null)
        {
            SenderManager.Instance.SearchGame(SceneName);
        }
    }*/
}
#endif