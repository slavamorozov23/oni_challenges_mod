using System;
using System.IO;
using HarmonyLib;
using STRINGS;
using UnityEngine;

namespace SlavaMorozov.NoPollutionMod
{
    internal static class ModLocalization
    {
        private static bool _registered;
        private static string _modPath;

        internal static void SetModPath(string path)
        {
            _modPath = path;
        }

        internal static void RegisterStrings(bool generateTemplate = false)
        {
            if (_registered)
            {
                LoadStrings();
                return;
            }

            _registered = true;
            Localization.RegisterForTranslation(typeof(STRINGS));
            LocString.CreateLocStringKeys(typeof(STRINGS), null);

            if (generateTemplate)
            {
                TryGenerateTemplate();
            }

            LoadStrings();
        }

        private static void LoadStrings()
        {
            var code = Localization.GetLocale()?.Code;
            if (string.IsNullOrEmpty(code))
            {
                return;
            }

            if (string.IsNullOrEmpty(_modPath))
            {
                Debug.LogWarning("[NoPollutionMod] ModLocalization: mod path is empty.");
                return;
            }

            var path = Path.Combine(_modPath, "translations", code + ".po");
            if (!File.Exists(path))
            {
                return;
            }

            Localization.OverloadStrings(Localization.LoadStringsFile(path, false));
            Debug.Log($"[NoPollutionMod] Loaded translation file: {path}");
        }

        private static void TryGenerateTemplate()
        {
            try
            {
                var templatesPath = Path.Combine(_modPath, "strings_templates");
                Directory.CreateDirectory(templatesPath);
                Localization.GenerateStringsTemplate(typeof(STRINGS), templatesPath);
            }
            catch (Exception ex)
            {
                Debug.LogWarning($"[NoPollutionMod] Failed to generate translation template: {ex}");
            }
        }

        [HarmonyLib.HarmonyPatch(typeof(Localization), "Initialize")]
        private static class Localization_Initialize_Patch
        {
            private static void Postfix()
            {
                RegisterStrings();
            }
        }
    }
}
