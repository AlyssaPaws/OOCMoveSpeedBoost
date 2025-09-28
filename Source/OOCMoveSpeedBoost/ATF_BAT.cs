using HarmonyLib;
using RimWorld;
using Verse.AI;

#nullable disable
namespace OOCMoveSpeedBoost;

[HarmonyPatch(typeof (AttackTargetFinder), "BestAttackTarget")]
public static class ATF_BAT
{
    private static void Postfix(IAttackTarget __result)
    {
        if (__result == null || __result.Thing.Faction != Faction.OfPlayer)
            return;
        MainController.ForceSlow();
    }
}