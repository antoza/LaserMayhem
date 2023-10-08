using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MenuButton
{
    [field: SerializeField]
    private int SceneIndex = 0;
    public override void ChangeMenu()
    {
        SceneManager.LoadScene(SceneIndex > 0 ? SceneIndex : SceneManager.GetActiveScene().buildIndex + 1);
        MenuMessageManager.GetGameInitialParameters();
    }
}
