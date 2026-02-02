using System;
using System.Reflection;
using HarmonyLib;
using STRINGS;
using UnityEngine;
using UnityEngine.UI;
using ModStrings = SlavaMorozov.NoPollutionMod.STRINGS;

namespace SlavaMorozov.NoPollutionMod
{
    internal sealed class MainMenuMedalHoverOverlay : KMonoBehaviour
    {
        private const string OverlayRootName = "NoPollutionMedalHoverOverlay";
        private const float OverlayAlpha = 0.75f;
        private static GameObject overlayRoot;
        private static Image overlayBg;
        private static Image overlayImage;
        private static TMPro.TextMeshProUGUI overlayHintText;

        public static void Show(Sprite sprite, string hintText)
        {
            if (sprite == null)
            {
                return;
            }

            EnsureCreated();
            if (overlayRoot == null || overlayImage == null || overlayBg == null)
            {
                return;
            }

            overlayImage.sprite = sprite;
            if (overlayHintText != null)
            {
                overlayHintText.text = hintText ?? string.Empty;
                overlayHintText.gameObject.SetActive(!string.IsNullOrWhiteSpace(hintText));
            }
            overlayRoot.SetActive(true);
            overlayBg.enabled = true;
            overlayImage.enabled = true;
        }

        public static void Hide()
        {
            if (overlayRoot == null)
            {
                return;
            }

            overlayRoot.SetActive(false);
        }

        private static void EnsureCreated()
        {
            if (overlayRoot != null)
            {
                return;
            }

            var canvas = Global.Instance?.globalCanvas;
            if (canvas == null)
            {
                return;
            }

            var existing = canvas.transform.Find(OverlayRootName);
            if (existing != null)
            {
                overlayRoot = existing.gameObject;
                overlayBg = overlayRoot.transform.Find("Bg")?.GetComponent<Image>();
                overlayImage = overlayRoot.transform.Find("CenterImage")?.GetComponent<Image>();
                overlayHintText = overlayRoot.transform.Find("HintText")?.GetComponent<TMPro.TextMeshProUGUI>();
                return;
            }

            overlayRoot = new GameObject(
                OverlayRootName,
                typeof(RectTransform)
            );
            overlayRoot.transform.SetParent(canvas.transform, false);
            overlayRoot.transform.SetAsLastSibling();
            overlayRoot.SetActive(false);

            var rootRect = overlayRoot.GetComponent<RectTransform>();
            rootRect.anchorMin = Vector2.zero;
            rootRect.anchorMax = Vector2.one;
            rootRect.offsetMin = Vector2.zero;
            rootRect.offsetMax = Vector2.zero;

            var bg = new GameObject("Bg", typeof(RectTransform), typeof(Image));
            bg.transform.SetParent(overlayRoot.transform, false);
            bg.transform.SetAsFirstSibling();

            var bgRect = bg.GetComponent<RectTransform>();
            bgRect.anchorMin = Vector2.zero;
            bgRect.anchorMax = Vector2.one;
            bgRect.offsetMin = Vector2.zero;
            bgRect.offsetMax = Vector2.zero;

            overlayBg = bg.GetComponent<Image>();
            overlayBg.color = new Color(0f, 0f, 0f, OverlayAlpha);
            overlayBg.raycastTarget = false;

            var center = new GameObject("CenterImage", typeof(RectTransform), typeof(Image));
            center.transform.SetParent(overlayRoot.transform, false);
            center.transform.SetAsLastSibling();

            var centerRect = center.GetComponent<RectTransform>();
            centerRect.anchorMin = new Vector2(0.5f, 0.5f);
            centerRect.anchorMax = new Vector2(0.5f, 0.5f);
            centerRect.pivot = new Vector2(0.5f, 0.5f);
            centerRect.anchoredPosition = Vector2.zero;
            centerRect.sizeDelta = new Vector2(512f, 512f);

            overlayImage = center.GetComponent<Image>();
            overlayImage.preserveAspect = true;
            overlayImage.raycastTarget = false;
            overlayImage.color = Color.white;

            var hint = new GameObject("HintText", typeof(RectTransform), typeof(TMPro.TextMeshProUGUI));
            hint.transform.SetParent(overlayRoot.transform, false);
            hint.transform.SetAsLastSibling();

            var hintRect = hint.GetComponent<RectTransform>();
            hintRect.anchorMin = new Vector2(0.5f, 0.5f);
            hintRect.anchorMax = new Vector2(0.5f, 0.5f);
            hintRect.pivot = new Vector2(0.5f, 1f);
            hintRect.anchoredPosition = new Vector2(0f, -(512f * 0.5f + 16f));
            hintRect.sizeDelta = new Vector2(900f, 140f);

            overlayHintText = hint.GetComponent<TMPro.TextMeshProUGUI>();
            overlayHintText.text = string.Empty;
            overlayHintText.alignment = TMPro.TextAlignmentOptions.Top;
            overlayHintText.fontSize = 24;
            overlayHintText.color = Color.white;
            overlayHintText.raycastTarget = false;
            hint.SetActive(false);
        }
    }

