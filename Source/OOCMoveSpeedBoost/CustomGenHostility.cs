using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;

#nullable disable
namespace OOCMoveSpeedBoost;

public static class CustomGenHostility
{
    public static bool AnyHostileActiveThreatToPlayer(Map map, bool countDormantPawnsAsHostile = false, bool countSolitaryInsectsAsHostile = true)
    {
        return AnyHostileActiveThreatTo(map, Faction.OfPlayer, countDormantPawnsAsHostile, countSolitaryInsectsAsHostile);
    }

    public static bool AnyHostileActiveThreatTo(Map map, Faction faction, bool countDormantPawnsAsHostile = false, bool countSolitaryInsectsAsHostile = true)
    {
        return AnyHostileActiveThreatTo(map, faction, out IAttackTarget _, countDormantPawnsAsHostile, countSolitaryInsectsAsHostile);
    }

    public static bool AnyHostileActiveThreatTo(Map map, Faction faction, out IAttackTarget threat, bool countDormantPawnsAsHostile = false, bool countSolitaryInsectsAsHostile = true)
    {
        foreach (IAttackTarget target in map.attackTargetsCache.TargetsHostileToFaction(faction))
        {
            if (!countSolitaryInsectsAsHostile && target is Pawn p && p.RaceProps.Insect &&
                p.GetLord() == null) continue;

            if (!IsActiveThreatTo(target, faction))
            {
                if (!countDormantPawnsAsHostile || !target.Thing.HostileTo(faction) || target.Thing.Fogged() ||
                    target.ThreatDisabled((IAttackTargetSearcher)null) || target.Thing is not Pawn thing) continue;

                CompCanBeDormant comp = thing.GetComp<CompCanBeDormant>();

                if (comp is not { Awake: false }) continue;
            }

            threat = target;
            return true;
        }

        threat = (IAttackTarget)null;
        return false;
    }

    public static bool IsActiveThreatTo(IAttackTarget target, Faction faction)
    {
        Pawn targetPawn = target.Thing as Pawn;
        
        if (!target.Thing.HostileTo(faction) || target.Thing is not IAttackTargetSearcher || target.ThreatDisabled((IAttackTargetSearcher)null))
            return false;

        Lord lord = targetPawn?.GetLord();
        if (lord is { LordJob: LordJob_DefendAndExpandHive } && (targetPawn.mindState.duty == null || targetPawn.mindState.duty.def != DutyDefOf.AssaultColony))
            return false;

        if (targetPawn is { IsPrisoner: true })
            return false;
        
        CompCanBeDormant compCanBeDormant = target.Thing.TryGetComp<CompCanBeDormant>();
        if (compCanBeDormant is { Awake: false })
            return false;
        
        CompInitiatable compInit = target.Thing.TryGetComp<CompInitiatable>();
        if (compInit is { Initiated: false })
            return false;

        if (!target.Thing.Spawned) return true;
        TraverseParms traverseParms = targetPawn != null 
            ? TraverseParms.For(targetPawn)
            : TraverseParms.For(TraverseMode.PassDoors);
        return target.Thing.Map.reachability.CanReachUnfogged(target.Thing.Position, traverseParms);
    }
}