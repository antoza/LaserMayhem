#if !DEDICATED_SERVER
using UnityEngine;

public class ChangeMenuButton : MenuButton
{
    [SerializeField]
    private Menus TargetMenu;

    public override void DoOnClick()
    {
        MenusManager.Instance.ChangeMenu(TargetMenu);
    }
}
#endif
