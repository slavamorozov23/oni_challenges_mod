using System;
using STRINGS;

namespace Database
{
	// Token: 0x02000F8F RID: 3983
	public class UpgradeAllBasicBuildings : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007DC7 RID: 32199 RVA: 0x0031F664 File Offset: 0x0031D864
		public UpgradeAllBasicBuildings(Tag basicBuilding, Tag upgradeBuilding)
		{
			this.basicBuilding = basicBuilding;
			this.upgradeBuilding = upgradeBuilding;
		}

		// Token: 0x06007DC8 RID: 32200 RVA: 0x0031F67C File Offset: 0x0031D87C
		public override bool Success()
		{
			bool result = false;
			foreach (IBasicBuilding basicBuilding in Components.BasicBuildings.Items)
			{
				KPrefabID component = basicBuilding.transform.GetComponent<KPrefabID>();
				if (component.HasTag(this.basicBuilding))
				{
					return false;
				}
				if (component.HasTag(this.upgradeBuilding))
				{
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06007DC9 RID: 32201 RVA: 0x0031F700 File Offset: 0x0031D900
		public void Deserialize(IReader reader)
		{
			string name = reader.ReadKleiString();
			this.basicBuilding = new Tag(name);
			string name2 = reader.ReadKleiString();
			this.upgradeBuilding = new Tag(name2);
		}

		// Token: 0x06007DCA RID: 32202 RVA: 0x0031F734 File Offset: 0x0031D934
		public override string GetProgress(bool complete)
		{
			BuildingDef buildingDef = Assets.GetBuildingDef(this.basicBuilding.Name);
			BuildingDef buildingDef2 = Assets.GetBuildingDef(this.upgradeBuilding.Name);
			return string.Format(COLONY_ACHIEVEMENTS.MISC_REQUIREMENTS.STATUS.UPGRADE_ALL_BUILDINGS, buildingDef.Name, buildingDef2.Name);
		}

		// Token: 0x04005C4C RID: 23628
		private Tag basicBuilding;

		// Token: 0x04005C4D RID: 23629
		private Tag upgradeBuilding;
	}
}
