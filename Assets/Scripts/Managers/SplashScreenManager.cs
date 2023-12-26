using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreenManager : MonoBehaviour
{
    public static SplashScreenManager Instance { get; private set; }
    [SerializeField]
    private float m_AnimationTime = 3f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
#if DEBUG
        m_AnimationTime = 1f;
#endif
#if !DEDICATED_SERVER
        StartCoroutine(DelayChangeScene(m_AnimationTime));
#else
        SceneManager.LoadScene("ServerMenu");
#endif
    }

    private IEnumerator DelayChangeScene(float animationTime)
    {
        yield return new WaitForSeconds(animationTime);
        SceneManager.LoadScene("MainMenu");
    }
}