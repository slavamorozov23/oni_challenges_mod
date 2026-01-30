using System;
using System.Collections.Generic;
using STRINGS;

namespace Database
{
	// Token: 0x02000F8E RID: 3982
	public class AtLeastOneBuildingForEachDupe : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007DC2 RID: 32194 RVA: 0x0031F458 File Offset: 0x0031D658
		public AtLeastOneBuildingForEachDupe(List<Tag> validBuildingTypes)
		{
			this.validBuildingTypes = validBuildingTypes;
		}

		// Token: 0x06007DC3 RID: 32195 RVA: 0x0031F474 File Offset: 0x0031D674
		public override bool Success()
		{
			if (Components.LiveMinionIdentities.Items.Count <= 0)
			{
				return false;
			}
			int num = 0;
			foreach (IBasicBuilding basicBuilding in Components.BasicBuildings.Items)
			{
				Tag prefabTag = basicBuilding.transform.GetComponent<KPrefabID>().PrefabTag;
				if (this.validBuildingTypes.Contains(prefabTag))
				{
					num++;
					if (prefabTag == "FlushToilet" || prefabTag == "Outhouse")
					{
						return true;
					}
				}
			}
			return num >= Components.LiveMinionIdentities.Items.Count;
		}

		// Token: 0x06007DC4 RID: 32196 RVA: 0x0031F53C File Offset: 0x0031D73C
		public override bool Fail()
		{
			return false;
		}

		// Token: 0x06007DC5 RID: 32197 RVA: 0x0031F540 File Offset: 0x0031D740
		public void Deserialize(IReader reader)
		{
			int num = reader.ReadInt32();
			this.validBuildingTypes = new List<Tag>(num);
			for (int i = 0; i < num; i++)
			{
				string name = reader.ReadKleiString();
				this.validBuildingTypes.Add(new Tag(name));
			}
		}

		// Token: 0x06007DC6 RID: 32198 RVA: 0x0031F584 File Offset: 0x0031D784
		public override string GetProgress(bool complete)
		{
			if (this.validBuildingTypes.Contains("FlushToilet"))
			{
				return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_ONE_TOILET;
			}
			if (complete)
			{
				return COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILT_ONE_BED_PER_DUPLICANT;
			}
			int num = 0;
			foreach (IBasicBuilding basicBuilding in Components.BasicBuildings.Items)
			{
				KPrefabID component = basicBuilding.transform.GetComponent<KPrefabID>();
				if (this.validBuildingTypes.Contains(component.PrefabTag))
				{
					num++;
				}
			}
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.BUILING_BEDS, complete ? Components.LiveMinionIdentities.Items.Count : num, Components.LiveMinionIdentities.Items.Count);
		}

		// Token: 0x04005C4B RID: 23627
		private List<Tag> validBuildingTypes = new List<Tag>();
	}
}
