using HarmonyLib;
using UnityEngine;
using STRINGS;
using ModStrings = SlavaMorozov.NoPollutionMod.STRINGS;

namespace SlavaMorozov.NoPollutionMod
{
    [HarmonyPatch(typeof(MainMenu), "OnPrefabInit")]
    internal static class MainMenu_OnPrefabInit_Patch
    {
        private static bool _shown;

        private static void Postfix(MainMenu __instance)
        {
            Debug.Log("[NoPollutionMod] MainMenu.OnPrefabInit Postfix FIRED");

            if (_shown) return;
            _shown = true;

            try
            {
                if (__instance != null)
                {
                    __instance.StartCoroutine(
                        DialogLogoInjector.ShowWhenReadyRoutine(
                            ModStrings.UI.MOD_LOADED_TITLE,
                            ModStrings.UI.MOD_LOADED_BODY
                        )
                    );
                }
                else
                {
                    Debug.LogWarning("[NoPollutionMod] MainMenu __instance is null, can't StartCoroutine.");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[NoPollutionMod] MainMenu patch failed: {e}");
            }
        }
    }
}
