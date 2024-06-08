using UnityEngine.UI;

public class CharacterBtn : BaseButton
{
    private UI ui;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        ui = GetComponentInParent<UI>();
    }

    protected override void OnClick()
    {
        ui.SwitchMenuTo(ui.CharacterUI);
    }
}
