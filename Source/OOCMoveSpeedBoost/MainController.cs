#nullable disable
using System.Globalization;
using RimWorld;
using Verse;

namespace OOCMoveSpeedBoost;

public static class MainController
{
    public static int checkCombatTicks = 300;
    public static bool refreshTicks;
    public static bool inCombat;
    public static float mult = Settings.speedBoostMult;
    public static float combatMult = Settings.combatSpeedMult;
    
    public static string ManualToggleTooltip = "OOCMSB.BoostToggle.TT".Translate(Settings.speedBoostMult.ToString(CultureInfo.CurrentCulture), Settings.combatSpeedMult.ToString(CultureInfo.CurrentCulture));

    public static void ForceSlow()
    {
        if (Settings.manualOverride) return;
        
        if (!inCombat)
        {
            if (Settings.manualReactivation) Settings.boostToggle = false;
            inCombat = true;
            mult = combatMult;
            
           if (Settings.onOffNotification) Messages.Message("OOCMSB.Message.SpeedBoostDisabled".Translate(), MessageTypeDefOf.SilentInput);
        }
        else
            refreshTicks = true;
    }

    public static void Resume()
    {
        inCombat = false;
        mult = Settings.speedBoostMult;
        
        if (Settings.onOffNotification && !Settings.manualReactivation) Messages.Message("OOCMSB.Message.SpeedBoostReEnabled".Translate(), MessageTypeDefOf.SilentInput);
    }

    public static void ManualOverride()
    {
        Settings.manualOverride = !Settings.manualOverride;

        mult = Settings.manualOverride switch
        {
            true when inCombat => Settings.speedBoostMult,
            false when inCombat => Settings.combatSpeedMult,
            _ => mult
        };
        
        Messages.Message("OOCMSB.Message.ManualOverrideToggle".Translate(Settings.manualOverride ? "on" : "off"), MessageTypeDefOf.SilentInput);
    }
}