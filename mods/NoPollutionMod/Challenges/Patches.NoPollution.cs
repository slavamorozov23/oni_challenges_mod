using System;
using HarmonyLib;
using Klei.AI;
using STRINGS;
using UnityEngine;
using ModStrings = SlavaMorozov.NoPollutionMod.STRINGS;

namespace SlavaMorozov.NoPollutionMod
{
    [HarmonyPatch(typeof(MinionIdentity), "OnSpawn")]
    internal static class MinionIdentity_OnSpawn_Patch
    {
        private static void Postfix(MinionIdentity __instance)
        {
            if (__instance == null)
            {
                return;
            }

            __instance.gameObject.AddOrGet<NoPollutionMinionDebuffTracker>();
        }
    }

    internal sealed class NoPollutionMinionDebuffTracker : KMonoBehaviour, ISim1000ms
    {
        private Effects _effects;
        private bool _hasDebuff;
        private string _effectId;

        protected override void OnSpawn()
        {
            base.OnSpawn();
            _effects = GetComponent<Effects>();
        }

        public void Sim1000ms(float dt)
        {
            if (_effects == null)
            {
                return;
            }

            if (!ChallengeSettings.IsChallengeActive())
            {
                RemoveDebuff();
                return;
            }

            var module = ChallengeModuleManager.GetActiveModule();
            if (module == null || string.IsNullOrEmpty(module.RoomDebuffEffectId))
            {
                RemoveDebuff();
                return;
            }

            module.EnsureEffects();
            _effectId = module.RoomDebuffEffectId;

            var room = GetCurrentRoom();
            if (room == null || room.buildings == null)
            {
                RemoveDebuff();
                return;
            }

            if (module.ShouldApplyRoomDebuff(room))
            {
                ApplyDebuff();
                return;
            }

            RemoveDebuff();
        }

        private Room GetCurrentRoom()
        {
            if (Game.Instance == null || Game.Instance.roomProber == null)
            {
                return null;
            }

            var cell = Grid.PosToCell(gameObject);
            var cavity = Game.Instance.roomProber.GetCavityForCell(cell);
            return cavity != null ? cavity.room : null;
        }

        private void ApplyDebuff()
        {
            if (_hasDebuff)
            {
                return;
            }

            if (string.IsNullOrEmpty(_effectId))
            {
                return;
            }

            _effects.Add(_effectId, true);
            _hasDebuff = true;
            ModLog.Info("Polluted surroundings debuff applied");
        }

        private void RemoveDebuff()
        {
            if (!_hasDebuff)
            {
                return;
            }

            if (!string.IsNullOrEmpty(_effectId))
            {
                _effects.Remove(_effectId);
            }
            _hasDebuff = false;
            ModLog.Info("Polluted surroundings debuff removed");
        }
    }

    [HarmonyPatch(typeof(SelectModuleSideScreen), "TestBuildable")]
    internal static class SelectModuleSideScreen_TestBuildable_Patch
    {
        private static void Postfix(BuildingDef def, ref bool __result)
        {
            if (!__result)
            {
                return;
            }

            if (!ChallengeSettings.IsChallengeActive())
            {
                return;
            }

            if (ChallengeSettings.IsBlockedBuilding(def))
            {
                ModLog.Info($"SelectModuleSideScreen.TestBuildable: blocked rocket module {def.PrefabID}");
                __result = false;
            }
        }
    }

    [HarmonyPatch(typeof(BuildTool), "TryBuild")]
    internal static class BuildTool_TryBuild_Patch
    {
        private static readonly AccessTools.FieldRef<BuildTool, BuildingDef> DefRef =
            AccessTools.FieldRefAccess<BuildTool, BuildingDef>("def");

        private static float _lastDialogTime;

        private static bool Prefix(BuildTool __instance)
        {
            if (!ChallengeSettings.IsChallengeActive())
            {
                ModLog.Info("BuildTool.TryBuild: challenge inactive");
                return true;
            }

            var def = DefRef(__instance);
            if (!ChallengeSettings.IsBlockedBuilding(def))
            {
                ModLog.Info($"BuildTool.TryBuild: allowed {(def != null ? def.PrefabID : "<null>")}");
                return true;
            }

            ModLog.Info($"BuildTool.TryBuild: blocked {(def != null ? def.PrefabID : "<null>")}");

            var now = Time.unscaledTime;
            if (now - _lastDialogTime > 0.5f)
            {
                _lastDialogTime = now;
                ShowBlockedDialog();
            }

            return false;
        }

        private static void ShowBlockedDialog()
        {
            var canvas = Global.Instance?.globalCanvas;
            if (canvas == null)
            {
                return;
            }

            KMod.Manager.Dialog(
                canvas,
                ModStrings.UI.BUILDING_BLOCKED_TITLE,
                string.Format(
                    ModStrings.UI.BUILDING_BLOCKED_BODY,
                    ChallengeSettings.GetChallengeName(ChallengeSettings.GetCurrentChallengeLevelId())
                ),
                UI.CONFIRMDIALOG.OK.text
            );
        }
    }
}
