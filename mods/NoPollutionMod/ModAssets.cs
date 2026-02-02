using System.IO;
using UnityEngine;

namespace SlavaMorozov.NoPollutionMod
{
    internal static class ModAssets
    {
        private static string ModRootPath;
        private static readonly System.Collections.Generic.Dictionary<string, Sprite> ChallengeMedalSprites =
            new System.Collections.Generic.Dictionary<string, Sprite>();

        internal static Sprite LogoSprite { get; private set; }
        internal static Sprite ShadowSprite { get; private set; }
        internal static Sprite DefaultMedalSprite { get; private set; }
        internal static Sprite NoPollutionMedalSprite { get; private set; }
        internal static Sprite PlaceholderSprite { get; private set; }

        internal static void TryLoad(string modRootPath)
        {
            if (string.IsNullOrWhiteSpace(modRootPath))
            {
                Debug.LogWarning("[NoPollutionMod] Mod root path is empty. Skipping asset load.");
                return;
            }

            ModRootPath = modRootPath;
            ChallengeMedalSprites.Clear();

            LogoSprite = TryLoadSprite(modRootPath, "logo.png");
            ShadowSprite = TryLoadSprite(modRootPath, "shadow.png");
            DefaultMedalSprite = TryLoadSprite(modRootPath, "medal_default.png");
            NoPollutionMedalSprite = TryLoadSprite(modRootPath, "medal_NoPollution.png");
            PlaceholderSprite = TryLoadSprite(modRootPath, "placeholder.png");

            if (DefaultMedalSprite == null)
            {
                DefaultMedalSprite = LogoSprite;
            }

            if (NoPollutionMedalSprite == null)
            {
                NoPollutionMedalSprite = DefaultMedalSprite;
            }

            if (ShadowSprite == null)
            {
                ShadowSprite = LogoSprite;
            }

            if (PlaceholderSprite == null)
            {
                PlaceholderSprite = ShadowSprite ?? LogoSprite;
            }
        }

        internal static Sprite GetChallengeMedalSprite(string challengeId)
        {
            if (!string.IsNullOrWhiteSpace(challengeId))
            {
                if (ChallengeMedalSprites.TryGetValue(challengeId, out var cached) && cached != null)
                {
                    return cached;
                }

                if (!string.IsNullOrWhiteSpace(ModRootPath))
                {
                    var fileName = $"medal_{challengeId}.png";
                    var sprite = TryLoadSprite(ModRootPath, fileName);
                    if (sprite != null)
                    {
                        ChallengeMedalSprites[challengeId] = sprite;
                        return sprite;
                    }

                    ChallengeMedalSprites[challengeId] = null;
                }
            }

            if (challengeId == ChallengeSettings.LevelNoPollution && NoPollutionMedalSprite != null)
            {
                return NoPollutionMedalSprite;
            }

            if (DefaultMedalSprite != null)
            {
                return DefaultMedalSprite;
            }

            return LogoSprite ?? ShadowSprite;
        }

        internal static Sprite GetShadowSprite()
        {
            return ShadowSprite ?? DefaultMedalSprite ?? LogoSprite;
        }

        private static Sprite TryLoadSprite(string modRootPath, string fileName)
        {
            try
            {
                var filePath = Path.Combine(modRootPath, "assets", fileName);

                if (!File.Exists(filePath))
                {
                    Debug.LogWarning($"[NoPollutionMod] {fileName} not found: {filePath}");
                    return null;
                }

                var bytes = File.ReadAllBytes(filePath);
                var tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);

                if (!UnityEngine.ImageConversion.LoadImage(tex, bytes))
                {
                    Debug.LogError($"[NoPollutionMod] Failed to LoadImage(): {filePath}");
                    return null;
                }

                tex.wrapMode = TextureWrapMode.Clamp;
                tex.filterMode = FilterMode.Bilinear;

                Debug.Log($"[NoPollutionMod] {fileName} loaded: {tex.width}x{tex.height}");

                return Sprite.Create(
                    tex,
                    new Rect(0, 0, tex.width, tex.height),
                    new Vector2(0.5f, 0.5f),
                    100f
                );
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[NoPollutionMod] Failed to load {fileName}: {ex}");
                return null;
            }
        }
    }
}
