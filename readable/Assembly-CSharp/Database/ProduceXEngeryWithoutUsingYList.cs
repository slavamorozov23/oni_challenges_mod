using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02000F84 RID: 3972
	public class ProduceXEngeryWithoutUsingYList : ColonyAchievementRequirement, AchievementRequirementSerialization_Deprecated
	{
		// Token: 0x06007D98 RID: 32152 RVA: 0x0031E8B0 File Offset: 0x0031CAB0
		public ProduceXEngeryWithoutUsingYList(float amountToProduce, List<Tag> disallowedBuildings)
		{
			this.disallowedBuildings = disallowedBuildings;
			this.amountToProduce = amountToProduce;
			this.usedDisallowedBuilding = false;
		}

		// Token: 0x06007D99 RID: 32153 RVA: 0x0031E8D8 File Offset: 0x0031CAD8
		public override bool Success()
		{
			float num = 0f;
			foreach (KeyValuePair<Tag, float> keyValuePair in Game.Instance.savedInfo.powerCreatedbyGeneratorType)
			{
				if (!this.disallowedBuildings.Contains(keyValuePair.Key))
				{
					num += keyValuePair.Value;
				}
			}
			return num / 1000f > this.amountToProduce;
		}

		// Token: 0x06007D9A RID: 32154 RVA: 0x0031E960 File Offset: 0x0031CB60
		public override bool Fail()
		{
			foreach (Tag key in this.disallowedBuildings)
			{
				if (Game.Instance.savedInfo.powerCreatedbyGeneratorType.ContainsKey(key))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007D9B RID: 32155 RVA: 0x0031E9CC File Offset: 0x0031CBCC
		public void Deserialize(IReader reader)
		{
			int num = reader.ReadInt32();
			this.disallowedBuildings = new List<Tag>(num);
			for (int i = 0; i < num; i++)
			{
				string name = reader.ReadKleiString();
				this.disallowedBuildings.Add(new Tag(name));
			}
			this.amountProduced = (float)reader.ReadDouble();
			this.amountToProduce = (float)reader.ReadDouble();
			this.usedDisallowedBuilding = (reader.ReadByte() > 0);
		}

		// Token: 0x06007D9C RID: 32156 RVA: 0x0031EA3C File Offset: 0x0031CC3C
		public float GetProductionAmount(bool complete)
		{
			if (complete)
			{
				return this.amountToProduce * 1000f;
			}
			float num = 0f;
			foreach (KeyValuePair<Tag, float> keyValuePair in Game.Instance.savedInfo.powerCreatedbyGeneratorType)
			{
				if (!this.disallowedBuildings.Contains(keyValuePair.Key))
				{
					num += keyValuePair.Value;
				}
			}
			return num;
		}

		// Token: 0x04005C3D RID: 23613
		public List<Tag> disallowedBuildings = new List<Tag>();

		// Token: 0x04005C3E RID: 23614
		public float amountToProduce;

		// Token: 0x04005C3F RID: 23615
		private float amountProduced;

		// Token: 0x04005C40 RID: 23616
		private bool usedDisallowedBuilding;
	}
}
