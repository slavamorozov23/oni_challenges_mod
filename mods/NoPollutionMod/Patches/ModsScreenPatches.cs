using System.Reflection;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace SlavaMorozov.NoPollutionMod
{
    [HarmonyPatch(typeof(ModsScreen), "BuildDisplay")]
    internal static class ModsScreen_BuildDisplay_Patch
    {
        private static void Postfix(ModsScreen __instance)
        {
            // Get displayedMods field through reflection
            var displayedModsField = typeof(ModsScreen).GetField("displayedMods", BindingFlags.NonPublic | BindingFlags.Instance);
            if (displayedModsField == null)
            {
                return;
            }

            var displayedMods = displayedModsField.GetValue(__instance) as System.Collections.IList;
            if (displayedMods == null)
            {
                return;
            }

            // Find our mod in the displayed mods
            for (int i = 0; i < displayedMods.Count; i++)
            {
                var displayedMod = displayedMods[i];
                if (displayedMod == null) continue;

                // Get mod_index from DisplayedMod
                var modIndexField = displayedMod.GetType().GetField("mod_index", BindingFlags.Public | BindingFlags.Instance);
                if (modIndexField == null) continue;

                var modIndex = (int)modIndexField.GetValue(displayedMod);
                if (modIndex >= 0 && modIndex < Global.Instance.modManager.mods.Count)
                {
                    var mod = Global.Instance.modManager.mods[modIndex];
                    var modLabel = mod.label.ToString();

                    if (modLabel == "No Pollution Mod")
                    {
                        // Get rect_transform from DisplayedMod
                        var rectTransformField = displayedMod.GetType().GetField("rect_transform", BindingFlags.Public | BindingFlags.Instance);
                        if (rectTransformField == null) continue;

                        var rectTransform = (RectTransform)rectTransformField.GetValue(displayedMod);
                        if (rectTransform == null) continue;

                        // Obtain references from hierarchy
                        var hierarchyRefs = rectTransform.GetComponent<HierarchyReferences>();
                        if (hierarchyRefs == null) continue;

                        var versionText = hierarchyRefs.GetReference<LocText>("Version");
                        var toggle = hierarchyRefs.GetReference<MultiToggle>("EnabledToggle");
                        var manageButton = hierarchyRefs.GetReference<KButton>("ManageButton");
                        if (versionText == null || toggle == null || manageButton == null)
                        {
                            continue;
                        }

                        var parent = versionText.rectTransform.parent;
                        if (parent == null)
                        {
                            continue;
                        }

                        Transform existing = parent.Find("OptionsButton");
                        GameObject optionsButtonGO;
                        KButton optionsButton;
                        if (existing != null)
                        {
                            optionsButtonGO = existing.gameObject;
                            optionsButton = optionsButtonGO.GetComponent<KButton>();
                        }
                        else
                        {
                            optionsButtonGO = UnityEngine.Object.Instantiate(manageButton.gameObject, parent);
                            optionsButtonGO.name = "OptionsButton";
                            optionsButton = optionsButtonGO.GetComponent<KButton>();
                        }

                        if (optionsButton == null)
                        {
                            continue;
                        }

                        optionsButton.ClearOnClick();
                        optionsButton.onClick += () => ShowModOptionsDialog(toggle, versionText);

                        var manageIndex = manageButton.transform.GetSiblingIndex();
                        var targetIndex = Mathf.Max(0, manageIndex - 1);
                        optionsButtonGO.transform.SetSiblingIndex(targetIndex);

                        var optionsText = optionsButton.GetComponentInChildren<LocText>();
                        if (optionsText != null)
                        {
                            optionsText.SetText("Options");
                        }
                        break;
                    }
                }
            }
        }

        private static void ShowModOptionsDialog(MultiToggle togglePrefab, LocText labelPrefab)
        {
            var canvasRoot = Global.Instance?.globalCanvas;
            if (canvasRoot == null)
            {
                return;
            }

            DebugOptionsScreen.Show(canvasRoot.transform, togglePrefab, labelPrefab);
        }
    }
}
