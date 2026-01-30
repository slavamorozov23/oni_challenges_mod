using System;
using HarmonyLib;
using UnityEngine;
using ModStrings = SlavaMorozov.NoPollutionMod.STRINGS;

namespace SlavaMorozov.NoPollutionMod
{
    internal sealed class NoToiletsBuildingDisabler : KMonoBehaviour
    {
        private static readonly Operational.Flag NoToiletsFlag =
            new Operational.Flag("no_toilets_challenge", Operational.Flag.Type.Functional);

        private static StatusItem noToiletsStatusItem;

        private Operational operational;
        private KSelectable selectable;

        private Guid statusGuid = Guid.Empty;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            operational = GetComponent<Operational>();
            selectable = GetComponent<KSelectable>();
            EnsureStatusItem();
            UpdateState();
        }

        protected override void OnCleanUp()
        {
            ClearState();
            base.OnCleanUp();
        }

        private void UpdateState()
        {
            bool disable = ChallengeGuards.IsNoToiletsChallengeActive();
            if (operational != null)
            {
                operational.SetFlag(NoToiletsFlag, !disable);
            }

            if (selectable != null && noToiletsStatusItem != null)
            {
                statusGuid = selectable.ToggleStatusItem(noToiletsStatusItem, statusGuid, disable, null);
            }
        }

        private void ClearState()
        {
            if (operational != null)
            {
                operational.SetFlag(NoToiletsFlag, true);
            }

            if (selectable != null && noToiletsStatusItem != null)
            {
                statusGuid = selectable.ToggleStatusItem(noToiletsStatusItem, statusGuid, false, null);
            }
        }

        private static void EnsureStatusItem()
        {
            if (noToiletsStatusItem != null)
            {
                return;
            }

            noToiletsStatusItem = new StatusItem(
                "NoToiletsChallengeOutOfOrder",
                ModStrings.NO_POLLUTION_CHALLENGE.NO_TOILETS_STATUS.NAME,
                ModStrings.NO_POLLUTION_CHALLENGE.NO_TOILETS_STATUS.TOOLTIP,
                "status_item_broken",
                StatusItem.IconType.Custom,
                NotificationType.BadMinor,
                false,
                OverlayModes.None.ID,
                129022,
                true,
                null
            );
        }
    }

    [HarmonyPatch(typeof(BuildingComplete), "OnSpawn")]
    internal static class BuildingComplete_OnSpawn_NoToilets_Patch
    {
        private static void Postfix(BuildingComplete __instance)
        {
            if (__instance == null || !ChallengeGuards.IsNoToiletsChallengeActive())
            {
                return;
            }

            var prefabId = __instance.GetComponent<KPrefabID>();
            if (prefabId == null)
            {
                return;
            }

            bool isToilet = prefabId.HasTag(RoomConstraints.ConstraintTags.ToiletType) ||
                            prefabId.HasTag(RoomConstraints.ConstraintTags.FlushToiletType);

            if (!isToilet)
            {
                var id = prefabId.PrefabID().Name;
                isToilet = NoToiletsChallengeModule.ToiletBuildings.Contains(id);
            }

            if (!isToilet)
            {
                return;
            }

            __instance.gameObject.AddOrGet<NoToiletsBuildingDisabler>();
        }
    }
}
