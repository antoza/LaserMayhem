using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameButton : MenuButton
{
    public override void ChangeMenu()
    {
        base.OnClick();

        m_MenuSelection.ChangeMenu(Menus.GameMode);
    }
}
