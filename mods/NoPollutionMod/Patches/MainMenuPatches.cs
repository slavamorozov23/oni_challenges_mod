using System;
using System.Reflection;
using HarmonyLib;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;
using ModStrings = SlavaMorozov.NoPollutionMod.STRINGS;

namespace SlavaMorozov.NoPollutionMod
{
    [HarmonyPatch(typeof(MainMenu), "OnSpawn")]
    internal static class MainMenu_OnSpawn_Patch
    {
        private static readonly AccessTools.FieldRef<MainMenu, GameObject> ButtonParentRef =
            AccessTools.FieldRefAccess<MainMenu, GameObject>("buttonParent");

        private static readonly AccessTools.FieldRef<MainMenu, ColorStyleSetting> NormalButtonStyleRef =
            AccessTools.FieldRefAccess<MainMenu, ColorStyleSetting>("normalButtonStyle");

        private const string MedalContainerName = "NoPollutionChallengeMedalsMainMenu";
        private const string MedalNamePrefix = "NoPollutionChallengeMedal_";

        private static void Postfix(MainMenu __instance)
        {
            if (__instance == null)
            {
                return;
            }

            TryInjectMedals(__instance);
        }

        private static void TryInjectMedals(MainMenu menu)
        {
            var parent = ButtonParentRef(menu);
            if (parent == null)
            {
                return;
            }

            var existing = parent.transform.Find(MedalContainerName);
            if (existing != null)
            {
                return;
            }

            var container = new GameObject(
                MedalContainerName,
                typeof(RectTransform),
                typeof(GridLayoutGroup),
                typeof(ContentSizeFitter),
                typeof(LayoutElement)
            );
            container.transform.SetParent(parent.transform, false);
            container.transform.SetAsLastSibling();

            var parentRect = parent.GetComponent<RectTransform>();
            var containerRect = container.GetComponent<RectTransform>();
            containerRect.anchorMin = new Vector2(0f, 0f);
            containerRect.anchorMax = new Vector2(0f, 0f);
            containerRect.pivot = new Vector2(0f, 0.5f);
            containerRect.anchoredPosition = new Vector2(0f, -38f);
            if (parentRect != null)
            {
                containerRect.sizeDelta = new Vector2(parentRect.rect.width, containerRect.sizeDelta.y);
            }

            var grid = container.GetComponent<GridLayoutGroup>();
            grid.cellSize = new Vector2(64f, 64f);
            grid.spacing = new Vector2(8f, 8f);
            grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            grid.constraintCount = 1;
            grid.childAlignment = TextAnchor.MiddleLeft;
            grid.padding = new RectOffset(0, 0, 0, 0);

            var fitter = container.GetComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            var layout = container.GetComponent<LayoutElement>();
            layout.minHeight = 70f;
            layout.flexibleWidth = 0f;
            layout.ignoreLayout = true;
            if (parentRect != null && parentRect.rect.width > 0f)
            {
                layout.preferredWidth = parentRect.rect.width;
            }

            foreach (var module in ChallengeModuleManager.GetModules())
            {
                var completed = ChallengeSettings.IsChallengeCompleted(module.Id);
                ModLog.Info($"MainMenu medals: {module.Id} completed={completed}");
                CreateMedal(container.transform, menu, module.Id, completed);
            }

            if (DebugOptionsScreen.IsDebugEnabled())
            {
                for (int i = 0; i < 10; i++)
                {
                    CreatePlaceholderMedal(container.transform, menu, i);
                }
            }
        }

        private static void CreatePlaceholderMedal(Transform parent, MainMenu menu, int index)
        {
            var medal = new GameObject(
                $"{MedalNamePrefix}Placeholder_{index}",
                typeof(RectTransform)
            );
            medal.transform.SetParent(parent, false);

            var rect = medal.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(64f, 64f);

            var medalImage = new GameObject("MedalImage", typeof(RectTransform), typeof(Image));
            medalImage.transform.SetParent(medal.transform, false);
            var medalRect = medalImage.GetComponent<RectTransform>();
            medalRect.anchorMin = Vector2.zero;
            medalRect.anchorMax = Vector2.one;
            medalRect.offsetMin = Vector2.zero;
            medalRect.offsetMax = Vector2.zero;

            var image = medalImage.GetComponent<Image>();
            image.sprite = ModAssets.GetShadowSprite() ?? ModAssets.LogoSprite;
            image.preserveAspect = true;
            image.color = Color.white;

            var bgImage = new GameObject("ButtonBg", typeof(RectTransform), typeof(KImage));
            bgImage.transform.SetParent(medal.transform, false);
            bgImage.transform.SetAsFirstSibling();
            var bgRect = bgImage.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.offsetMin = Vector2.zero;
            bgRect.offsetMax = Vector2.zero;

            var bgKImage = bgImage.GetComponent<KImage>();
            var style = NormalButtonStyleRef(menu);
            if (style != null)
            {
                bgKImage.colorStyleSetting = style;
                bgKImage.ApplyColorStyleSetting();
            }

            var button = medal.AddComponent<KButton>();
            button.bgImage = bgKImage;
            button.additionalKImages = Array.Empty<KImage>();
            if (button.soundPlayer == null)
            {
                button.soundPlayer = new ButtonSoundPlayer();
            }
        }

