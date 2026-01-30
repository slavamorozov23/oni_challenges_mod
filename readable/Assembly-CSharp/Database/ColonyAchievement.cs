using System;
using System.Collections.Generic;
using FMODUnity;
using ProcGen;

namespace Database
{
	// Token: 0x02000FAA RID: 4010
	public class ColonyAchievement : Resource, IHasDlcRestrictions
	{
		// Token: 0x170008C7 RID: 2247
		// (get) Token: 0x06007E23 RID: 32291 RVA: 0x00322663 File Offset: 0x00320863
		// (set) Token: 0x06007E24 RID: 32292 RVA: 0x0032266B File Offset: 0x0032086B
		public EventReference victoryNISSnapshot { get; private set; }

		// Token: 0x06007E25 RID: 32293 RVA: 0x00322674 File Offset: 0x00320874
		public string[] GetRequiredDlcIds()
		{
			return this.requiredDlcIds;
		}

		// Token: 0x06007E26 RID: 32294 RVA: 0x0032267C File Offset: 0x0032087C
		public string[] GetForbiddenDlcIds()
		{
			return this.forbiddenDlcIds;
		}

		// Token: 0x06007E27 RID: 32295 RVA: 0x00322684 File Offset: 0x00320884
		public ColonyAchievement()
		{
			this.Id = "Disabled";
			this.platformAchievementId = "Disabled";
			this.Name = "Disabled";
			this.description = "Disabled";
			this.isVictoryCondition = false;
			this.requirementChecklist = new List<ColonyAchievementRequirement>();
			this.messageTitle = string.Empty;
			this.messageBody = string.Empty;
			this.shortVideoName = string.Empty;
			this.loopVideoName = string.Empty;
			this.platformAchievementId = string.Empty;
			this.icon = string.Empty;
			this.clusterTag = string.Empty;
			this.Disabled = true;
		}

		// Token: 0x06007E28 RID: 32296 RVA: 0x00322734 File Offset: 0x00320934
		public ColonyAchievement(string Id, string platformAchievementId, string Name, string description, bool isVictoryCondition, List<ColonyAchievementRequirement> requirementChecklist, string messageTitle = "", string messageBody = "", string videoDataName = "", string victoryLoopVideo = "", Action<KMonoBehaviour> VictorySequence = null, EventReference victorySnapshot = default(EventReference), string icon = "", string[] requiredDlcIds = null, string[] forbiddenDlcIds = null, string dlcIdFrom = null, string clusterTag = null) : base(Id, Name)
		{
			this.Id = Id;
			this.platformAchievementId = platformAchievementId;
			this.Name = Name;
			this.description = description;
			this.isVictoryCondition = isVictoryCondition;
			this.requirementChecklist = requirementChecklist;
			this.messageTitle = messageTitle;
			this.messageBody = messageBody;
			this.shortVideoName = videoDataName;
			this.loopVideoName = victoryLoopVideo;
			this.victorySequence = VictorySequence;
			this.victoryNISSnapshot = (victorySnapshot.IsNull ? AudioMixerSnapshots.Get().VictoryNISGenericSnapshot : victorySnapshot);
			this.icon = icon;
			this.clusterTag = clusterTag;
			this.requiredDlcIds = requiredDlcIds;
			this.forbiddenDlcIds = forbiddenDlcIds;
			this.dlcIdFrom = dlcIdFrom;
		}

		// Token: 0x06007E29 RID: 32297 RVA: 0x003227F0 File Offset: 0x003209F0
		public bool IsValidForSave()
		{
			if (this.clusterTag.IsNullOrWhiteSpace())
			{
				return true;
			}
			DebugUtil.Assert(CustomGameSettings.Instance != null, "IsValidForSave called when CustomGamesSettings is not initialized.");
			ClusterLayout currentClusterLayout = CustomGameSettings.Instance.GetCurrentClusterLayout();
			return currentClusterLayout != null && currentClusterLayout.clusterTags.Contains(this.clusterTag);
		}

		// Token: 0x04005C93 RID: 23699
		public string description;

		// Token: 0x04005C94 RID: 23700
		public bool isVictoryCondition;

		// Token: 0x04005C95 RID: 23701
		public string messageTitle;

		// Token: 0x04005C96 RID: 23702
		public string messageBody;

		// Token: 0x04005C97 RID: 23703
		public string shortVideoName;

		// Token: 0x04005C98 RID: 23704
		public string loopVideoName;

		// Token: 0x04005C99 RID: 23705
		public string platformAchievementId;

		// Token: 0x04005C9A RID: 23706
		public string icon;

		// Token: 0x04005C9B RID: 23707
		public string clusterTag;

		// Token: 0x04005C9C RID: 23708
		public List<ColonyAchievementRequirement> requirementChecklist = new List<ColonyAchievementRequirement>();

		// Token: 0x04005C9D RID: 23709
		public Action<KMonoBehaviour> victorySequence;

		// Token: 0x04005C9F RID: 23711
		public string[] requiredDlcIds;

		// Token: 0x04005CA0 RID: 23712
		public string[] forbiddenDlcIds;

		// Token: 0x04005CA1 RID: 23713
		public string dlcIdFrom;
	}
}
