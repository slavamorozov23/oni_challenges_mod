using System;
using System.Collections.Generic;
using ProcGen;
using STRINGS;
using UnityEngine;

namespace Klei.CustomSettings
{
	// Token: 0x0200101A RID: 4122
	public class SubworldMixingSettingConfig : MixingSettingConfig
	{
		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x06007FEC RID: 32748 RVA: 0x00336D2C File Offset: 0x00334F2C
		public override string label
		{
			get
			{
				SubworldMixingSettings cachedSubworldMixingSetting = SettingsCache.GetCachedSubworldMixingSetting(base.worldgenPath);
				StringEntry entry;
				if (!Strings.TryGet(cachedSubworldMixingSetting.name, out entry))
				{
					return cachedSubworldMixingSetting.name;
				}
				return entry;
			}
		}

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x06007FED RID: 32749 RVA: 0x00336D64 File Offset: 0x00334F64
		public override string tooltip
		{
			get
			{
				SubworldMixingSettings cachedSubworldMixingSetting = SettingsCache.GetCachedSubworldMixingSetting(base.worldgenPath);
				StringEntry entry;
				if (!Strings.TryGet(cachedSubworldMixingSetting.description, out entry))
				{
					return cachedSubworldMixingSetting.description;
				}
				return entry;
			}
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x06007FEE RID: 32750 RVA: 0x00336D9C File Offset: 0x00334F9C
		public override Sprite icon
		{
			get
			{
				SubworldMixingSettings cachedSubworldMixingSetting = SettingsCache.GetCachedSubworldMixingSetting(base.worldgenPath);
				Sprite sprite = (cachedSubworldMixingSetting.icon != null) ? Assets.GetSprite(cachedSubworldMixingSetting.icon) : null;
				if (sprite == null)
				{
					sprite = Assets.GetSprite("unknown");
				}
				return sprite;
			}
		}

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x06007FEF RID: 32751 RVA: 0x00336DEB File Offset: 0x00334FEB
		public override List<string> forbiddenClusterTags
		{
			get
			{
				return SettingsCache.GetCachedSubworldMixingSetting(base.worldgenPath).forbiddenClusterTags;
			}
		}

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x06007FF0 RID: 32752 RVA: 0x00336DFD File Offset: 0x00334FFD
		public override bool isModded
		{
			get
			{
				return SettingsCache.GetCachedSubworldMixingSetting(base.worldgenPath).isModded;
			}
		}

		// Token: 0x06007FF1 RID: 32753 RVA: 0x00336E10 File Offset: 0x00335010
		public SubworldMixingSettingConfig(string id, string worldgenPath, string[] required_content = null, string dlcIdFrom = null, bool triggers_custom_game = true, long coordinate_range = 5L) : base(id, null, null, null, worldgenPath, coordinate_range, false, triggers_custom_game, required_content, "", false)
		{
			this.dlcIdFrom = dlcIdFrom;
			List<SettingLevel> levels = new List<SettingLevel>
			{
				new SettingLevel("Disabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.DISABLED.NAME, DlcManager.FeatureClusterSpaceEnabled() ? UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.DISABLED.TOOLTIP : UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.DISABLED.TOOLTIP_BASEGAME, 0L, null),
				new SettingLevel("TryMixing", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.TRY_MIXING.NAME, DlcManager.FeatureClusterSpaceEnabled() ? UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.TRY_MIXING.TOOLTIP : UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.TRY_MIXING.TOOLTIP_BASEGAME, 1L, null),
				new SettingLevel("GuranteeMixing", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.GUARANTEE_MIXING.NAME, DlcManager.FeatureClusterSpaceEnabled() ? UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.GUARANTEE_MIXING.TOOLTIP : UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.SUBWORLD_MIXING.LEVELS.GUARANTEE_MIXING.TOOLTIP_BASEGAME, 2L, null)
			};
			base.StompLevels(levels, "Disabled", "Disabled");
		}

		// Token: 0x040060F8 RID: 24824
		private const int COORDINATE_RANGE = 5;

		// Token: 0x040060F9 RID: 24825
		public const string DisabledLevelId = "Disabled";

		// Token: 0x040060FA RID: 24826
		public const string TryMixingLevelId = "TryMixing";

		// Token: 0x040060FB RID: 24827
		public const string GuaranteeMixingLevelId = "GuranteeMixing";
	}
}
