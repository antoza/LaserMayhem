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

    private void StartMainMenu()
    {
        if (MenuMessageManager.Instance.IsTcpReady) {
            MenusManager.Instance.ChangeMenu(Menus.Connection);
        }
        else
        {
            UIManager.Instance.DisplayWideMessage("The server is not available at the moment.\n" +
                "It usually becomes available between 3 p.m. and 12 p.m., so please try again during these times.\n" +
                "\n" +
                "You'll find more information on Discord.\n" +
                "(on the bottom left corner)");
        }
        Destroy(gameObject);
    }

    private void StartServerMenu()
    {
        Destroy(gameObject);
    }
}