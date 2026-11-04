using System;
using System.Reflection;

namespace CaravanPartyLimitIncreasedFixed
{
    // Soft link to Clan Tier Expanded's MCM hard cap (lives in that mod's menu).
    internal static class ClanTierCaravanCap
    {
        private static bool _resolved;
        private static MethodInfo _getCap;

        internal static int TryGetCap()
        {
            EnsureResolved();
            if (_getCap == null)
                return 0;

            try
            {
                Type settingsType = _getCap.DeclaringType;
                PropertyInfo instance = settingsType != null
                    ? settingsType.GetProperty("Instance", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
                    : null;
                object settings = instance != null ? instance.GetValue(null, null) : null;
                if (settings == null)
                    return 0;

                object value = _getCap.Invoke(settings, null);
                if (value is int)
                {
                    int cap = (int)value;
                    return cap > 0 ? cap : 0;
                }
            }
            catch
            {
            }

            return 0;
        }

        private static void EnsureResolved()
        {
            if (_resolved)
                return;
            _resolved = true;

            try
            {
                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    Type type = assembly.GetType("ClanTierExpanded.Settings", false);
                    if (type == null)
                        type = assembly.GetType("CustomizableClanTier.Settings", false);
                    if (type == null)
                        continue;

                    MethodInfo method = type.GetMethod("GetCaravanPartySizeCap", BindingFlags.Instance | BindingFlags.Public);
                    if (method != null && method.ReturnType == typeof(int) && method.GetParameters().Length == 0)
                    {
                        _getCap = method;
                        return;
                    }
                }
            }
            catch
            {
            }
        }
    }
}
