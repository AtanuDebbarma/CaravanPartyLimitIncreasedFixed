using TaleWorlds.CampaignSystem;

namespace CaravanPartyLimitIncreasedFixed
{
    // If another mod replaces PartySizeLimitModel after launch, rebind our postfix.
    internal sealed class CaravanLimitRebindBehavior : CampaignBehaviorBase
    {
        public override void RegisterEvents()
        {
            CampaignEvents.DailyTickEvent.AddNonSerializedListener(this, OnDailyTick);
            CampaignEvents.OnSessionLaunchedEvent.AddNonSerializedListener(this, OnSessionLaunched);
        }

        public override void SyncData(IDataStore dataStore)
        {
        }

        private void OnSessionLaunched(CampaignGameStarter starter)
        {
            CaravanLimitPatches.EnsurePatched();
        }

        private void OnDailyTick()
        {
            CaravanLimitPatches.EnsurePatched();
        }
    }
}
