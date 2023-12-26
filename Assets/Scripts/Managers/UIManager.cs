using UnityEngine;
using System;
#nullable enable

public sealed class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField]
    private GameObject VictoryPopUp;
    [SerializeField]
    private GameObject DefeatPopUp;
    [SerializeField]
    private GameObject DrawPopUp;

    void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
    }

    public void TriggerDraw()
    {
        DrawPopUp.SetActive(true);
    }

    public void TriggerVictory()
    {
        VictoryPopUp.SetActive(true);
    }

    public void TriggerDefeat()
    {
        DefeatPopUp.SetActive(true);
    }
}
