using System;
using System.Collections.Generic;

namespace Klei.CustomSettings
{
	// Token: 0x02001016 RID: 4118
	public class SeedSettingConfig : SettingConfig
	{
		// Token: 0x06007FD8 RID: 32728 RVA: 0x00336A30 File Offset: 0x00334C30
		public SeedSettingConfig(string id, string label, string tooltip, bool debug_only = false, bool triggers_custom_game = true) : base(id, label, tooltip, "", "", -1L, debug_only, triggers_custom_game, null, "", false)
		{
		}

		// Token: 0x06007FD9 RID: 32729 RVA: 0x00336A5D File Offset: 0x00334C5D
		public override SettingLevel GetLevel(string level_id)
		{
			return new SettingLevel(level_id, level_id, level_id, 0L, null);
		}

		// Token: 0x06007FDA RID: 32730 RVA: 0x00336A6A File Offset: 0x00334C6A
		public override List<SettingLevel> GetLevels()
		{
			return new List<SettingLevel>();
		}
	}
}
