using System.Collections;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace SlavaMorozov.NoPollutionMod
{
    [HarmonyPatch(typeof(MinionSelectScreen), "SetDefaultMinionsRoutine")]
    internal static class MinionSelectScreen_SetDefaultMinionsRoutine_Patch
    {
        private static readonly AccessTools.FieldRef<CharacterSelectionController, List<ITelepadDeliverableContainer>> ContainersRef =
            AccessTools.FieldRefAccess<CharacterSelectionController, List<ITelepadDeliverableContainer>>("containers");

        private static bool Prefix(MinionSelectScreen __instance, ref IEnumerator __result)
        {
            if (__instance == null || !ChallengeGuards.IsOneDuplicantChallengeActive())
            {
                return true;
            }

            var containers = ContainersRef(__instance);
            if (containers == null || containers.Count >= 3)
            {
                return true;
            }

            __result = OneDuplicantDefaultRoutine(__instance);
            return false;
        }

        private static IEnumerator OneDuplicantDefaultRoutine(MinionSelectScreen screen)
        {
            yield return SequenceUtil.WaitForNextFrame;

            var containers = ContainersRef(screen);
            if (containers == null || containers.Count == 0)
            {
                yield break;
            }

            var characterContainer = containers[0] as CharacterContainer;
            if (characterContainer != null)
            {
                characterContainer.GenerateCharacter(true, null);
            }
        }
    }

    [HarmonyPatch(typeof(CharacterSelectionController), "InitializeContainers")]
    internal static class CharacterSelectionController_InitializeContainers_Patch
    {
        private static readonly AccessTools.FieldRef<CharacterSelectionController, List<ITelepadDeliverableContainer>> ContainersRef =
            AccessTools.FieldRefAccess<CharacterSelectionController, List<ITelepadDeliverableContainer>>("containers");

        private static readonly AccessTools.FieldRef<CharacterSelectionController, CarePackageContainer> CarePackagePrefabRef =
            AccessTools.FieldRefAccess<CharacterSelectionController, CarePackageContainer>("carePackageContainerPrefab");

        private static readonly AccessTools.FieldRef<CharacterSelectionController, GameObject> ContainerParentRef =
            AccessTools.FieldRefAccess<CharacterSelectionController, GameObject>("containerParent");

        private static readonly AccessTools.FieldRef<CharacterSelectionController, int> NumberOfCarePackageOptionsRef =
            AccessTools.FieldRefAccess<CharacterSelectionController, int>("numberOfCarePackageOptions");

        private static readonly AccessTools.FieldRef<CharacterSelectionController, int> NumberOfDuplicantOptionsRef =
            AccessTools.FieldRefAccess<CharacterSelectionController, int>("numberOfDuplicantOptions");

        private static void Postfix(CharacterSelectionController __instance)
        {
            if (__instance == null)
            {
                return;
            }

            if (!ChallengeGuards.IsOneDuplicantChallengeActive())
            {
                return;
            }

            var containers = ContainersRef(__instance);
            if (containers == null || containers.Count == 0)
            {
                return;
            }

            if (__instance.IsStarterMinion)
            {
                bool keptOne = false;
                for (int i = containers.Count - 1; i >= 0; i--)
                {
                    if (containers[i] is CharacterContainer characterContainer)
                    {
                        if (keptOne)
                        {
                            Object.Destroy(characterContainer.gameObject);
                            containers.RemoveAt(i);
                        }
                        else
                        {
                            keptOne = true;
                        }
                    }
                }

                NumberOfDuplicantOptionsRef(__instance) = keptOne ? 1 : 0;
                return;
            }

            int removedDuplicants = 0;
            int carePackages = 0;
            for (int i = containers.Count - 1; i >= 0; i--)
            {
                if (containers[i] is CharacterContainer characterContainer)
                {
                    Object.Destroy(characterContainer.gameObject);
                    containers.RemoveAt(i);
                    removedDuplicants++;
                }
                else if (containers[i] is CarePackageContainer)
                {
                    carePackages++;
                }
            }

            var prefab = CarePackagePrefabRef(__instance);
            var parent = ContainerParentRef(__instance);
            if (prefab != null && parent != null)
            {
                for (int i = 0; i < removedDuplicants; i++)
                {
                    var carePackageContainer = Util.KInstantiateUI<CarePackageContainer>(prefab.gameObject, parent, false);
                    carePackageContainer.SetController(__instance);
                    containers.Add(carePackageContainer);
                    carePackageContainer.gameObject.transform.SetSiblingIndex(Random.Range(0, carePackageContainer.transform.parent.childCount));
                }
            }

            NumberOfDuplicantOptionsRef(__instance) = 0;
            NumberOfCarePackageOptionsRef(__instance) = carePackages + removedDuplicants;
        }
    }

    [HarmonyPatch(typeof(CharacterContainer), "SetReshufflingState")]
    internal static class CharacterContainer_SetReshufflingState_Patch
    {
        private static readonly AccessTools.FieldRef<CharacterContainer, CharacterSelectionController> ControllerRef =
            AccessTools.FieldRefAccess<CharacterContainer, CharacterSelectionController>("controller");

        private static readonly AccessTools.FieldRef<CharacterContainer, List<Tag>> PermittedModelsRef =
            AccessTools.FieldRefAccess<CharacterContainer, List<Tag>>("permittedModels");

        private static readonly AccessTools.FieldRef<CharacterContainer, DropDown> ModelDropDownRef =
            AccessTools.FieldRefAccess<CharacterContainer, DropDown>("modelDropDown");

        private static void Postfix(CharacterContainer __instance)
        {
            if (__instance == null || !ChallengeGuards.IsOneDuplicantChallengeActive())
            {
                return;
            }

            var controller = ControllerRef(__instance);
            if (controller == null || !controller.IsStarterMinion)
            {
                return;
            }

            PermittedModelsRef(__instance) = new List<Tag> { GameTags.Minions.Models.Standard };
            var modelDropDown = ModelDropDownRef(__instance);
            if (modelDropDown != null && modelDropDown.transform != null && modelDropDown.transform.parent != null)
            {
                modelDropDown.transform.parent.gameObject.SetActive(false);
            }
        }
    }

    [HarmonyPatch(typeof(CharacterContainer), "GenerateCharacter")]
    internal static class CharacterContainer_GenerateCharacter_Patch
    {
        private static readonly AccessTools.FieldRef<CharacterContainer, CharacterSelectionController> ControllerRef =
            AccessTools.FieldRefAccess<CharacterContainer, CharacterSelectionController>("controller");

        private static readonly AccessTools.FieldRef<CharacterContainer, List<Tag>> PermittedModelsRef =
            AccessTools.FieldRefAccess<CharacterContainer, List<Tag>>("permittedModels");

        private static void Prefix(CharacterContainer __instance, bool is_starter)
        {
            if (__instance == null || !is_starter || !ChallengeGuards.IsOneDuplicantChallengeActive())
            {
                return;
            }

            var controller = ControllerRef(__instance);
            if (controller == null || !controller.IsStarterMinion)
            {
                return;
            }

            PermittedModelsRef(__instance) = new List<Tag> { GameTags.Minions.Models.Standard };
        }
    }

    [HarmonyPatch(typeof(CryoTank), "SidescreenButtonInteractable")]
    internal static class CryoTank_SidescreenButtonInteractable_Patch
    {
        private static void Postfix(ref bool __result)
        {
            if (ChallengeGuards.IsOneDuplicantChallengeActive())
            {
                __result = false;
            }
        }
    }

    [HarmonyPatch(typeof(CryoTank), "OnClickOpen")]
    internal static class CryoTank_OnClickOpen_Patch
    {
        private static bool Prefix()
        {
            return !ChallengeGuards.IsOneDuplicantChallengeActive();
        }
    }
}
