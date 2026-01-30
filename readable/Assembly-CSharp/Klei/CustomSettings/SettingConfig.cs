using System;
using System.Collections.Generic;

namespace Klei.CustomSettings
{
	// Token: 0x02001013 RID: 4115
	public abstract class SettingConfig : IHasDlcRestrictions
	{
		// Token: 0x06007FAA RID: 32682 RVA: 0x003364A8 File Offset: 0x003346A8
		public SettingConfig(string id, string label, string tooltip, string default_level_id, string nosweat_default_level_id, long coordinate_range = -1L, bool debug_only = false, bool triggers_custom_game = true, string[] required_content = null, string missing_content_default = "", bool hide_in_ui = false)
		{
			this.id = id;
			this.label = label;
			this.tooltip = tooltip;
			this.default_level_id = default_level_id;
			this.nosweat_default_level_id = nosweat_default_level_id;
			this.coordinate_range = coordinate_range;
			this.debug_only = debug_only;
			this.triggers_custom_game = triggers_custom_game;
			this.required_content = required_content;
			this.missing_content_default = missing_content_default;
			this.hide_in_ui = hide_in_ui;
		}

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x06007FAB RID: 32683 RVA: 0x00336510 File Offset: 0x00334710
		// (set) Token: 0x06007FAC RID: 32684 RVA: 0x00336518 File Offset: 0x00334718
		public string id { get; private set; }

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x06007FAD RID: 32685 RVA: 0x00336521 File Offset: 0x00334721
		// (set) Token: 0x06007FAE RID: 32686 RVA: 0x00336529 File Offset: 0x00334729
		public virtual string label { get; private set; }

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x06007FAF RID: 32687 RVA: 0x00336532 File Offset: 0x00334732
		// (set) Token: 0x06007FB0 RID: 32688 RVA: 0x0033653A File Offset: 0x0033473A
		public virtual string tooltip { get; private set; }

		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06007FB1 RID: 32689 RVA: 0x00336543 File Offset: 0x00334743
		// (set) Token: 0x06007FB2 RID: 32690 RVA: 0x0033654B File Offset: 0x0033474B
		public long coordinate_range { get; protected set; }

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x06007FB3 RID: 32691 RVA: 0x00336554 File Offset: 0x00334754
		// (set) Token: 0x06007FB4 RID: 32692 RVA: 0x0033655C File Offset: 0x0033475C
		public string[] required_content { get; private set; }

		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x06007FB5 RID: 32693 RVA: 0x00336565 File Offset: 0x00334765
		// (set) Token: 0x06007FB6 RID: 32694 RVA: 0x0033656D File Offset: 0x0033476D
		public string missing_content_default { get; private set; }

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06007FB7 RID: 32695 RVA: 0x00336576 File Offset: 0x00334776
		// (set) Token: 0x06007FB8 RID: 32696 RVA: 0x0033657E File Offset: 0x0033477E
		public bool triggers_custom_game { get; protected set; }

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x06007FB9 RID: 32697 RVA: 0x00336587 File Offset: 0x00334787
		// (set) Token: 0x06007FBA RID: 32698 RVA: 0x0033658F File Offset: 0x0033478F
		public bool debug_only { get; protected set; }

		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06007FBB RID: 32699 RVA: 0x00336598 File Offset: 0x00334798
		// (set) Token: 0x06007FBC RID: 32700 RVA: 0x003365A0 File Offset: 0x003347A0
		public bool hide_in_ui { get; protected set; }

		// Token: 0x06007FBD RID: 32701
		public abstract SettingLevel GetLevel(string level_id);

		// Token: 0x06007FBE RID: 32702
		public abstract List<SettingLevel> GetLevels();

		// Token: 0x06007FBF RID: 32703 RVA: 0x003365A9 File Offset: 0x003347A9
		public bool IsDefaultLevel(string level_id)
		{
			return level_id == this.default_level_id;
		}

		// Token: 0x06007FC0 RID: 32704 RVA: 0x003365B7 File Offset: 0x003347B7
		public bool ShowInUI()
		{
			return !this.deprecated && !this.hide_in_ui && (!this.debug_only || DebugHandler.enabled) && DlcManager.IsAllContentSubscribed(this.required_content);
		}

		// Token: 0x06007FC1 RID: 32705 RVA: 0x003365EA File Offset: 0x003347EA
		public string GetDefaultLevelId()
		{
			if (!DlcManager.IsAllContentSubscribed(this.required_content) && !string.IsNullOrEmpty(this.missing_content_default))
			{
				return this.missing_content_default;
			}
			return this.default_level_id;
		}

		// Token: 0x06007FC2 RID: 32706 RVA: 0x00336613 File Offset: 0x00334813
		public string GetNoSweatDefaultLevelId()
		{
			if (!DlcManager.IsAllContentSubscribed(this.required_content) && !string.IsNullOrEmpty(this.missing_content_default))
			{
				return this.missing_content_default;
			}
			return this.nosweat_default_level_id;
		}

		// Token: 0x06007FC3 RID: 32707 RVA: 0x0033663C File Offset: 0x0033483C
		public string[] GetRequiredDlcIds()
		{
			return this.required_content;
		}

		// Token: 0x06007FC4 RID: 32708 RVA: 0x00336644 File Offset: 0x00334844
		public string[] GetForbiddenDlcIds()
		{
			return null;
		}

		// Token: 0x040060DF RID: 24799
		protected string default_level_id;

		// Token: 0x040060E0 RID: 24800
		protected string nosweat_default_level_id;

		// Token: 0x040060E7 RID: 24807
		public bool deprecated;
	}
}
