using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButton : MenuButton
{
    public override void ChangeMenu()
    {
        base.OnClick();

        m_MenuSelection.ChangeMenu(Menus.Main);
    }
}
