using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F91 RID: 3985
	public class CreateMasterPainting : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007DCF RID: 32207 RVA: 0x0031F940 File Offset: 0x0031DB40
		public override bool Success()
		{
			foreach (Painting painting in Components.Paintings.Items)
			{
				if (painting != null)
				{
					ArtableStage artableStage = Db.GetArtableStages().TryGet(painting.CurrentStage);
					if (artableStage != null && artableStage.statusItem == Db.Get().ArtableStatuses.LookingGreat)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06007DD0 RID: 32208 RVA: 0x0031F9CC File Offset: 0x0031DBCC
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x06007DD1 RID: 32209 RVA: 0x0031F9CE File Offset: 0x0031DBCE
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.CREATE_A_PAINTING;
		}
	}
}
