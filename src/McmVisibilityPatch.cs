using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using MCM.Abstractions;
using MCM.Abstractions.Base;
using TaleWorlds.Library;

namespace CaravanPartyLimitIncreasedFixed
{
    // Show only Fixed limit or Clan tier formula settings, matching Clan tier based.
    [HarmonyPatch]
    internal static class McmVisibilityPatch
    {
        static bool Prepare()
        {
            return AccessTools.TypeByName("MCM.Implementation.AttributeSettingsPropertyDiscoverer") != null;
        }

        static MethodBase TargetMethod()
        {
            Type type = AccessTools.TypeByName("MCM.Implementation.AttributeSettingsPropertyDiscoverer");
            if (type == null)
                return null;

            MethodInfo method = AccessTools.Method(type, "GetProperties", new Type[] { typeof(BaseSettings) });
            if (method == null)
                method = AccessTools.Method(type, "GetProperties");
            return method;
        }

        static void Postfix(BaseSettings settings, ref IEnumerable<ISettingsPropertyDefinition> __result)
        {
            try
            {
                if (settings == null || settings.Id == null || settings.Id.IndexOf("CaravanPartyLimitIncreasedFixed") < 0)
                    return;
                if (__result == null)
                    return;

                Settings cpl = settings as Settings;
                if (cpl == null)
                    cpl = Settings.Instance;

                bool tierBased = cpl == null || cpl.ClanTierBased;

                List<ISettingsPropertyDefinition> kept = new List<ISettingsPropertyDefinition>();
                foreach (ISettingsPropertyDefinition definition in __result)
                {
                    if (definition == null)
                        continue;

                    string group = GetGroupName(definition);
                    if (tierBased && group.IndexOf(Settings.FixedLimitGroupMarker) >= 0)
                        continue;
                    if (!tierBased && group.IndexOf(Settings.TierLimitGroupMarker) >= 0)
                        continue;

                    kept.Add(definition);
                }

                __result = kept;
            }
            catch (Exception e)
            {
                Debug.Print("[Caravan Party Limit Increased Fixed] MCM visibility filter skipped: " + e.Message);
            }
        }

        private static string GetGroupName(ISettingsPropertyDefinition definition)
        {
            string groupName = null;
            try
            {
                groupName = definition.GroupName;
            }
            catch
            {
            }

            try
            {
                PropertyInfo raw = definition.GetType().GetProperty(
                    "GroupNameRaw", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                if (raw != null)
                {
                    string rawName = raw.GetValue(definition, null) as string;
                    if (!string.IsNullOrEmpty(rawName))
                    {
                        if (string.IsNullOrEmpty(groupName))
                            groupName = rawName;
                        else
                            groupName = groupName + "|" + rawName;
                    }
                }
            }
            catch
            {
            }

            return groupName ?? string.Empty;
        }

    }
}
