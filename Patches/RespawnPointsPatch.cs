using UnityEngine;
using HarmonyLib;
using SpawnOnBeach.Classes;

namespace SpawnOnBeach.Patches;

[HarmonyPatch(typeof(RespawnPoints))]
internal static class RespawnPointsPatch
{
    [HarmonyPrefix]
    [HarmonyPatch(nameof(RespawnPoints.GetBaseSpawnPoint))]
    private static bool ModifyInitialSpawnPoint(ref Vector3 __result)
    {
        if (!BeachPoints.TryGetRandomBeachPoint(out var beachPoint))
            return true;

        __result = beachPoint;
        return false;
    }

    [HarmonyPrefix]
    [HarmonyPatch(nameof(RespawnPoints.GetRespawnPoint))]
    private static bool ModifyRespawnPoint(ref Vector3 __result)
    {
        if (!BeachPoints.TryGetRandomBeachPoint(out var beachPoint))
            return true;

        __result = beachPoint;
        return false;
    }
}
