using System;
using HarmonyLib;
using UnityEngine;
using System.Reflection;

namespace SlavaMorozov.NoPollutionMod
{
    public sealed class NoPollutionMod : KMod.UserMod2
    {
        public override void OnLoad(Harmony harmony)
        {
            Debug.Log("[NoPollutionMod] OnLoad ENTER");

            ModLocalization.SetModPath(path);
            ModLocalization.RegisterStrings();

            // Загружаем ассеты (logo.png) из папки мода
            try
            {
                ModAssets.TryLoad(path);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[NoPollutionMod] ModAssets.TryLoad failed: {ex}");
            }

            // Стандартный OnLoad + патчи Harmony
            try
            {
                base.OnLoad(harmony);
            }
            catch (Exception ex)
            {
                Debug.LogError($"[NoPollutionMod] base.OnLoad failed: {ex}");
            }

            try
            {
                harmony.PatchAll(Assembly.GetExecutingAssembly());
                Debug.Log("[NoPollutionMod] PatchAll DONE");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[NoPollutionMod] PatchAll failed: {ex}");
            }
        }
    }
}
