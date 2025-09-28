using UnityEngine;

#nullable disable
namespace OOCMoveSpeedBoost;

public static class KeyBindingHandler
{
    public static void OnGUI()
    {
        if (Event.current.type != EventType.KeyDown || !KeyBindings.MUR_ToggleBoost.KeyDownEvent)
            return;
        Settings.boostToggle = !Settings.boostToggle;
    }
}