    internal sealed class MainMenuMedalHoverHandler : KMonoBehaviour, UnityEngine.EventSystems.IPointerEnterHandler, UnityEngine.EventSystems.IPointerExitHandler
    {
        [SerializeField]
        private Image sourceImage;
        private string hintText;

        public void Initialize(Image image, string hint)
        {
            sourceImage = image;
            hintText = hint;
        }

        public void OnPointerEnter(UnityEngine.EventSystems.PointerEventData eventData)
        {
            if (sourceImage == null)
            {
                return;
            }

            MainMenuMedalHoverOverlay.Show(sourceImage.sprite, hintText);
        }

        public void OnPointerExit(UnityEngine.EventSystems.PointerEventData eventData)
        {
            MainMenuMedalHoverOverlay.Hide();
        }
    }

    [HarmonyPatch(typeof(MainMenu), "OnSpawn")]
    internal static class MainMenu_OnSpawn_Patch
    {
        private static readonly AccessTools.FieldRef<MainMenu, GameObject> ButtonParentRef =
            AccessTools.FieldRefAccess<MainMenu, GameObject>("buttonParent");

        private static readonly AccessTools.FieldRef<MainMenu, HierarchyReferences> LogoDLC1Ref =
            AccessTools.FieldRefAccess<MainMenu, HierarchyReferences>("logoDLC1");

        private static readonly AccessTools.FieldRef<MainMenu, HierarchyReferences> LogoDLC2Ref =
            AccessTools.FieldRefAccess<MainMenu, HierarchyReferences>("logoDLC2");

        private static readonly AccessTools.FieldRef<MainMenu, HierarchyReferences> LogoDLC3Ref =
            AccessTools.FieldRefAccess<MainMenu, HierarchyReferences>("logoDLC3");

        private static readonly AccessTools.FieldRef<MainMenu, HierarchyReferences> LogoDLC4Ref =
            AccessTools.FieldRefAccess<MainMenu, HierarchyReferences>("logoDLC4");

        private static readonly AccessTools.FieldRef<MainMenu, ColorStyleSetting> NormalButtonStyleRef =
            AccessTools.FieldRefAccess<MainMenu, ColorStyleSetting>("normalButtonStyle");

        private const string MedalContainerName = "NoPollutionChallengeMedalsMainMenu";
        private const string MedalNamePrefix = "NoPollutionChallengeMedal_";
        private const float MedalSize = 128f;
        private const float MedalOffsetY = 40f;
        private const float MedalOffsetX = 0f;

