using System;
using System.Collections.Generic;
using HarmonyLib;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace CaravanPartyLimitIncreasedFixed
{
    // Caravan Party Limit Increased Fixed for Bannerlord 1.4.8.
    public class SubModule : MBSubModuleBase
    {
        private const string LogPrefix = "[Caravan Party Limit Increased Fixed] ";
        public const string HarmonyId = "com.atanu.bannerlord.CaravanPartyLimitIncreasedFixed";
        private static readonly List<string> PendingProblems = new List<string>();
        private readonly Harmony _harmony = new Harmony(HarmonyId);

        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();
            try
            {
                _harmony.PatchAll(typeof(SubModule).Assembly);
            }
            catch (Exception e)
            {
                Debug.Print(LogPrefix + "Harmony patch failed: " + e.Message);
            }
        }

        protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
        {
            base.OnGameStart(game, gameStarterObject);
            if (!(game.GameType is Campaign))
                return;

            CampaignGameStarter starter = gameStarterObject as CampaignGameStarter;
            if (starter != null)
                starter.AddBehavior(new CaravanLimitRebindBehavior());
        }

        public override void OnGameInitializationFinished(Game game)
        {
            base.OnGameInitializationFinished(game);
            if (!(game.GameType is Campaign))
                return;

            try
            {
                CaravanLimitPatches.PatchActiveModel(_harmony);
            }
            catch (Exception e)
            {
                ReportProblem("Could not patch caravan party sizes: " + e.Message);
            }

            foreach (string message in PendingProblems)
                InformationManager.DisplayMessage(new InformationMessage(LogPrefix + message, Colors.Red));
            PendingProblems.Clear();
        }

        internal static void ReportProblem(string message)
        {
            Debug.Print(LogPrefix + message);
            PendingProblems.Add(message);
        }
    }
}
