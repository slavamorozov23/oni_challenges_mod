using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000F86 RID: 3974
	public class NoFarmables : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007DA1 RID: 32161 RVA: 0x0031EB20 File Offset: 0x0031CD20
		public override bool Success()
		{
			foreach (WorldContainer worldContainer in ClusterManager.Instance.WorldContainers)
			{
				foreach (PlantablePlot plantablePlot in Components.PlantablePlots.GetItems(worldContainer.id))
				{
					if (plantablePlot.Occupant != null)
					{
						using (IEnumerator<Tag> enumerator3 = plantablePlot.possibleDepositObjectTags.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								if (enumerator3.Current != GameTags.DecorSeed)
								{
									return false;
								}
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x06007DA2 RID: 32162 RVA: 0x0031EC18 File Offset: 0x0031CE18
		public override bool Fail()
		{
			return !this.Success();
		}

		// Token: 0x06007DA3 RID: 32163 RVA: 0x0031EC23 File Offset: 0x0031CE23
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x06007DA4 RID: 32164 RVA: 0x0031EC25 File Offset: 0x0031CE25
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.NO_FARM_TILES;
		}
	}
}
