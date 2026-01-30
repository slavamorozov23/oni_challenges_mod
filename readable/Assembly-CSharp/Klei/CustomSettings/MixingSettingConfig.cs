using System;
using System.Collections.Generic;
using UnityEngine;

namespace Klei.CustomSettings
{
	// Token: 0x02001018 RID: 4120
	public class MixingSettingConfig : ListSettingConfig
	{
		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06007FDE RID: 32734 RVA: 0x00336B09 File Offset: 0x00334D09
		// (set) Token: 0x06007FDF RID: 32735 RVA: 0x00336B11 File Offset: 0x00334D11
		public string worldgenPath { get; private set; }

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06007FE0 RID: 32736 RVA: 0x00336B1A File Offset: 0x00334D1A
		public virtual Sprite icon { get; }

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x06007FE1 RID: 32737 RVA: 0x00336B22 File Offset: 0x00334D22
		public virtual List<string> forbiddenClusterTags { get; }

		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x06007FE2 RID: 32738 RVA: 0x00336B2A File Offset: 0x00334D2A
		// (set) Token: 0x06007FE3 RID: 32739 RVA: 0x00336B32 File Offset: 0x00334D32
		public virtual string dlcIdFrom { get; protected set; }

		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x06007FE4 RID: 32740 RVA: 0x00336B3B File Offset: 0x00334D3B
		public virtual bool isModded { get; }

		// Token: 0x06007FE5 RID: 32741 RVA: 0x00336B44 File Offset: 0x00334D44
		protected MixingSettingConfig(string id, List<SettingLevel> levels, string default_level_id, string nosweat_default_level_id, string worldgenPath, long coordinate_range = -1L, bool debug_only = false, bool triggers_custom_game = false, string[] required_content = null, string missing_content_default = "", bool hide_in_ui = false) : base(id, "", "", levels, default_level_id, nosweat_default_level_id, coordinate_range, debug_only, triggers_custom_game, required_content, missing_content_default, hide_in_ui)
		{
			this.worldgenPath = worldgenPath;
		}
	}
}
