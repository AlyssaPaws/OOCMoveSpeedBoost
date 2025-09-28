using Verse;

#nullable disable
namespace OOCMoveSpeedBoost;

[StaticConstructorOnStartup]
public class Startup
{
    static Startup() => MainController.mult = Settings.speedBoostMult;
}