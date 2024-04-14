using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationChoiceButton : MenuButton
{
    [SerializeField]
    private TranslationChoice TranslationChoice;
    [SerializeField]
    private int TranslationID;
    public override void DoOnClick()
    {
        OnButtonPressed(Menus.None);
        TranslationChoice.ChangeLocale(TranslationID);
    }
}
