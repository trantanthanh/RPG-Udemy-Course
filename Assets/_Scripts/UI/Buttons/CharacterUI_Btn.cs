using Menu;

public class CharacterUI_Btn : BaseButton
{
    protected override void OnClick()
    {
        ui.SwitchMenuTo(ui.CharacterUI);
    }
}
