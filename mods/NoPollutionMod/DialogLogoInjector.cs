using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using STRINGS;

namespace SlavaMorozov.NoPollutionMod
{
    internal static class DialogLogoInjector
    {
        internal static IEnumerator ShowWhenReadyRoutine(string title, string body)
        {
            // ждём появления Global + globalCanvas
            for (int i = 0; i < 240; i++) // ~4 секунды при 60fps
            {
                var canvas = Global.Instance?.globalCanvas;
                if (canvas != null)
                {
                    KMod.Manager.Dialog(canvas, title, body, UI.CONFIRMDIALOG.OK.text);
                    Debug.Log("[NoPollutionMod] Dialog shown (canvas ready)");
                    break;
                }
                yield return null;
            }

            // ждём, пока диалог реально появится в сцене
            for (int i = 0; i < 120; i++)
            {
                if (TryInjectLogoBetweenTextAndButton(ModAssets.LogoSprite))
                {
                    Debug.Log("[NoPollutionMod] Logo injected into dialog.");
                    yield break;
                }
                yield return null;
            }

            Debug.LogWarning("[NoPollutionMod] Logo NOT injected (dialog not found / layout changed).");
        }

        private static bool TryInjectLogoBetweenTextAndButton(Sprite logo)
        {
            if (logo == null)
                return false;

            var dlg = UnityEngine.Object.FindObjectsOfType<ConfirmDialogScreen>(true)
                .FirstOrDefault(x => x != null && x.gameObject != null && x.gameObject.activeInHierarchy);

            if (dlg == null)
                return false;

            var allButtons = dlg.gameObject.GetComponentsInChildren<KButton>(true);
            if (allButtons == null || allButtons.Length == 0)
                return false;

            // находим кнопку OK по тексту (fallback: первая кнопка)
            var okButton = allButtons.FirstOrDefault(b =>
            {
                var t = b.GetComponentInChildren<LocText>(true);
                return t != null && string.Equals(t.text?.Trim(), UI.CONFIRMDIALOG.OK.text, StringComparison.OrdinalIgnoreCase);
            }) ?? allButtons.FirstOrDefault();

            if (okButton == null)
                return false;

            var buttonsContainer = okButton.transform.parent;
            if (buttonsContainer == null)
                return false;

            var outer = buttonsContainer.parent != null ? buttonsContainer.parent : buttonsContainer;

            // не дублируем
            if (outer.Find("NoPollutionLogo") != null)
                return true;

            var go = new GameObject("NoPollutionLogo", typeof(RectTransform), typeof(Image), typeof(LayoutElement));
            go.transform.SetParent(outer, false);

            // вставляем картинку прямо перед контейнером кнопок, если можем
            if (outer == buttonsContainer.parent)
                go.transform.SetSiblingIndex(buttonsContainer.GetSiblingIndex());
            else
                go.transform.SetSiblingIndex(okButton.transform.GetSiblingIndex());

            var img = go.GetComponent<Image>();
            img.sprite = logo;
            img.preserveAspect = true;

            var le = go.GetComponent<LayoutElement>();
            le.preferredWidth = 160f;
            le.preferredHeight = 160f;

            return true;
        }
    }
}