        private static void CreateMedal(Transform parent, MainMenu menu, string challengeId, bool completed)
        {
            var medal = new GameObject(
                MedalNamePrefix + challengeId,
                typeof(RectTransform),
                typeof(ToolTip)
            );
            medal.transform.SetParent(parent, false);

            var rect = medal.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(64f, 64f);

            var medalImage = new GameObject("MedalImage", typeof(RectTransform), typeof(Image));
            medalImage.transform.SetParent(medal.transform, false);
            var medalRect = medalImage.GetComponent<RectTransform>();
            medalRect.anchorMin = Vector2.zero;
            medalRect.anchorMax = Vector2.one;
            medalRect.offsetMin = Vector2.zero;
            medalRect.offsetMax = Vector2.zero;

            var image = medalImage.GetComponent<Image>();
            image.sprite = completed
                ? (ModAssets.GetChallengeMedalSprite(challengeId) ?? ModAssets.LogoSprite)
                : (ModAssets.GetShadowSprite() ?? ModAssets.LogoSprite);
            image.preserveAspect = true;
            image.color = Color.white;

            var tooltip = medal.GetComponent<ToolTip>();
            UiHelpers.ConfigureTooltip(tooltip, ChallengeSettings.GetChallengeTooltip(challengeId));

            var bgImage = new GameObject("ButtonBg", typeof(RectTransform), typeof(KImage));
            bgImage.transform.SetParent(medal.transform, false);
            bgImage.transform.SetAsFirstSibling();
            var bgRect = bgImage.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.offsetMin = Vector2.zero;
            bgRect.offsetMax = Vector2.zero;

            var bgKImage = bgImage.GetComponent<KImage>();
            var style = NormalButtonStyleRef(menu);
            if (style != null)
            {
                bgKImage.colorStyleSetting = style;
                bgKImage.ApplyColorStyleSetting();
            }

            var button = medal.AddComponent<KButton>();
            button.bgImage = bgKImage;
            button.additionalKImages = Array.Empty<KImage>();
            if (button.soundPlayer == null)
            {
                button.soundPlayer = new ButtonSoundPlayer();
            }
            if (completed)
            {
                button.onClick += () => ShowClearDialog(menu != null ? menu.transform : null, parent, challengeId);
            }
        }

        private static void ShowClearDialog(Transform parent, Transform container, string challengeId)
        {
            var root = parent ?? Global.Instance?.globalCanvas?.transform;
            if (root == null)
            {
                return;
            }

            ChallengeSettings.RegisterStrings();
            ChallengeSettings.EnsureConfig();

            var prefab = ScreenPrefabs.Instance?.InfoDialogScreen;
            if (prefab == null)
            {
                return;
            }

            var dialog = GameScreenManager.Instance != null
                ? (InfoDialogScreen)GameScreenManager.Instance.StartScreen(prefab.gameObject, root.gameObject)
                : Util.KInstantiateUI<InfoDialogScreen>(prefab.gameObject, root.gameObject, true);
            if (dialog == null)
            {
                return;
            }

            dialog.gameObject.SetActive(true);
            dialog.SetHeader(ModStrings.UI.CLEAR_CHALLENGE_TITLE);
            var challengeName = ChallengeSettings.GetChallengeName(challengeId);
            dialog.AddPlainText(string.Format(ModStrings.UI.CLEAR_CHALLENGE_BODY, challengeName));

            dialog.AddOption(false, out KButton cancelButton, out LocText cancelText);
            cancelText.text = UI.CONFIRMDIALOG.CANCEL.text;
            cancelButton.onClick += dialog.Deactivate;

            dialog.AddOption(true, out KButton okButton, out LocText okText);
            okText.text = UI.CONFIRMDIALOG.OK.text;
            okButton.onClick += () =>
            {
                if (!string.IsNullOrEmpty(challengeId) && challengeId != ChallengeSettings.LevelNone)
                {
                    ChallengeSettings.ClearChallengeCompleted(challengeId);
                    ApplyMedalState(container, challengeId, false);
                }
                dialog.Deactivate();
            };

            SetDialogEscapeCloses(dialog, true);
        }

        private static void ApplyMedalState(Transform container, string challengeId, bool completed)
        {
            if (container == null)
            {
                return;
            }

            var medalTransform = container.Find(MedalNamePrefix + challengeId);
            if (medalTransform == null)
            {
                return;
            }

            var image = medalTransform.Find("MedalImage")?.GetComponent<Image>();
            if (image != null)
            {
                image.sprite = completed
                    ? (ModAssets.GetChallengeMedalSprite(challengeId) ?? ModAssets.LogoSprite)
                    : (ModAssets.GetShadowSprite() ?? ModAssets.LogoSprite);
            }

            var tooltip = medalTransform.GetComponent<ToolTip>();
            UiHelpers.ConfigureTooltip(tooltip, ChallengeSettings.GetChallengeTooltip(challengeId));

            var button = medalTransform.GetComponent<KButton>();
            if (completed)
            {
                if (button == null)
                {
                    button = medalTransform.gameObject.AddComponent<KButton>();
                    button.onClick += () => ShowClearDialog(Global.Instance?.globalCanvas?.transform, container, challengeId);
                }
            }
            else if (button != null)
            {
                UnityEngine.Object.Destroy(button);
            }
        }

        private static void SetDialogEscapeCloses(InfoDialogScreen dialog, bool value)
        {
            if (dialog == null)
            {
                return;
            }

            var field = typeof(InfoDialogScreen).GetField("escapeCloses", BindingFlags.NonPublic | BindingFlags.Instance);
            field?.SetValue(dialog, value);
        }
    }
}
