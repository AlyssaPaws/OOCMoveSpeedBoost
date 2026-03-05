using System.Linq;
using Verse;

#nullable disable
namespace OOCMoveSpeedBoost;

public class CombatChecker : GameComponent
{
    private int _checkCombatTicks = MainController.checkCombatTicks;

    public CombatChecker(Game game)
    {
    }

    public override void LoadedGame()
    {
        base.LoadedGame();
        if (!NotSafeToBoost())
            return;
        MainController.ForceSlow();
    }

    public override void GameComponentTick()
    {
        base.GameComponentTick();
        if (!MainController.inCombat)
            return;
        if (MainController.refreshTicks)
        {
            _checkCombatTicks = MainController.checkCombatTicks;
            MainController.refreshTicks = false;
        }

        --_checkCombatTicks;
        if (_checkCombatTicks > 0) return;
        
        _checkCombatTicks = MainController.checkCombatTicks;
        if (NotSafeToBoost())
            return;
        MainController.Resume();
    }

    private static bool NotSafeToBoost()
    {
        foreach (Map map in Find.Maps)
        {
            if (CustomGenHostility.AnyHostileActiveThreatToPlayer(map))
                return true;
            
            if (!Settings.disableWhenDrafted) continue;
            
            if (Enumerable.Any(map.mapPawns.FreeColonistsSpawned, pawn => pawn.Drafted))
                return true;
        }

        return false;
    }
}