using Menu;

public class CraftUI_Btn : BaseButton
{
    protected override void LoadComponents()
    {
        base.LoadComponents();
    }

    protected override void OnClick()
    {
        ui.SwitchMenuTo(ui.CraftUI);
    }
}
