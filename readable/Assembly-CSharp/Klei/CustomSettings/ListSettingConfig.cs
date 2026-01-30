using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.CustomSettings
{
	// Token: 0x02001014 RID: 4116
	public class ListSettingConfig : SettingConfig
	{
		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x06007FC5 RID: 32709 RVA: 0x00336647 File Offset: 0x00334847
		// (set) Token: 0x06007FC6 RID: 32710 RVA: 0x0033664F File Offset: 0x0033484F
		public List<SettingLevel> levels { get; private set; }

		// Token: 0x06007FC7 RID: 32711 RVA: 0x00336658 File Offset: 0x00334858
		public ListSettingConfig(string id, string label, string tooltip, List<SettingLevel> levels, string default_level_id, string nosweat_default_level_id, long coordinate_range = -1L, bool debug_only = false, bool triggers_custom_game = true, string[] required_content = null, string missing_content_default = "", bool hide_in_ui = false) : base(id, label, tooltip, default_level_id, nosweat_default_level_id, coordinate_range, debug_only, triggers_custom_game, required_content, missing_content_default, hide_in_ui)
		{
			this.levels = levels;
		}

		// Token: 0x06007FC8 RID: 32712 RVA: 0x00336686 File Offset: 0x00334886
		public void StompLevels(List<SettingLevel> levels, string default_level_id, string nosweat_default_level_id)
		{
			this.levels = levels;
			this.default_level_id = default_level_id;
			this.nosweat_default_level_id = nosweat_default_level_id;
		}

		// Token: 0x06007FC9 RID: 32713 RVA: 0x003366A0 File Offset: 0x003348A0
		public override SettingLevel GetLevel(string level_id)
		{
			for (int i = 0; i < this.levels.Count; i++)
			{
				if (this.levels[i].id == level_id)
				{
					return this.levels[i];
				}
			}
			for (int j = 0; j < this.levels.Count; j++)
			{
				if (this.levels[j].id == this.default_level_id)
				{
					return this.levels[j];
				}
			}
			global::Debug.LogError("Unable to find setting level for setting:" + base.id + " level: " + level_id);
			return null;
		}

		// Token: 0x06007FCA RID: 32714 RVA: 0x00336746 File Offset: 0x00334946
		public override List<SettingLevel> GetLevels()
		{
			return this.levels;
		}

		// Token: 0x06007FCB RID: 32715 RVA: 0x00336750 File Offset: 0x00334950
		public string CycleSettingLevelID(string current_id, int direction)
		{
			string result = "";
			if (current_id == "")
			{
				current_id = this.levels[0].id;
			}
			for (int i = 0; i < this.levels.Count; i++)
			{
				if (this.levels[i].id == current_id)
				{
					int index = Mathf.Clamp(i + direction, 0, this.levels.Count - 1);
					result = this.levels[index].id;
					break;
				}
			}
			return result;
		}

		// Token: 0x06007FCC RID: 32716 RVA: 0x003367E0 File Offset: 0x003349E0
		public bool IsFirstLevel(string level_id)
		{
			return this.levels.FindIndex((SettingLevel l) => l.id == level_id) == 0;
		}

		// Token: 0x06007FCD RID: 32717 RVA: 0x00336814 File Offset: 0x00334A14
		public bool IsLastLevel(string level_id)
		{
			return this.levels.FindIndex((SettingLevel l) => l.id == level_id) == this.levels.Count - 1;
		}
	}
}
