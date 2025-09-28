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
    
    public static string ManualToggleTooltip = "OOCMSB.BoostToggle.Tooltip".Translate(Settings.speedBoostMult.ToString(CultureInfo.CurrentCulture));

    public static void ForceSlow()
    {
        if (!inCombat)
        {
            if (Settings.manualReactivation) Settings.boostToggle = false;
            inCombat = true;
            mult = 1f;
            
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
}