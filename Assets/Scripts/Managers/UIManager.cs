using UnityEngine;
using System;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
#nullable enable

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private bool isUIManagerReady = false;

    [SerializeField]
    private GameObject canvas;

    private int[] playerIndexes;

    [SerializeField]
    private GameObject errorMessagePrefab;
    private float errorDisplayTime = 8f;

    // TODO : Rendre UIManager abstraite, avec des sous-classes comme GameUIManager
    [SerializeField]
    private GameObject victoryPopUp;
    [SerializeField]
    private GameObject defeatPopUp;
    [SerializeField]
    private GameObject drawPopUp;

    [SerializeField]
    private TextMeshProUGUI[] usernameTexts;
    [SerializeField]
    private TextMeshProUGUI[] healthTexts;
    [SerializeField]
    private TextMeshProUGUI[] manaTexts;
    [SerializeField]
    private TextMeshProUGUI[] movementCostTexts;
    [SerializeField]
    private TextMeshProUGUI[] deletionCostTexts;

    void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        AssociatePlayersInAscendingOrder(GameInitialParameters.localPlayerID, GameInitialParameters.Rules.NumberOfPlayers);
        isUIManagerReady = true;
    }

    private void AssociatePlayersInAscendingOrder(int localPlayerID, int numberOfPlayers)
    {
        playerIndexes = new int[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            playerIndexes[i] = (i + localPlayerID) % numberOfPlayers;
        }
    }

    // Wait for UIManager to be ready, modifies UI only for clients
    private async Task<bool> IsUIUpdatable()
    {
#if DEDICATED_SERVER
        return false;
#else
        while (!isUIManagerReady) await Task.Delay(100);
        return true;
#endif
    }

    // Error Message

    public async void DisplayError(string error)
    {
        if (await IsUIUpdatable())
        {
            GameObject errorMessageGameObject = Instantiate(errorMessagePrefab, canvas.transform);
            errorMessageGameObject.GetComponent<TextMeshProUGUI>().text = error;
            StartCoroutine(DestroyCoroutine(errorMessageGameObject, errorDisplayTime));
        }
    }

    private IEnumerator DestroyCoroutine(GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(go);
    }


    // Gameover Popups

    public async void TriggerDraw()
    {
        if (await IsUIUpdatable())
        {
            drawPopUp.SetActive(true);
        }
    }

    public async void TriggerVictory()
    {
        if (await IsUIUpdatable())
        {
            victoryPopUp.SetActive(true);
        }
    }

    public async void TriggerDefeat()
    {
        if (await IsUIUpdatable())
        {
            defeatPopUp.SetActive(true);
        }
    }


    // Players UI

    public async void UpdateUsername(int playerID, string value)
    {
        if (await IsUIUpdatable())
        {
            TextMeshProUGUI tmp = usernameTexts[playerIndexes[playerID]];
            if (tmp != null) tmp.text = value;
        }
    }

    public async void UpdateHealth(int playerID, int value)
    {
        if (await IsUIUpdatable())
        {
            TextMeshProUGUI tmp = healthTexts[playerIndexes[playerID]];
            if (tmp != null) tmp.text = value.ToString();
        }
    }

    public async void UpdateMana(int playerID, int value)
    {
        if (await IsUIUpdatable())
        {
            TextMeshProUGUI tmp = manaTexts[playerIndexes[playerID]];
            if (tmp != null) tmp.text = value.ToString();
        }
    }

    public async void UpdateMovementCost(int playerID, int value)
    {
        if (await IsUIUpdatable())
        {
            TextMeshProUGUI tmp = movementCostTexts[playerIndexes[playerID]];
            if (tmp != null) tmp.text = "Movement: " + value.ToString();
        }
    }

    public async void UpdateDeletionCost(int playerID, int value)
    {
        if (await IsUIUpdatable())
        {
            TextMeshProUGUI tmp = deletionCostTexts[playerIndexes[playerID]];
            if (tmp != null) tmp.text = "Deletion: " + value.ToString();
        }
    }
}
