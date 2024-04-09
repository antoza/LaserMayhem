using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenManager : MonoBehaviour
{
    public static SplashScreenManager Instance { get; private set; }
    [SerializeField]
    private float _splashScreenDelay = 3f;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        StartCoroutine(WaitSplashScreenDelay());
    }

    private IEnumerator WaitSplashScreenDelay()
    {
        // TODO : Mettre ailleurs
        if (!PlayerPrefs.HasKey("unlockedChallenges"))
        {
            PlayerPrefs.SetInt("unlockedChallenges", 1);
        }

#if !DEDICATED_SERVER
#if DEBUG
        _splashScreenDelay = .2f;
#endif
        yield return new WaitForSeconds(_splashScreenDelay);
        SceneManager.LoadScene("MainMenu");
        yield return new WaitForSeconds(.1f);
        StartMainMenu();
#else
        SceneManager.LoadScene("ServerMenu");
        yield return new WaitForSeconds(.1f);
        StartServerMenu();
#endif
    }

#if !DEDICATED_SERVER
    private void StartMainMenu()
    {
        if (MenuMessageManager.Instance.IsTcpReady) {
            MenusManager.Instance.ChangeMenu(Menus.Connection);
        }
        else
        {
#if !WEBGL_CLIENT
            UIManager.Instance.DisplayWideMessage("The server is not available at the moment.\n" +
                "You can't play online but you have access to challenges.\n" +
                "\n" +
                "You'll find more information on Discord.\n" +
                "(on the bottom left corner)");
#else
            UIManager.Instance.DisplayWideMessage("You are playing on the browser.\n" +
                "You have access to challenges (solo),\n" +
                "but you need to download the game to play online.");
#endif
            // TODO : A supprimer plus tard
            GameObject.Find("Menus").transform.Find("ChallengeSelectionMenu").Find("BackButton").gameObject.SetActive(false);
            GameObject.Find("Menus").transform.Find("ChallengeSelectionMenu").Find("BackButtonOffline").gameObject.SetActive(true);
        }
        Destroy(gameObject);
    }
#else
    private void StartServerMenu()
    {
        Destroy(gameObject);
    }
#endif
        }