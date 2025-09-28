using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;

#nullable disable
namespace OOCMoveSpeedBoost;

[StaticConstructorOnStartup]
public class HarmonyPatches
{
    static HarmonyPatches() => new Harmony("alyssapaws.oocmovespeedboost").PatchAll();
}

[HarmonyPatch(typeof(Pawn_PathFollower), "CostToMoveIntoCell", new System.Type[] { typeof(Pawn), typeof(IntVec3) })]
public class Pawn_PathFollower_CostToMoveIntoCell
{
    private static void Postfix(Pawn pawn, IntVec3 c, ref float __result)
    {
        if (!Settings.boostToggle)
            return;
        __result /= MainController.mult;
    }
}

[HarmonyPatch(typeof(Pawn), nameof(Pawn.TryStartAttack))]
public static class Pawn_TryStartAttack
{
    private static void Postfix(bool __result, ref Pawn __instance)
    {
        if (!__result || __instance.Faction != Faction.OfPlayer)
            return;
        MainController.ForceSlow();
    }
}

[HarmonyPatch(typeof(Pawn_DraftController), nameof(Pawn_DraftController.Drafted), MethodType.Setter)]
public static class PawnDraftController_Drafted
{
    private static void Postfix(Pawn_DraftController __instance)
    {
        if (!__instance.Drafted || !Settings.disableWhenDrafted) return;
        
        MainController.ForceSlow();
    }
}

[HarmonyPatch(typeof(PlaySettings), nameof(PlaySettings.DoPlaySettingsGlobalControls))]
public static class PlaySettings_DoPlaySettingsGlobalControls
{
    private static void Postfix(WidgetRow row)
    {
        if (!Settings.showBoostToggle)
            return;
        row.ToggleableIcon(ref Settings.boostToggle, Resources.boostToggleIcon, MainController.ManualToggleTooltip, SoundDefOf.Mouseover_ButtonToggle);
    }
}

[HarmonyPatch(typeof(TimeSlower), nameof(TimeSlower.SignalForceNormalSpeed))]
public class TimeSlower_SignalForceNormalSpeed
{
    private static void Postfix() => MainController.ForceSlow();
}

[HarmonyPatch(typeof(TimeSlower), nameof(TimeSlower.SignalForceNormalSpeedShort))]
public class TimeSlower_SignalForceNormalSpeedShort
{
    private static void Postfix() => MainController.ForceSlow();
}

[HarmonyPatch(typeof(UIRoot), nameof(UIRoot.UIRootOnGUI))]
public static class UIRoot_UIRootOnGUI
{
    private static void Postfix() => KeyBindingHandler.OnGUI();
}