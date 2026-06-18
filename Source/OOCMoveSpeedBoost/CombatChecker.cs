using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

#nullable disable
namespace OOCMoveSpeedBoost;

public class CombatChecker : GameComponent
{
    private int _checkCombatTicks = MainController.checkCombatTicks;

    private static Dictionary<Map, bool> _boostDict = [];
    private static Dictionary<Map, bool> _manualOverrideDict = [];

    public CombatChecker(Game game)
    {
    }

    public override void LoadedGame()
    {
        base.LoadedGame();

        RefreshMapList();
    }

    public override void GameComponentTick()
    {
        base.GameComponentTick();
        if (!Settings.boostToggle) return;
            
        if (MainController.refreshTicks)
        {
            _checkCombatTicks = MainController.checkCombatTicks;
            MainController.refreshTicks = false;
        }

        --_checkCombatTicks;
        if (_checkCombatTicks > 0) return;
        
        _checkCombatTicks = MainController.checkCombatTicks;
        
        UpdateMapCombatStates();
    }
    
    public static void UpdateMapCombatStates()
    {
        foreach (Map map in Find.Maps)
        {
            if (IsSafeToBoost(map)) MainController.ResumeForMap(map);
        }
    }

    public static void RefreshMapList()
    {
        Dictionary<Map, bool> boostDict = _boostDict;
        Dictionary<Map, bool> manualOverrideDict = _manualOverrideDict;
        
        List<Map> activeMaps = Find.Maps.ToList();
        
        foreach (KeyValuePair<Map, bool> entry in boostDict.ToList())
        {
            if (activeMaps.Contains(entry.Key)) continue;
            
            boostDict.Remove(entry.Key);
            manualOverrideDict.Remove(entry.Key);
        }

        foreach (Map map in activeMaps)
        {
            boostDict.TryAdd(map, false);
            manualOverrideDict.TryAdd(map, false);
        }

        _boostDict = boostDict;
        _manualOverrideDict = manualOverrideDict;
    }

    private static bool IsSafeToBoost(Map map)
    {
        if (HostilityChecker.AnyThreatToPlayer(map))
            return false;

        if (!Settings.disableWhenDrafted) return true;

        return !AnyPawnsDrafted(map);
    }

    public static void ForceSlowCheckMaps()
    {
        foreach (Map map in Find.Maps.ToList())
        {
            if (HostilityChecker.AnyThreatToPlayer(map))
                MainController.ForceSlowForMap(map);
        }
    }
    
    public static bool IsCombatActive(Map map) => _boostDict.GetValueOrDefault(map, false);
    public static bool IsManualOverride(Map map) => _manualOverrideDict.GetValueOrDefault(map, false);
    public static bool AnyPawnsDrafted(Map map) => Enumerable.Any(map.mapPawns.FreeColonistsSpawned, pawn => pawn.Drafted);
    
    public static void SetCombatState(Map map, bool combatActive) => _boostDict[map] = combatActive;
    public static void SetManualOverride(Map map, bool manualOverride) => _manualOverrideDict[map] = manualOverride;
}