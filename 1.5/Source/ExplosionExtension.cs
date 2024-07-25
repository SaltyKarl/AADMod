using System.Diagnostics.CodeAnalysis;
using Verse;
using VFECore.Abilities;

namespace AADMod;

[HotSwappable]
[SuppressMessage("ReSharper", "UnassignedField.Global")]
public class ExplosionExtension : AbilityExtension_AbilityMod
{
    public float arc;
    public DamageDef damageDef;
}
