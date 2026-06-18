using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

#nullable disable
namespace OOCMoveSpeedBoost;

[StaticConstructorOnStartup]
public class Resources
{
    public static Texture2D boostToggleIcon = ContentFinder<Texture2D>.Get("OOCMoveSpeedBoost/BoostToggle");

    public static string OnKeyStr = "OOCMSB.On".Translate();
    public static string OffKeyStr = "OOCMSB.Off".Translate();

    public static string ManualToggleTooltip = "OOCMSB.BoostToggle.TT".Translate(Settings.speedBoostMult, Settings.combatSpeedMult);

    public static string BoostDisabledStr = "OOCMSB.Message.SpeedBoostDisabled".Translate();
    public static string BoostReEnabledStr = "OOCMSB.Message.SpeedBoostReEnabled".Translate();
}