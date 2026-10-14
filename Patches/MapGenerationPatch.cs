using HarmonyLib;
using SpawnOnBeach.Classes;

namespace SpawnOnBeach.Patches;

[HarmonyPatch(typeof(MapGeneration))]
internal static class MapGenerationPatch
{
    [HarmonyPostfix]
    [HarmonyPatch(nameof(MapGeneration.CleanUp))]
    private static void ClearBeachPointsCache()
    {
        BeachPoints.ClearCache();
    }
}
