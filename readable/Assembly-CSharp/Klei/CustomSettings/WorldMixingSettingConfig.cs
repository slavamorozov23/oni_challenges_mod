using System;
using System.Collections.Generic;
using ProcGen;
using STRINGS;
using UnityEngine;

namespace Klei.CustomSettings
{
	// Token: 0x02001019 RID: 4121
	public class WorldMixingSettingConfig : MixingSettingConfig
	{
		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x06007FE6 RID: 32742 RVA: 0x00336B7C File Offset: 0x00334D7C
		public override string label
		{
			get
			{
				WorldMixingSettings cachedWorldMixingSetting = SettingsCache.GetCachedWorldMixingSetting(base.worldgenPath);
				StringEntry entry;
				if (!Strings.TryGet(cachedWorldMixingSetting.name, out entry))
				{
					return cachedWorldMixingSetting.name;
				}
				return entry;
			}
		}

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x06007FE7 RID: 32743 RVA: 0x00336BB4 File Offset: 0x00334DB4
		public override string tooltip
		{
			get
			{
				WorldMixingSettings cachedWorldMixingSetting = SettingsCache.GetCachedWorldMixingSetting(base.worldgenPath);
				StringEntry entry;
				if (!Strings.TryGet(cachedWorldMixingSetting.description, out entry))
				{
					return cachedWorldMixingSetting.description;
				}
				return entry;
			}
		}

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06007FE8 RID: 32744 RVA: 0x00336BEC File Offset: 0x00334DEC
		public override Sprite icon
		{
			get
			{
				WorldMixingSettings cachedWorldMixingSetting = SettingsCache.GetCachedWorldMixingSetting(base.worldgenPath);
				Sprite sprite = (cachedWorldMixingSetting.icon != null) ? ColonyDestinationAsteroidBeltData.GetUISprite(cachedWorldMixingSetting.icon) : null;
				if (sprite == null)
				{
					sprite = Assets.GetSprite(cachedWorldMixingSetting.icon);
				}
				if (sprite == null)
				{
					sprite = Assets.GetSprite("unknown");
				}
				return sprite;
			}
		}

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x06007FE9 RID: 32745 RVA: 0x00336C50 File Offset: 0x00334E50
		public override List<string> forbiddenClusterTags
		{
			get
			{
				return SettingsCache.GetCachedWorldMixingSetting(base.worldgenPath).forbiddenClusterTags;
			}
		}

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x06007FEA RID: 32746 RVA: 0x00336C62 File Offset: 0x00334E62
		public override bool isModded
		{
			get
			{
				return SettingsCache.GetCachedWorldMixingSetting(base.worldgenPath).isModded;
			}
		}

		// Token: 0x06007FEB RID: 32747 RVA: 0x00336C74 File Offset: 0x00334E74
		public WorldMixingSettingConfig(string id, string worldgenPath, string[] required_content = null, string dlcIdFrom = null, bool triggers_custom_game = true, long coordinate_range = 5L) : base(id, null, null, null, worldgenPath, coordinate_range, false, triggers_custom_game, required_content, "", false)
		{
			this.dlcIdFrom = dlcIdFrom;
			List<SettingLevel> levels = new List<SettingLevel>
			{
				new SettingLevel("Disabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.DISABLED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.DISABLED.TOOLTIP, 0L, null),
				new SettingLevel("TryMixing", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.TRY_MIXING.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.TRY_MIXING.TOOLTIP, 1L, null),
				new SettingLevel("GuranteeMixing", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.GUARANTEE_MIXING.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.WORLD_MIXING.LEVELS.GUARANTEE_MIXING.TOOLTIP, 2L, null)
			};
			base.StompLevels(levels, "Disabled", "Disabled");
		}

		// Token: 0x040060F4 RID: 24820
		private const int COORDINATE_RANGE = 5;

		// Token: 0x040060F5 RID: 24821
		public const string DisabledLevelId = "Disabled";

		// Token: 0x040060F6 RID: 24822
		public const string TryMixingLevelId = "TryMixing";

		// Token: 0x040060F7 RID: 24823
		public const string GuaranteeMixingLevelId = "GuranteeMixing";
	}
}
