using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UndoButton : MonoBehaviour
{
#if !DEDICATED_SERVER
    private Button undoButton;
    private Image image;

    [SerializeField]
    private Sprite _unpressedButton;
    [SerializeField]
    private Sprite _pressedButton;

    private void Start()
    {
        undoButton = GetComponent<Button>();
        image = GetComponent<Image>();
    }

    public void OnClick()
    {
        LocalPlayerManager.Instance.CreateAndVerifyUndoAction();
    }

    public void SetAsPressed()
    {
        image.sprite = _pressedButton;
        undoButton.interactable = false;
    }

    public void SetAsQuicklyPressed()
    {
        StartCoroutine(QuicklyPressButton());
    }

    public void SetAsUnpressed()
    {
        image.sprite = _unpressedButton;
        undoButton.interactable = true;
    }

    private IEnumerator QuicklyPressButton() // TODO : J'en ai marre de l'outil d'Animation, il fait n'importe quoi. A changer plus tard pour une vraie animation
    {
        image.sprite = _pressedButton;
        yield return new WaitForSeconds(0.1f);
        image.sprite = _unpressedButton;
    }
#endif
}
