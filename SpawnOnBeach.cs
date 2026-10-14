using System;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace SpawnOnBeach;

[BepInPlugin(PluginGuid, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class SpawnOnBeach : BaseUnityPlugin
{
    internal const string PluginGuid = "com.theblackvoid.spawnonbeach";
    internal new static ManualLogSource Logger;
    private readonly Harmony _harmony = new(PluginGuid);

    private void Awake()
    {
        Logger = base.Logger;
        try
        {
            _harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
        catch (Exception e)
        {
            Logger.LogError(e);
        }
    }
}
