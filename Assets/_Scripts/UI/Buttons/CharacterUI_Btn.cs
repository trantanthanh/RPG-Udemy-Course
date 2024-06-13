using Menu;

public class CharacterUI_Btn : BaseButton
{
    protected override void LoadComponents()
    {
        base.LoadComponents();
    }

    protected override void OnClick()
    {
        ui.SwitchMenuTo(ui.CharacterUI);
    }
}
