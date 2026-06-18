using System;
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace OOCMoveSpeedBoost;

public static class HostilityChecker
{
    private static readonly List<string> ignorePawnDefs = ["BMT_PustuleHornet", "DankPyon_Deathstinger"];
    const int waitTick = 1200;
    
    public static bool AnyThreatToPlayer(Map map)
    {
        int curTick = Find.TickManager.TicksGame;
        
        foreach (IAttackTarget target in map.attackTargetsCache.TargetsHostileToFaction(Faction.OfPlayer))
        {
            Pawn targetPawn = target.Thing as Pawn;
            
            // Ignore solitary insects
            if (targetPawn != null && targetPawn.RaceProps.Insect && targetPawn.GetLord() == null) continue;
            
            
            //checks HostileTo (dormancy, among others),
            //IsPotentialThreat (dormancy(?), mech deactivated, CompInitiatable),
            //ignores fogged, ignores hives and insects not attacking
            if (GenHostility.IsActiveThreatToPlayer(target))
            {
                Pawn enemyTarget = targetPawn?.mindState?.enemyTarget as Pawn;
                bool ticksPassed = curTick > targetPawn?.mindState?.lastEngageTargetTick + waitTick;

                if (!ignorePawnDefs.Contains(targetPawn?.def.defName)) return true;
                if (ticksPassed && enemyTarget != null && enemyTarget.Faction == Faction.OfPlayer)
                    return false;
            }


            //Present in GenHostility.IsPotentialThreat, but only checked if "map.generatorDef.defeatRequiresCantReachUnfogged"
            if (!target.Thing.Spawned || !target.Thing.Fogged() || map.generatorDef.defeatRequiresCantReachUnfogged) continue;
            TraverseParms traverseParms = targetPawn != null 
                ? TraverseParms.For(targetPawn)
                : TraverseParms.For(TraverseMode.PassDoors);
            return target.Thing.Map.reachability.CanReachUnfogged(target.Thing.Position, traverseParms);
        }

        return false;
    }
}