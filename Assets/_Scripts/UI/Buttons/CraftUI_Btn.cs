using Menu;

public class CraftUI_Btn : BaseButton
{
    private UI ui;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        ui = GetComponentInParent<UI>();
    }

    protected override void OnClick()
    {
        ui.SwitchMenuTo(ui.CraftUI);
    }
}
