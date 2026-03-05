using Verse;

#nullable disable
namespace OOCMoveSpeedBoost;

public class Settings : ModSettings
{
    public static float speedBoostMult = 3f;
    public static float combatSpeedMult = 1f;
    public static bool showBoostToggle = true;
    public static bool boostToggle = true;
    public static bool manualOverride = false;
    public static bool manualReactivation;
    public static bool disableWhenDrafted;
    public static bool onOffNotification;

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref speedBoostMult, "speedBoostMult", 3f, true);
        Scribe_Values.Look(ref combatSpeedMult, "combatSpeedMult", 1f, true);
        Scribe_Values.Look(ref showBoostToggle, "showBoostToggle", true, true);
        Scribe_Values.Look(ref boostToggle, "speedBoostToggle", true, true);
        Scribe_Values.Look(ref manualOverride, "manualOverride", false, true);
        Scribe_Values.Look(ref manualReactivation, "manualReactivation", forceSave: true);
        Scribe_Values.Look(ref disableWhenDrafted, "disableWhenDrafted", forceSave: true);
        Scribe_Values.Look(ref onOffNotification, "onOffNotification", forceSave: true);
    }
}