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

    public async void DisplayError(string error)
    {
        await WaitForReadiness();
        GameObject errorMessageGameObject = Instantiate(errorMessagePrefab, canvas.transform);
        errorMessageGameObject.GetComponent<TextMeshProUGUI>().text = error;
        StartCoroutine(DestroyCoroutine(errorMessageGameObject, errorDisplayTime));
    }

    protected IEnumerator DestroyCoroutine(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(go);
    }
#endif
}