        private static bool TryGetDlcRowBounds(MainMenu menu, out float rightEdgeX, out float bottomEdgeY)
        {
            rightEdgeX = 0f;
            bottomEdgeY = 0f;

            var logos = new[]
            {
                LogoDLC1Ref(menu)?.GetComponent<RectTransform>(),
                LogoDLC2Ref(menu)?.GetComponent<RectTransform>(),
                LogoDLC3Ref(menu)?.GetComponent<RectTransform>(),
                LogoDLC4Ref(menu)?.GetComponent<RectTransform>()
            };

            var hasAny = false;
            rightEdgeX = float.MinValue;
            bottomEdgeY = float.MaxValue;

            var corners = new Vector3[4];
            for (int i = 0; i < logos.Length; i++)
            {
                var rt = logos[i];
                if (rt == null || !rt.gameObject.activeInHierarchy)
                {
                    continue;
                }

                rt.GetWorldCorners(corners);
                hasAny = true;
                for (int c = 0; c < 4; c++)
                {
                    rightEdgeX = Mathf.Max(rightEdgeX, corners[c].x);
                    bottomEdgeY = Mathf.Min(bottomEdgeY, corners[c].y);
                }
            }

            if (!hasAny)
            {
                rightEdgeX = 0f;
                bottomEdgeY = 0f;
                return false;
            }

            return true;
        }

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

            var logo1 = LogoDLC1Ref(menu);
            var dlcHost = logo1 != null ? logo1.transform.parent : null;
            if (dlcHost == null)
            {
                dlcHost = parent.transform;
            }

            var existing = dlcHost.Find(MedalContainerName);
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
            container.transform.SetParent(dlcHost, false);
            container.transform.SetAsLastSibling();

            var parentRect = dlcHost.GetComponent<RectTransform>();
            var containerRect = container.GetComponent<RectTransform>();

            containerRect.anchorMin = new Vector2(1f, 1f);
            containerRect.anchorMax = new Vector2(1f, 1f);
            containerRect.pivot = new Vector2(1f, 1f);
            containerRect.anchoredPosition = new Vector2(-MedalOffsetX, -MedalOffsetY);

            if (TryGetDlcRowBounds(menu, out var rightEdgeX, out var bottomEdgeY))
            {
                var targetWorld = new Vector3(rightEdgeX - MedalOffsetX, bottomEdgeY - MedalOffsetY, containerRect.position.z);
                containerRect.position = targetWorld;
            }

            var grid = container.GetComponent<GridLayoutGroup>();
            grid.cellSize = new Vector2(MedalSize, MedalSize);
            grid.spacing = new Vector2(8f, 8f);
            grid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
            grid.constraintCount = 1;
            grid.childAlignment = TextAnchor.MiddleRight;
            grid.padding = new RectOffset(0, 0, 0, 0);

            var fitter = container.GetComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            var layout = container.GetComponent<LayoutElement>();
            layout.minHeight = MedalSize + 6f;
            layout.flexibleWidth = 0f;
            layout.ignoreLayout = true;
            layout.preferredWidth = 0f;

            if (DebugOptionsScreen.IsDebugEnabled())
            {
                for (int i = 0; i < 10; i++)
                {
                    CreatePlaceholderMedal(container.transform, menu, i);
                }
            }

            var modules = new System.Collections.Generic.List<IChallengeModule>(ChallengeModuleManager.GetModules());
            for (int i = modules.Count - 1; i >= 0; i--)
            {
                var module = modules[i];
                var completed = ChallengeSettings.IsChallengeCompleted(module.Id);
                ModLog.Info($"MainMenu medals: {module.Id} completed={completed}");
                CreateMedal(container.transform, menu, module.Id, completed);
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
            rect.sizeDelta = new Vector2(MedalSize, MedalSize);

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

            var hover = medal.AddComponent<MainMenuMedalHoverHandler>();
            hover.Initialize(image, string.Empty);

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
            rect.sizeDelta = new Vector2(MedalSize, MedalSize);

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

            var hover = medal.AddComponent<MainMenuMedalHoverHandler>();
            var hintText = ChallengeSettings.GetChallengeTooltip(challengeId);
            hover.Initialize(image, hintText);

            var tooltip = medal.GetComponent<ToolTip>();
            UiHelpers.ConfigureTooltip(tooltip, hintText);

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

            var hover = medalTransform.GetComponent<MainMenuMedalHoverHandler>();
            if (hover == null)
            {
                hover = medalTransform.gameObject.AddComponent<MainMenuMedalHoverHandler>();
            }
            hover.Initialize(image, ChallengeSettings.GetChallengeTooltip(challengeId));

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
