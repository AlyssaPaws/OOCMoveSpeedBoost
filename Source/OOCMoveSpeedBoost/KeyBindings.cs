using RimWorld;
using Verse;

#nullable disable
namespace OOCMoveSpeedBoost;

[DefOf]
public static class KeyBindings
{
    static KeyBindings() => DefOfHelper.EnsureInitializedInCtor(typeof(KeyBindingDefOf));
    
    public static KeyBindingDef MUR_ToggleBoost;

    public static KeyBindingDef MUR_ManualOverride;

    public static KeyBindingDef MUR_ManualReactivation;
}