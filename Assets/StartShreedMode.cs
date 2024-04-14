using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartShreedMode : MenuButton
{
    [SerializeField]
    private int ShreedSceneId;
    public override void DoOnClick()
    {
        SceneManager.LoadScene(ShreedSceneId);
    }
}
