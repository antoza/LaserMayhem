using UnityEngine;
using System;
using TMPro;
using System.Collections;
#nullable enable

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private GameObject canvas;

    [SerializeField]
    private GameObject errorMessagePrefab;
    private float errorDisplayTime = 3f;

    // TODO : Rendre UIManager abstraite, avec des sous-classes comme GameUIManager
    [SerializeField]
    private GameObject victoryPopUp;
    [SerializeField]
    private GameObject defeatPopUp;
    [SerializeField]
    private GameObject drawPopUp;

    void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
    }

    public void DisplayError(string error)
    {
        GameObject errorMessageGameObject = Instantiate(errorMessagePrefab, canvas.transform);
        errorMessageGameObject.GetComponent<TextMeshProUGUI>().text = error;
        StartCoroutine(DestroyCoroutine(errorMessageGameObject, errorDisplayTime));
    }

    private IEnumerator DestroyCoroutine(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(go);
    }

    public void TriggerDraw()
    {
        drawPopUp.SetActive(true);
    }

    public void TriggerVictory()
    {
        victoryPopUp.SetActive(true);
    }

    public void TriggerDefeat()
    {
        defeatPopUp.SetActive(true);
    }
}
