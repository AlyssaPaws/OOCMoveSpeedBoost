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
            if (countSolitaryInsectsAsHostile || !(target is Pawn p) || !p.RaceProps.Insect || p.GetLord() != null)
            {
                if (IsActiveThreatTo(target, faction))
                {
                    threat = target;
                    return true;
                }

                if (countDormantPawnsAsHostile && target.Thing.HostileTo(faction) && !target.Thing.Fogged() &&
                    !target.ThreatDisabled((IAttackTargetSearcher)null) && target.Thing is Pawn thing)
                {
                    CompCanBeDormant comp = thing.GetComp<CompCanBeDormant>();
                    if (comp != null && !comp.Awake)
                    {
                        threat = target;
                        return true;
                    }
                }
            }
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
        if (lord != null && lord.LordJob is LordJob_DefendAndExpandHive && (targetPawn.mindState.duty == null || targetPawn.mindState.duty.def != DutyDefOf.AssaultColony))
            return false;

        if (targetPawn != null && targetPawn.IsPrisoner)
            return false;
        
        CompCanBeDormant compCanBeDormant = target.Thing.TryGetComp<CompCanBeDormant>();
        if (compCanBeDormant != null && !compCanBeDormant.Awake)
            return false;
        
        CompInitiatable compInit = target.Thing.TryGetComp<CompInitiatable>();
        if (compInit != null && !compInit.Initiated)
            return false;
        
        if (target.Thing.Spawned)
        {
            TraverseParms traverseParms = targetPawn != null 
                ? TraverseParms.For(targetPawn)
                : TraverseParms.For(TraverseMode.PassDoors);
            if (!target.Thing.Map.reachability.CanReachUnfogged(target.Thing.Position, traverseParms))
                return false;
        }

        return true;
    }
}