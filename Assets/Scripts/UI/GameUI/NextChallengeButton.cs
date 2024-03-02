using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextChallengeButton : MonoBehaviour
{
    public void OnClick()
    {
        SceneManager.LoadScene($"Challenge{GameModeManagerSolo.Instance.ChallengeNumber + 1}");
    }
}
