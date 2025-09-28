using System.Globalization;
using UnityEngine;
using Verse;

#nullable disable
namespace OOCMoveSpeedBoost;

public class OOCMoveSpeedBoost_Mod : Mod
{
    public static Settings settings;

    public OOCMoveSpeedBoost_Mod(ModContentPack content) : base(content)
    {
        settings = GetSettings<Settings>();
    }

    public override string SettingsCategory() => "OOCMSB.Title".Translate();

    public override void DoSettingsWindowContents(Rect inRect)
    {
        Listing_Standard listingStandard = new Listing_Standard();
        listingStandard.Begin(new Rect(inRect.x, inRect.y + 24f, inRect.width, inRect.height - 24f));
        listingStandard.verticalSpacing = 10f;
        listingStandard.ColumnWidth = 250f;
        listingStandard.Gap(5f);
        listingStandard.Label("OOCMSB.NonCombatSpeedMult.Label".Translate(), -1f, "OOCMSB.NonCombatSpeedMult.TT".Translate());
        listingStandard.ColumnWidth += 74f;
        listingStandard.CheckboxLabeled("OOCMSB.ShowBoostToggle.Label".Translate(), ref Settings.showBoostToggle, "OOCMSB.ShowBoostToggle.TT".Translate());
        listingStandard.CheckboxLabeled("OOCMSB.ManualReactivation.Label".Translate(), ref Settings.manualReactivation, "OOCMSB.ManualReactivation.TT".Translate());
        listingStandard.CheckboxLabeled("OOCMSB.DraftedDisable.Label".Translate(), ref Settings.disableWhenDrafted, "OOCMSB.DraftedDisable.TT".Translate());
        listingStandard.CheckboxLabeled("OOCMSB.OnOffNotification.Label".Translate(), ref Settings.onOffNotification, "OOCMSB.OnOffNotification.TT".Translate());
        listingStandard.ColumnWidth -= 74f;
        listingStandard.NewColumn();
        listingStandard.ColumnWidth = 32f;
        if (listingStandard.ButtonText("-"))
        {
            --Settings.speedBoostMult;
            if ((double)Settings.speedBoostMult < 1.1000000238418579)
                Settings.speedBoostMult = 1.1f;
        }

        listingStandard.ColumnWidth = 16f;
        listingStandard.NewColumn();
        listingStandard.ColumnWidth = 64f;
        listingStandard.Gap(5f);
        string buffer = Settings.speedBoostMult.ToString(CultureInfo.CurrentCulture);
        listingStandard.TextFieldNumeric<float>(ref Settings.speedBoostMult, ref buffer, 1.1f, 10f);
        listingStandard.ColumnWidth = 48f;
        listingStandard.NewColumn();
        listingStandard.ColumnWidth = 32f;
        if (listingStandard.ButtonText("+"))
        {
            ++Settings.speedBoostMult;
            if ((double)Settings.speedBoostMult > 10.0)
                Settings.speedBoostMult = 10f;
        }

        listingStandard.End();
        settings.Write();
    }

    public override void WriteSettings()
    {
        base.WriteSettings();
        MainController.mult = Settings.speedBoostMult;
        MainController.ManualToggleTooltip = "OOCMSB.BoostToggle.Tooltip".Translate($"{Settings.speedBoostMult}");
    }
}