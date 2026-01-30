using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F72 RID: 3954
	public class CollectedArtifacts : VictoryColonyAchievementRequirement
	{
		// Token: 0x06007D40 RID: 32064 RVA: 0x0031D9D8 File Offset: 0x0031BBD8
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.COLLECT_ARTIFACTS.Replace("{collectedCount}", this.GetStudiedArtifactCount().ToString()).Replace("{neededCount}", 10.ToString());
		}

		// Token: 0x06007D41 RID: 32065 RVA: 0x0031DA1B File Offset: 0x0031BC1B
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x06007D42 RID: 32066 RVA: 0x0031DA29 File Offset: 0x0031BC29
		public override bool Success()
		{
			return ArtifactSelector.Instance.AnalyzedArtifactCount >= 10;
		}

		// Token: 0x06007D43 RID: 32067 RVA: 0x0031DA3C File Offset: 0x0031BC3C
		private int GetStudiedArtifactCount()
		{
			return ArtifactSelector.Instance.AnalyzedArtifactCount;
		}

		// Token: 0x06007D44 RID: 32068 RVA: 0x0031DA48 File Offset: 0x0031BC48
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.STUDY_ARTIFACTS.REQUIREMENTS.STUDY_ARTIFACTS.Replace("{artifactCount}", 10.ToString());
		}

		// Token: 0x04005C2D RID: 23597
		private const int REQUIRED_ARTIFACT_COUNT = 10;
	}
}
