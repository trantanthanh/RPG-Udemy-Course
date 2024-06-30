using Menu;

public class OptionsUI_Btn : BaseButton
{
    protected override void OnClick()
    {
        ui.SwitchMenuTo(ui.OptionsUI);
    }
}
