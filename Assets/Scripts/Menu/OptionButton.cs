using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionButton : MenuButton
{
    public override void ChangeMenu()
    {
        base.OnClick();

        m_MenuSelection.ChangeMenu(Menus.Options);
    }
}
