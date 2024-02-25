#if !DEDICATED_SERVER
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartChallengeButton : StartGameOfflineButton
{
    [field: SerializeField]
    private int challengeNumber;
    [field: SerializeField]
    private GameObject lockedButton;
    private bool unlocked;

    private void Start()
    {
        SceneName += challengeNumber.ToString();
        unlocked = challengeNumber <= PlayerPrefs.GetFloat("unlockedChallenges");
        if (!unlocked)
        {
            lockedButton.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
#endif