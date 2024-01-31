using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
#nullable enable

public abstract class UIManager : MonoBehaviour
{
#if !DEDICATED_SERVER
    public static UIManager Instance { get; private set; }

    private bool isUIManagerReady = false;
    // Orders to execute in order the UI updates called before manager is ready
    private int nextUpdateOrder = 0;
    private int nextUpdateToExecute = 0;

    [SerializeField]
    private GameObject canvas;

    [SerializeField]
    private GameObject errorMessagePrefab;
    private float errorDisplayTime = 8f;
    [SerializeField]
    private GameObject wideMessagePrefab;

    void Awake()
    {
        Instance = this;
    }

    protected virtual void Start()
    {
        isUIManagerReady = true;

    }

    // Wait for UIManager to be ready, modifies UI only for clients
    protected async Task WaitForReadiness()
    {
        if (isUIManagerReady && (nextUpdateOrder == nextUpdateToExecute)) return;

        int thisUpdateOrder = nextUpdateOrder++;
        while (!isUIManagerReady) await Task.Delay(10);
        while (thisUpdateOrder != nextUpdateToExecute) await Task.Yield();
        nextUpdateToExecute++;
    }

    // Error message

    public async void DisplayErrorMessage(string message)
    {
        await WaitForReadiness();
        GameObject errorMessageGameObject = Instantiate(errorMessagePrefab, canvas.transform);
        errorMessageGameObject.GetComponent<TextMeshProUGUI>().text = message;
        StartCoroutine(DestroyCoroutine(errorMessageGameObject, errorDisplayTime));
    }

    public async void DisplayWideMessage(string message)
    {
        await WaitForReadiness();
        GameObject wideMessageGameObject = Instantiate(wideMessagePrefab, canvas.transform);
        wideMessageGameObject.GetComponentInChildren<TextMeshProUGUI>().text = message;
    }

    protected IEnumerator DestroyCoroutine(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(go);
    }
#endif
}
