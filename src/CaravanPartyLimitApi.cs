using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.Party;

namespace CaravanPartyLimitIncreasedFixed
{
    // Public compatibility surface for Castle Market Towns and similar mods.
    // True only when CPI is enabled, this party is a caravan CPI affects, and a limit was computed.
    public static class CaravanPartyLimitApi
    {
        public static bool TryGetEffectiveCaravanPartyLimit(PartyBase party, out int limit)
        {
            limit = 0;
            Settings settings = Settings.Instance;
            if (settings == null || !settings.Enabled || party == null || !party.IsMobile)
                return false;

            MobileParty mobileParty = party.MobileParty;
            if (mobileParty == null || !mobileParty.IsCaravan)
                return false;

            if (!CaravanLimitPatches.ShouldAffectCaravan(party, settings))
                return false;

            int desired = CaravanLimitPatches.CalculateCaravanLimit(party, settings);
            if (desired < 1)
                return false;

            int cap = ClanTierCaravanCap.TryGetCap();
            if (cap > 0 && desired > cap)
                desired = cap;

            limit = desired;
            return true;
        }
    }
}
