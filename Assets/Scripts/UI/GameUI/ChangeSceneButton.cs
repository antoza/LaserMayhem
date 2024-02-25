using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneButton : MonoBehaviour
{
    [SerializeField]
    private string scene;

    public void OnClick()
    {
        SceneManager.LoadScene(scene);
    }
}
