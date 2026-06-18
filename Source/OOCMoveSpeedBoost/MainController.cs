#nullable disable
using System.Globalization;
using RimWorld;
using Verse;
using Verse.Sound;

namespace OOCMoveSpeedBoost;

public static class MainController
{
    public static int checkCombatTicks = 1200;
    public static bool refreshTicks;

    public static void ForceSlowForMap(Map map)
    {
        if (map == null) { Log.Error("[OOC Move Speed Boost] Tried toggling speed multiplier for null map."); return; }

        if (CombatChecker.IsManualOverride(map)) return;
        if (CombatChecker.IsCombatActive(map)) return;
        
        CombatChecker.SetCombatState(map, true);
        
        if (Settings.onOffNotification && Find.CurrentMap == map) Messages.Message(Resources.BoostDisabledStr, MessageTypeDefOf.SilentInput, false);
    }

    public static void ResumeForMap(Map map, bool manualReactivationMode = false)
    {
        if (map == null) { Log.Error("[OOC Move Speed Boost] Tried toggling speed multiplier for null map."); return; }

        if (!CombatChecker.IsCombatActive(map)) return;
        if (Settings.manualReactivation && !manualReactivationMode) return;
        
        CombatChecker.SetCombatState(map, false);
        
        if (Settings.onOffNotification && Find.CurrentMap == map) Messages.Message(Resources.BoostReEnabledStr, MessageTypeDefOf.SilentInput, false);
    }
    
    public static void ManualOverride(Map map)
    {
        bool currentOverride = CombatChecker.IsManualOverride(map);
        
        CombatChecker.SetManualOverride(map, !currentOverride);
        
        Messages.Message("OOCMSB.Message.ManualOverrideToggle".Translate(!currentOverride ? Resources.OnKeyStr : Resources.OffKeyStr), MessageTypeDefOf.SilentInput, false);
    }

    public static void ManualReactivation(Map map)
    {
        if (!Settings.manualReactivation || !CombatChecker.IsCombatActive(map)) return;
        if (CombatChecker.AnyPawnsDrafted(map)) return;
        
        ResumeForMap(map, true);
    }

    public static void MainToggle()
    {
        Settings.boostToggle = !Settings.boostToggle;

        SoundDef sound = Settings.boostToggle ? SoundDefOf.Checkbox_TurnedOn : SoundDefOf.Checkbox_TurnedOff;
        sound.PlayOneShotOnCamera();
    }
}