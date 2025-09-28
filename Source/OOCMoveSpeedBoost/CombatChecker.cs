using Verse;

#nullable disable
namespace OOCMoveSpeedBoost;

public class CombatChecker : GameComponent
{
    private int checkCombatTicks = MainController.checkCombatTicks;

    public CombatChecker(Game game)
    {
    }

    public override void LoadedGame()
    {
        base.LoadedGame();
        if (!this.NotSafeToBoost())
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
            this.checkCombatTicks = MainController.checkCombatTicks;
            MainController.refreshTicks = false;
        }

        --this.checkCombatTicks;
        if (this.checkCombatTicks <= 0)
        {
            this.checkCombatTicks = MainController.checkCombatTicks;
            if (this.NotSafeToBoost())
                return;
            MainController.Resume();
        }
    }

    private bool NotSafeToBoost()
    {
        foreach (Map map in Find.Maps)
        {
            if (CustomGenHostility.AnyHostileActiveThreatToPlayer(map))
                return true;
            if (Settings.disableWhenDrafted)
            {
                foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
                {
                    if (pawn.Drafted)
                        return true;
                }
            }
        }

        return false;
    }

    public void RefreshCountdown()
    {
        if (!MainController.inCombat)
            return;
        this.checkCombatTicks = MainController.checkCombatTicks;
    }
}