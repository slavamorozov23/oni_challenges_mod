using HarmonyLib;
using UnityEngine;

namespace SlavaMorozov.NoPollutionMod
{
    [HarmonyPatch(typeof(Game), "OnPrefabInit")]
    internal static class BootToastPatch
    {
        private static void Postfix()
        {
            Debug.Log("[NoPollutionMod] Harmony patch OK (Game.OnPrefabInit)");
        }
    }
}
