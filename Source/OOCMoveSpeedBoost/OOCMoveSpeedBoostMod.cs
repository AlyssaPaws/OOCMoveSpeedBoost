using System;
using System.Globalization;
using UnityEngine;
using Verse;

#nullable disable
namespace OOCMoveSpeedBoost;

public class OOCMoveSpeedBoostMod : Mod
{
    public static Settings settings;

    public OOCMoveSpeedBoostMod(ModContentPack content) : base(content)
    {
        settings = GetSettings<Settings>();
    }

    public override string SettingsCategory() => "OOCMSB.Title".Translate();

    public override void DoSettingsWindowContents(Rect inRect)
    {
        Listing_Standard lsHeader1 = new Listing_Standard();
        
        /*ListingStandared - Header 1*/
        lsHeader1.Begin(new Rect(inRect.x, inRect.y, inRect.width, 40f));
        lsHeader1.Label("OOCMSB.General.Header".Translate());
        lsHeader1.GapLine();
        lsHeader1.End();
        /*------------------------*/
        
        /*ListingStandard - SpeedMultiplier*/
        Listing_Standard lsSpeedMult = new Listing_Standard();
        lsSpeedMult.Begin(new Rect(inRect.x, inRect.y + 40f, 500f, 30f));
        
        double num = lsSpeedMult.SliderLabeled("OOCMSB.NonCombatSpeedMult.Label".Translate()+ $": {Math.Round(Settings.speedBoostMult, 1)}", Settings.speedBoostMult, 0.1f, 10f, tooltip: "OOCMSB.NonCombatSpeedMult.TT".Translate());
        num = Math.Round(num, 1);
        Settings.speedBoostMult = (float)num;

        lsSpeedMult.End();
        /*-----------------------*/
        
        /*ListingStandard - Check boxes section*/
        Listing_Standard lsCheckboxes = new Listing_Standard();
        lsCheckboxes.Begin(new Rect(inRect.x, inRect.y + 80f, 480f, 150f));
        lsCheckboxes.verticalSpacing = 10f;
        
        lsCheckboxes.CheckboxLabeled("OOCMSB.ShowBoostToggle.Label".Translate(), ref Settings.showBoostToggle, "OOCMSB.ShowBoostToggle.TT".Translate());
        lsCheckboxes.CheckboxLabeled("OOCMSB.ManualReactivation.Label".Translate(), ref Settings.manualReactivation, "OOCMSB.ManualReactivation.TT".Translate());
        lsCheckboxes.CheckboxLabeled("OOCMSB.DraftedDisable.Label".Translate(), ref Settings.disableWhenDrafted, "OOCMSB.DraftedDisable.TT".Translate());
        lsCheckboxes.CheckboxLabeled("OOCMSB.OnOffNotification.Label".Translate(), ref Settings.onOffNotification, "OOCMSB.OnOffNotification.TT".Translate());

        lsCheckboxes.End();
        /*----------------------------------*/
        
        /*ListingStandard - Advanced settings header*/
        Listing_Standard lsHeader2 = new Listing_Standard();
        lsHeader2.Begin(new Rect(inRect.x, inRect.y + 240f, inRect.width, 40f));
        lsHeader2.Label("OOCMSB.Advanced.Header".Translate());
        lsHeader2.GapLine();
        lsHeader2.End();
        /*--------------------------------*/
        
        /*ListingStandard - combat speed multiplier*/
        Listing_Standard lsCombatSpeedMult = new Listing_Standard();
        lsCombatSpeedMult.Begin(new Rect(inRect.x, inRect.y + 280f, 500f, 30f));

        double num2 = lsCombatSpeedMult.SliderLabeled("OOCMSB.CombatSpeedMult.Label".Translate()+ $": {Math.Round(Settings.combatSpeedMult, 1)}", Settings.combatSpeedMult, 0.1f, 10f, tooltip: "OOCMSB.CombatSpeedMult.TT".Translate());
        num2 = Math.Round(num2, 1);
        Settings.combatSpeedMult = (float)num2;
        
        lsSpeedMult.End();
        /*------------------*/
        
        Listing_Standard lsResetToDefault = new Listing_Standard();
        lsResetToDefault.Begin(new Rect(inRect.x, inRect.height - 40f, inRect.width, 40f));
        if (lsResetToDefault.ButtonText("OOCMSB.ResetValues.btn".Translate()))
        {
            Settings.speedBoostMult = 3f;
            Settings.combatSpeedMult = 1f;

            Settings.showBoostToggle = true;
            Settings.manualReactivation = false;
            Settings.disableWhenDrafted = false;
            Settings.onOffNotification = false;
        }
        lsResetToDefault.End();
        
        settings.Write();
    }

    public override void WriteSettings()
    {
        base.WriteSettings();
        
        if (!MainController.inCombat && Settings.boostToggle) MainController.mult = Settings.speedBoostMult;
        MainController.combatMult = Settings.combatSpeedMult;
        MainController.ManualToggleTooltip = "OOCMSB.BoostToggle.TT".Translate(Settings.speedBoostMult,  Settings.combatSpeedMult);
    }
}