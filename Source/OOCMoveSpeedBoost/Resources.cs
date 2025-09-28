using UnityEngine;
using Verse;

#nullable disable
namespace OOCMoveSpeedBoost;

[StaticConstructorOnStartup]
public class Resources
{
    public static Texture2D boostToggleIcon = ContentFinder<Texture2D>.Get("OOCMoveSpeedBoost/BoostToggle");
}