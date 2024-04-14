#if !DEDICATED_SERVER
using UnityEngine;

public class ChangeMenuButton : MenuButton
{
    [SerializeField]
    private Menus MenuToGo;

    public override void DoOnClick()
    {
        OnButtonPressed(MenuToGo);
    }
}
#endif
