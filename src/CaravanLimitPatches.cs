using System;
using System.Reflection;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using TaleWorlds.CampaignSystem.Party;
using TaleWorlds.Localization;

namespace CaravanPartyLimitIncreasedFixed
{
    // Patches the active PartySizeLimitModel so Improved Garrisons (and other replacements) still work.
    internal static class CaravanLimitPatches
    {
        private static readonly TextObject LimitText = new TextObject("{=cpl_caravan_size}Caravan party limit");
        private static MethodBase _patched;
        private static Harmony _harmony;

        internal static void PatchActiveModel(Harmony harmony)
        {
            if (harmony != null)
                _harmony = harmony;
            if (_harmony == null)
                return;

            PartySizeLimitModel model = Campaign.Current != null ? Campaign.Current.Models.PartySizeLimitModel : null;
            if (model == null)
            {
                SubModule.ReportProblem("No party size model found; caravan limits unchanged.");
                return;
            }

            MethodInfo target = FindPartyMemberSizeLimit(model.GetType());
            if (target == null)
            {
                SubModule.ReportProblem("GetPartyMemberSizeLimit missing on " + model.GetType().FullName + ".");
                return;
            }

            if (_patched == target)
                return;

            try
            {
                if (_patched != null)
                    _harmony.Unpatch(_patched, HarmonyPatchType.All, _harmony.Id);

                _harmony.Patch(
                    target,
                    postfix: new HarmonyMethod(typeof(CaravanLimitPatches).GetMethod(
                        "GetPartyMemberSizeLimitPostfix", BindingFlags.Static | BindingFlags.NonPublic)));
                _patched = target;
            }
            catch (Exception e)
            {
                SubModule.ReportProblem("Failed to patch party size model: " + e.Message);
            }
        }

        // Re-bind if another mod replaced PartySizeLimitModel after our first patch.
        internal static void EnsurePatched()
        {
            if (Campaign.Current == null || _harmony == null)
                return;

            PartySizeLimitModel model = Campaign.Current.Models.PartySizeLimitModel;
            if (model == null)
                return;

            MethodInfo target = FindPartyMemberSizeLimit(model.GetType());
            if (target != null && target != _patched)
                PatchActiveModel(_harmony);
        }

        private static MethodInfo FindPartyMemberSizeLimit(Type type)
        {
            Type[] args = new Type[] { typeof(PartyBase), typeof(bool) };
            while (type != null && type != typeof(object))
            {
                MethodInfo method = type.GetMethod(
                    "GetPartyMemberSizeLimit",
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly,
                    null,
                    args,
                    null);
                if (method != null)
                    return method;
                type = type.BaseType;
            }

            return null;
        }

        private static void GetPartyMemberSizeLimitPostfix(PartyBase party, bool includeDescriptions, ref ExplainedNumber __result)
        {
            Settings settings = Settings.Instance;
            if (settings == null || !settings.Enabled || party == null || !party.IsMobile)
                return;

            MobileParty mobileParty = party.MobileParty;
            if (mobileParty == null || !mobileParty.IsCaravan)
                return;

            if (!ShouldAffectCaravan(party, settings))
                return;

            int desired = CalculateCaravanLimit(party, settings);
            if (desired < 1)
                return;

            int cap = ClanTierCaravanCap.TryGetCap();
            if (cap > 0 && desired > cap)
                desired = cap;

            float current = __result.ResultNumber;
            float delta = desired - current;
            if (delta > 0.01f || delta < -0.01f)
                __result.Add(delta, LimitText, null);
        }

        internal static bool ShouldAffectCaravan(PartyBase party, Settings settings)
        {
            Clan playerClan = Clan.PlayerClan;

            if (IsPlayerCaravan(party, playerClan))
                return settings.AffectPlayerCaravans;

            if (IsSameKingdomCaravan(party, playerClan))
                return settings.AffectSameKingdomCaravans;

            return settings.AffectOtherKingdomsAndClans;
        }

        private static bool IsPlayerCaravan(PartyBase party, Clan playerClan)
        {
            if (party.Owner == Hero.MainHero)
                return true;

            Clan caravanClan = GetCaravanClan(party);
            if (playerClan != null && caravanClan == playerClan)
                return true;
            return false;
        }

        // Notables usually have no Clan; their kingdom comes from MapFaction / home settlement.
        private static bool IsSameKingdomCaravan(PartyBase party, Clan playerClan)
        {
            if (playerClan == null)
                return false;

            Kingdom playerKingdom = playerClan.Kingdom;
            if (playerKingdom == null)
                return false;

            IFaction faction = GetCaravanMapFaction(party);
            if (faction == null || faction == playerClan)
                return false;

            if (faction == playerKingdom)
                return true;

            Kingdom kingdom = faction as Kingdom;
            if (kingdom != null)
                return kingdom == playerKingdom;

            Clan clan = faction as Clan;
            return clan != null && clan.Kingdom == playerKingdom;
        }

        private static Clan GetCaravanClan(PartyBase party)
        {
            Hero leader = party.LeaderHero;
            if (leader != null && leader.Clan != null)
                return leader.Clan;

            Hero owner = party.Owner;
            if (owner != null)
                return owner.Clan;
            return null;
        }

        private static IFaction GetCaravanMapFaction(PartyBase party)
        {
            if (party == null)
                return null;

            MobileParty mobile = party.MobileParty;
            if (mobile != null && mobile.MapFaction != null)
                return mobile.MapFaction;

            Hero owner = party.Owner;
            if (owner != null && owner.MapFaction != null)
                return owner.MapFaction;

            Hero leader = party.LeaderHero;
            if (leader != null && leader.MapFaction != null)
                return leader.MapFaction;

            return null;
        }

        internal static int CalculateCaravanLimit(PartyBase party, Settings settings)
        {
            if (!settings.ClanTierBased)
                return settings.CaravanPartyLimit;

            const int baseSize = 30;
            int perTier = settings.ExtraPartySlotsPerTierRank;

            Hero leader = party.LeaderHero;
            if (leader != null && leader.Clan != null)
                return baseSize + perTier * leader.Clan.Tier;

            Hero owner = party.Owner;
            if (owner != null && owner.IsNotable)
            {
                float power = owner.Power;
                int tierProxy = 1;
                if (power >= 200f)
                    tierProxy = 3;
                else if (power >= 100f)
                    tierProxy = 2;
                return baseSize + perTier * tierProxy;
            }

            return settings.CaravanPartyLimit;
        }
    }
}
