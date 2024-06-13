using Menu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseUI_Btn : BaseButton
{
    protected override void OnClick()
    {
        ui.SwitchMenuTo(null);
    }
}
