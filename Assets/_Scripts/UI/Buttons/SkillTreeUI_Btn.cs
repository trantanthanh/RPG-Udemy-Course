using Menu;

public class SkillTreeUI_Btn : BaseButton
{
    protected override void LoadComponents()
    {
        base.LoadComponents();
    }

    protected override void OnClick()
    {
        ui.SwitchMenuTo(ui.SkillTreeUI);
    }
}
