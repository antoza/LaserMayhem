#if !DEDICATED_SERVER
using UnityEngine;

public class ChangeMenuButton : MenuButton
{
    [SerializeField]
    private Menus MenuToGo;

    public override void DoOnClick()
    {
        MenusManager.Instance.ChangeMenu(MenuToGo);
    }
}
#endif
