using System;
using System.Collections.Generic;

namespace Klei.CustomSettings
{
	// Token: 0x02001015 RID: 4117
	public class ToggleSettingConfig : SettingConfig
	{
		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x06007FCE RID: 32718 RVA: 0x00336854 File Offset: 0x00334A54
		// (set) Token: 0x06007FCF RID: 32719 RVA: 0x0033685C File Offset: 0x00334A5C
		public SettingLevel on_level { get; private set; }

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x06007FD0 RID: 32720 RVA: 0x00336865 File Offset: 0x00334A65
		// (set) Token: 0x06007FD1 RID: 32721 RVA: 0x0033686D File Offset: 0x00334A6D
		public SettingLevel off_level { get; private set; }

		// Token: 0x06007FD2 RID: 32722 RVA: 0x00336878 File Offset: 0x00334A78
		public ToggleSettingConfig(string id, string label, string tooltip, SettingLevel off_level, SettingLevel on_level, string default_level_id, string nosweat_default_level_id, long coordinate_range = -1L, bool debug_only = false, bool triggers_custom_game = true, string[] required_content = null, string missing_content_default = "") : base(id, label, tooltip, default_level_id, nosweat_default_level_id, coordinate_range, debug_only, triggers_custom_game, required_content, missing_content_default, false)
		{
			this.off_level = off_level;
			this.on_level = on_level;
		}

		// Token: 0x06007FD3 RID: 32723 RVA: 0x003368AD File Offset: 0x00334AAD
		public void StompLevels(SettingLevel off_level, SettingLevel on_level, string default_level_id, string nosweat_default_level_id)
		{
			this.off_level = off_level;
			this.on_level = on_level;
			this.default_level_id = default_level_id;
			this.nosweat_default_level_id = nosweat_default_level_id;
		}

		// Token: 0x06007FD4 RID: 32724 RVA: 0x003368CC File Offset: 0x00334ACC
		public override SettingLevel GetLevel(string level_id)
		{
			if (this.on_level.id == level_id)
			{
				return this.on_level;
			}
			if (this.off_level.id == level_id)
			{
				return this.off_level;
			}
			if (this.default_level_id == this.on_level.id)
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"Unable to find level for setting:",
					base.id,
					"(",
					level_id,
					") Using default level."
				}));
				return this.on_level;
			}
			if (this.default_level_id == this.off_level.id)
			{
				Debug.LogWarning(string.Concat(new string[]
				{
					"Unable to find level for setting:",
					base.id,
					"(",
					level_id,
					") Using default level."
				}));
				return this.off_level;
			}
			Debug.LogError("Unable to find setting level for setting:" + base.id + " level: " + level_id);
			return null;
		}

		// Token: 0x06007FD5 RID: 32725 RVA: 0x003369D1 File Offset: 0x00334BD1
		public override List<SettingLevel> GetLevels()
		{
			return new List<SettingLevel>
			{
				this.off_level,
				this.on_level
			};
		}

		// Token: 0x06007FD6 RID: 32726 RVA: 0x003369F0 File Offset: 0x00334BF0
		public string ToggleSettingLevelID(string current_id)
		{
			if (this.on_level.id == current_id)
			{
				return this.off_level.id;
			}
			return this.on_level.id;
		}

		// Token: 0x06007FD7 RID: 32727 RVA: 0x00336A1C File Offset: 0x00334C1C
		public bool IsOnLevel(string level_id)
		{
			return level_id == this.on_level.id;
		}
	}
}
