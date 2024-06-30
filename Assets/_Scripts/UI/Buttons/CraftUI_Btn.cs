using Menu;

public class CraftUI_Btn : BaseButton
{
    protected override void OnClick()
    {
        ui.SwitchMenuTo(ui.CraftUI);
    }
}
