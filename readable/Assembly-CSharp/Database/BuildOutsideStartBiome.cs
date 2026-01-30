using System;
using Klei;
using ProcGen;
using STRINGS;

namespace Database
{
	// Token: 0x02000F8A RID: 3978
	public class BuildOutsideStartBiome : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007DB3 RID: 32179 RVA: 0x0031EFBC File Offset: 0x0031D1BC
		public override bool Success()
		{
			WorldDetailSave clusterDetailSave = SaveLoader.Instance.clusterDetailSave;
			foreach (BuildingComplete buildingComplete in Components.BuildingCompletes.Items)
			{
				if (!buildingComplete.GetComponent<KPrefabID>().HasTag(GameTags.TemplateBuilding))
				{
					for (int i = 0; i < clusterDetailSave.overworldCells.Count; i++)
					{
						WorldDetailSave.OverworldCell overworldCell = clusterDetailSave.overworldCells[i];
						if (overworldCell.tags != null && !overworldCell.tags.Contains(WorldGenTags.StartWorld) && overworldCell.poly.PointInPolygon(buildingComplete.transform.GetPosition()))
						{
							Game.Instance.unlocks.Unlock("buildoutsidestartingbiome", true);
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06007DB4 RID: 32180 RVA: 0x0031F0AC File Offset: 0x0031D2AC
		public void Deserialize(IReader reader)
		{
		}

		// Token: 0x06007DB5 RID: 32181 RVA: 0x0031F0AE File Offset: 0x0031D2AE
		public override string GetProgress(bool complete)
		{
			return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_OUTSIDE_START;
		}
	}
}
