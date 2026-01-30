using System;
using STRINGS;

namespace Klei.CustomSettings
{
	// Token: 0x02001017 RID: 4119
	public class DlcMixingSettingConfig : ToggleSettingConfig
	{
		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x06007FDB RID: 32731 RVA: 0x00336A71 File Offset: 0x00334C71
		// (set) Token: 0x06007FDC RID: 32732 RVA: 0x00336A79 File Offset: 0x00334C79
		public virtual string dlcIdFrom { get; protected set; }

		// Token: 0x06007FDD RID: 32733 RVA: 0x00336A84 File Offset: 0x00334C84
		public DlcMixingSettingConfig(string id, string label, string tooltip, long coordinate_range = 5L, bool triggers_custom_game = false, string[] required_content = null, string dlcIdFrom = null, string missing_content_default = "") : base(id, label, tooltip, null, null, null, "Disabled", coordinate_range, false, triggers_custom_game, required_content, missing_content_default)
		{
			this.dlcIdFrom = dlcIdFrom;
			SettingLevel off_level = new SettingLevel("Disabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DLC_MIXING.LEVELS.DISABLED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DLC_MIXING.LEVELS.DISABLED.TOOLTIP, 0L, null);
			SettingLevel on_level = new SettingLevel("Enabled", UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DLC_MIXING.LEVELS.ENABLED.NAME, UI.FRONTEND.CUSTOMGAMESETTINGSSCREEN.SETTINGS.DLC_MIXING.LEVELS.ENABLED.TOOLTIP, 1L, null);
			base.StompLevels(off_level, on_level, "Disabled", "Disabled");
		}

		// Token: 0x040060EB RID: 24811
		private const int COORDINATE_RANGE = 5;

		// Token: 0x040060EC RID: 24812
		public const string DisabledLevelId = "Disabled";

		// Token: 0x040060ED RID: 24813
		public const string EnabledLevelId = "Enabled";
	}
}
