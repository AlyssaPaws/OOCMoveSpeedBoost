using RimWorld;
using UnityEngine;
using Verse;

#nullable disable
namespace OOCMoveSpeedBoost;

public static class KeyBindingHandler
{
    public static void OnGUI()
    {
        if (Event.current.type != EventType.KeyDown)
            return;
        
        if (KeyBindings.MUR_ToggleBoost.KeyDownEvent)
            MainController.MainToggle();

        if (KeyBindings.MUR_ManualOverride.KeyDownEvent)
            MainController.ManualOverride(Find.CurrentMap);
        
        if (KeyBindings.MUR_ManualReactivation.KeyDownEvent)
            MainController.ManualReactivation(Find.CurrentMap);
    }
}