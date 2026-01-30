using System.Collections;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace SlavaMorozov.NoPollutionMod
{
    [HarmonyPatch(typeof(LoadScreen), "AddColonyToList")]
    internal static class LoadScreen_AddColonyToList_Patch
    {
        private const string ChallengeIconName = "NoPollutionChallengeIcon";
        private static readonly AccessTools.FieldRef<LoadScreen, GameObject> SaveButtonRootRef =
            AccessTools.FieldRefAccess<LoadScreen, GameObject>("saveButtonRoot");

        private static void Postfix(LoadScreen __instance, object saves)
        {
            if (__instance == null || saves == null)
            {
                return;
            }

            var list = saves as IList;
            if (list == null || list.Count == 0)
            {
                return;
            }

            var root = SaveButtonRootRef(__instance);
            if (root == null || root.transform == null || root.transform.childCount == 0)
            {
                return;
            }

            var lastChild = root.transform.GetChild(root.transform.childCount - 1);
            if (lastChild == null)
            {
                return;
            }

            var references = lastChild.GetComponent<HierarchyReferences>();
            if (references == null)
            {
                return;
            }

            var first = list[0];
            if (first == null)
            {
                return;
            }

            var fileInfoField = AccessTools.Field(first.GetType(), "FileInfo");
            if (fileInfoField == null)
            {
                return;
            }

            var fileInfo = (SaveGame.GameInfo)fileInfoField.GetValue(first);
            if (!ChallengeSettings.TryGetChallengeForGameInfo(fileInfo, out var levelId))
            {
                return;
            }

            ModLog.Info($"LoadScreen.AddColonyToList: level={levelId}");

            if (levelId == ChallengeSettings.LevelNone)
            {
                return;
            }

            var dlcIcons = lastChild.transform.Find("Header")?.Find("DlcIcons")?.Find("Prefab_DlcIcon");
            if (dlcIcons == null)
            {
                return;
            }

            var parent = dlcIcons.parent;
            if (parent != null)
            {
                for (int i = 0; i < parent.childCount; i++)
                {
                    if (parent.GetChild(i).name == ChallengeIconName)
                    {
                        return;
                    }
                }
            }

            var sprite = ModAssets.GetChallengeMedalSprite(levelId);
            if (sprite == null)
            {
                return;
            }

            var icon = Util.KInstantiateUI(dlcIcons.gameObject, dlcIcons.parent.gameObject, true);
            icon.name = ChallengeIconName;
            icon.GetComponent<Image>().sprite = sprite;
            icon.GetComponent<ToolTip>().SetSimpleTooltip(ChallengeSettings.GetChallengeName(levelId));
        }
    }
}
