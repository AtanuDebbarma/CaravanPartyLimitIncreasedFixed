using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;

namespace CaravanPartyLimitIncreasedFixed
{
    // Options for Clan Tier based Caravan Party Size (Nexus 661) by Grant2600 — 1.4.8 local port.
    public sealed class Settings : AttributeGlobalSettings<Settings>
    {
        internal const string FixedLimitGroupMarker = "cpl_mcm_fixed_limit";
        internal const string TierLimitGroupMarker = "cpl_mcm_tier_limit";

        private const string FixedLimitGroupName = "cpl_mcm_fixed_limit/Fixed limit";
        private const string TierLimitGroupName = "cpl_mcm_tier_limit/Clan tier formula";

        private bool _clanTierBased = true;
        private int _uiVersion = 1;

        public override string DisplayName
        {
            get { return "Caravan Party Limit Increased Fixed"; }
        }

        public override string FolderName
        {
            get { return "CaravanPartyLimitIncreasedFixed"; }
        }

        public override string Id
        {
            get { return "CaravanPartyLimitIncreasedFixed_v1.0.0"; }
        }

        public override string FormatType
        {
            get { return "json2"; }
        }

        public override int UIVersion
        {
            get { return _uiVersion; }
        }

        [SettingPropertyBool("Enabled", IsToggle = true, Order = 0, RequireRestart = false, HintText = "Use custom caravan party sizes.")]
        [SettingPropertyGroup("Enabled", GroupOrder = 0)]
        public bool Enabled { get; set; }

        [SettingPropertyBool("Player caravans", Order = 1, RequireRestart = false, HintText = "Apply to your own caravans.")]
        [SettingPropertyGroup("Enabled/Who it affects")]
        public bool AffectPlayerCaravans { get; set; }

        [SettingPropertyBool("Same-kingdom caravans", Order = 2, RequireRestart = false, HintText = "Apply to other caravans in your kingdom, including notable merchants.")]
        [SettingPropertyGroup("Enabled/Who it affects")]
        public bool AffectSameKingdomCaravans { get; set; }

        [SettingPropertyBool("Other kingdoms and clans", Order = 3, RequireRestart = false, HintText = "Apply to caravans outside your kingdom.")]
        [SettingPropertyGroup("Enabled/Who it affects")]
        public bool AffectOtherKingdomsAndClans { get; set; }

        [SettingPropertyBool("Clan tier based", Order = 4, RequireRestart = false, HintText = "On: 30 + (extra slots × clan tier). Off: use the fixed limit.")]
        [SettingPropertyGroup("Enabled")]
        public bool ClanTierBased
        {
            get { return _clanTierBased; }
            set
            {
                if (_clanTierBased == value)
                    return;
                _clanTierBased = value;
                _uiVersion++;
                OnPropertyChanged("ClanTierBased");
                OnPropertyChanged("UIVersion");
            }
        }

        [SettingPropertyInteger("Fixed caravan party limit", 1, 100000, "0", Order = 5, RequireRestart = false, HintText = "Party size when clan tier based is off. Default 50.")]
        [SettingPropertyGroup("Enabled/" + FixedLimitGroupName)]
        public int CaravanPartyLimit { get; set; }

        [SettingPropertyInteger("Extra slots per clan tier", 0, 100, "0", Order = 6, RequireRestart = false, HintText = "Added per clan tier when clan tier based is on. Default 15.")]
        [SettingPropertyGroup("Enabled/" + TierLimitGroupName)]
        public int ExtraPartySlotsPerTierRank { get; set; }

        public Settings()
        {
            Enabled = true;
            AffectPlayerCaravans = true;
            AffectSameKingdomCaravans = true;
            AffectOtherKingdomsAndClans = true;
            _clanTierBased = true;
            CaravanPartyLimit = 50;
            ExtraPartySlotsPerTierRank = 15;
        }
    }
}
