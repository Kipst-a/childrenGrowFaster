using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;
using System;
using TaleWorlds.Core;

namespace childrenGrowFaster
{
    public class SubModuleSettings : AttributeGlobalSettings<SubModuleSettings>
    {
        [SettingPropertyFloatingInteger("Growth Rate", 0f, 40f, "0.0", Order = 0, RequireRestart = false, HintText = "Adjusts the rate at which all children grow.")]
        [SettingPropertyGroup("Children Growth Rate Settings")]
        public float newGrowthRate { get; set; } = 15f;

        [SettingPropertyFloatingInteger("Adult Growth Rate", 0f, 10f, "0.0", Order = 1, RequireRestart = false, HintText = "Adjusts the rate at which adults grow.")]
        [SettingPropertyGroup("Children Growth Rate Settings")]
        public float adultGrowthRate { get; set; } = 3f;

        [SettingPropertyBool("Affect player children only", Order = 2, RequireRestart = false, HintText = "Mod only affects growth rate of player children.")]
        [SettingPropertyGroup("Children Growth Rate Settings")]
        public bool affectOnlyPlayerChildren { get; set; } = true;

        [SettingPropertyBool("Instant Growth", Order = 3, RequireRestart = false, HintText = "Children will grow to adulthood instantly.")]
        [SettingPropertyGroup("Children Growth Rate Settings")]
        public bool instantGrowth { get; set; } = false;

        [SettingPropertyBool("Affect Everyone", Order = 4, RequireRestart = false, HintText = "Mod affects all children & adults.")]
        [SettingPropertyGroup("Children Growth Rate Settings")]
        public bool affectEveryone { get; set; } = false;

        [SettingPropertyBool("Random Traits for Children", Order = 1, HintText = "Enables random traits for children")]
        [SettingPropertyGroup("Random Traits")]
        public bool randomTraitsEnabled { get; set; } = false;

        [SettingPropertyFloatingInteger("Trait Chance", 0.05f, 1f, "0.0", Order = 2, RequireRestart = false, HintText = "Chance of children gaining a random trait")]
        [SettingPropertyGroup("Random Traits")]
        public float traitChance { get; set; } = 0.05f;

        [SettingPropertyBool("Enable adjustable pregnancy duration", Order = 1, RequireRestart = false, HintText = "Must be enabled for the slider to work")]
        [SettingPropertyGroup("Pregnancy Duration")]
        public bool enablePregnancyDuration { get; set; } = false;

        [SettingPropertyInteger("Adjust Pregnancy Duration", minValue: 1, maxValue: 36, Order = 2, RequireRestart = false, HintText = "Adjust the number of days it takes for children to be born.")]
        [SettingPropertyGroup("Pregnancy Duration")]
        public int AdjsutPregnancyDuration { get; set; } = 36;

        public override string Id => "childrenGrowFaster";
        public override string DisplayName => "Children Grow Faster";
        public override string FolderName => "childrenGrowFaster";
        public override string FormatType => "xml";
    }
}