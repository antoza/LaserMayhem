using UnityEngine;
using System;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
#nullable enable

public class UIManagerGame : UIManager
{
#if !DEDICATED_SERVER
    private int[] playerIndexes;

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

    [SerializeField]
    private PlayerTurnAnnouncementUI playerTurnAnnouncement;
    [SerializeField]
    private EndTurnButton endTurnButton;

    [SerializeField]
    private Image BackgroundImage;
    [SerializeField]
    public SkinData SkinData;

    protected override void Start()
    {
        base.Start();
        AssociatePlayersInAscendingOrder(GameInitialParameters.localPlayerID, GameInitialParameters.Rules.NumberOfPlayers);
        SetBackground();

    }

    private void AssociatePlayersInAscendingOrder(int localPlayerID, int numberOfPlayers)
    {
        playerIndexes = new int[numberOfPlayers];
        for (int i = 0; i < numberOfPlayers; i++)
        {
            playerIndexes[i] = (i + localPlayerID) % numberOfPlayers;
        }
    }


    // Gameover popups

    public async void TriggerDraw()
    {
        await WaitForReadiness();
        drawPopUp.SetActive(true);
    }

    public async void TriggerVictory()
    {
        await WaitForReadiness();
        victoryPopUp.SetActive(true);
    }

    public async void TriggerDefeat()
    {
        await WaitForReadiness();
        defeatPopUp.SetActive(true);
    }


    // Players information

    public async void UpdateUsername(int playerID, string value)
    {
        await WaitForReadiness();
        TextMeshProUGUI tmp = usernameTexts[playerIndexes[playerID]];
        if (tmp != null)
        {
            tmp.text = value;
            tmp.color = playerID == 0 ? new Color(.2f, .4f, 1f, 1f) : new Color(1f, .2f, .2f, 1f);
        }
    }

    public async void UpdateHealth(int playerID, int value)
    {
        await WaitForReadiness();
        TextMeshProUGUI tmp = healthTexts[playerIndexes[playerID]];
        if (tmp != null) tmp.text = value.ToString();
    }

    public async void UpdateMana(int playerID, int value)
    {
        await WaitForReadiness();
        TextMeshProUGUI tmp = manaTexts[playerIndexes[playerID]];
        if (tmp != null) tmp.text = value.ToString();
    }

    public async void UpdateMovementCost(int playerID, int value)
    {
        await WaitForReadiness();
        TextMeshProUGUI tmp = movementCostTexts[playerIndexes[playerID]];
        if (tmp != null) tmp.text = "Movement: " + value.ToString();
    }

    public async void UpdateDeletionCost(int playerID, int value)
    {
        await WaitForReadiness();
        TextMeshProUGUI tmp = deletionCostTexts[playerIndexes[playerID]];
        if (tmp != null) tmp.text = "Deletion: " + value.ToString();
    }

    // Turn

    public async void UpdateEndTurnButtonState(string state)
    {
        await WaitForReadiness();
        switch (state)
        {
            case "Pressed":
                if (LocalPlayerManager.Instance.IsLocalPlayersTurn()) endTurnButton.SetAsPressed();
                break;
            case "Unpressed":
                if (LocalPlayerManager.Instance.IsLocalPlayersTurn()) endTurnButton.SetAsUnpressed();
                break;
            default:
                break;
        }
    }

    public async void TriggerPlayerTurnAnnouncement()
    {
        await WaitForReadiness();
        playerTurnAnnouncement.StartAnimation(PlayersManager.Instance.GetCurrentPlayer().Username);
    }

    public void SetBackground()
    {/* TODO : décommenter (j'ai commenté car ça me créait des erreurs)
        if(PlayerPrefs.HasKey("Background Skin") && SkinData)
        {
            string backgroundSpriteName = PlayerPrefs.GetString("Background Skin");
            if(SkinData.BackgroundSkin.ContainsKey(backgroundSpriteName))
            {
                BackgroundImage.sprite = SkinData.BackgroundSkin[backgroundSpriteName];
            }
        }
        else
        {
            string backgroundSpriteName = "Default";
            if (SkinData.BackgroundSkin.ContainsKey(backgroundSpriteName))
            {
                BackgroundImage.sprite = SkinData.BackgroundSkin[backgroundSpriteName];
            }
        }*/
    }
#endif
}
