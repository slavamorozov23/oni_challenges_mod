using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F73 RID: 3955
	public class CollectedSpaceArtifacts : VictoryColonyAchievementRequirement
	{
		// Token: 0x06007D46 RID: 32070 RVA: 0x0031DA7C File Offset: 0x0031BC7C
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.COLLECT_SPACE_ARTIFACTS.Replace("{collectedCount}", this.GetStudiedSpaceArtifactCount().ToString()).Replace("{neededCount}", 10.ToString());
		}

		// Token: 0x06007D47 RID: 32071 RVA: 0x0031DABF File Offset: 0x0031BCBF
		public override string Description()
		{
			return this.GetProgress(this.Success());
		}

		// Token: 0x06007D48 RID: 32072 RVA: 0x0031DACD File Offset: 0x0031BCCD
		public override bool Success()
		{
			return ArtifactSelector.Instance.AnalyzedSpaceArtifactCount >= 10;
		}

		// Token: 0x06007D49 RID: 32073 RVA: 0x0031DAE0 File Offset: 0x0031BCE0
		private int GetStudiedSpaceArtifactCount()
		{
			return ArtifactSelector.Instance.AnalyzedSpaceArtifactCount;
		}

		// Token: 0x06007D4A RID: 32074 RVA: 0x0031DAEC File Offset: 0x0031BCEC
		public override string Name()
		{
			return COLONY_ACHIEVEMENTS.STUDY_ARTIFACTS.REQUIREMENTS.STUDY_SPACE_ARTIFACTS.Replace("{artifactCount}", 10.ToString());
		}

		// Token: 0x04005C2E RID: 23598
		private const int REQUIRED_ARTIFACT_COUNT = 10;
	}
}
