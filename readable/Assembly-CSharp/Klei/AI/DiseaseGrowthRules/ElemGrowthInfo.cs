using System;
using System.Collections.Generic;
using System.IO;

namespace Klei.AI.DiseaseGrowthRules
{
	// Token: 0x0200105C RID: 4188
	public struct ElemGrowthInfo
	{
		// Token: 0x060081C9 RID: 33225 RVA: 0x00340C1C File Offset: 0x0033EE1C
		public void Write(BinaryWriter writer)
		{
			writer.Write(this.underPopulationDeathRate);
			writer.Write(this.populationHalfLife);
			writer.Write(this.overPopulationHalfLife);
			writer.Write(this.diffusionScale);
			writer.Write(this.minCountPerKG);
			writer.Write(this.maxCountPerKG);
			writer.Write(this.minDiffusionCount);
			writer.Write(this.minDiffusionInfestationTickCount);
		}

		// Token: 0x060081CA RID: 33226 RVA: 0x00340C8C File Offset: 0x0033EE8C
		public static void SetBulk(ElemGrowthInfo[] info, Func<Element, bool> test, ElemGrowthInfo settings)
		{
			List<Element> elements = ElementLoader.elements;
			for (int i = 0; i < elements.Count; i++)
			{
				if (test(elements[i]))
				{
					info[i] = settings;
				}
			}
		}

		// Token: 0x060081CB RID: 33227 RVA: 0x00340CC8 File Offset: 0x0033EEC8
		public float CalculateDiseaseCountDelta(int disease_count, float kg, float dt)
		{
			float num = this.minCountPerKG * kg;
			float num2 = this.maxCountPerKG * kg;
			float result;
			if (num <= (float)disease_count && (float)disease_count <= num2)
			{
				result = (Disease.HalfLifeToGrowthRate(this.populationHalfLife, dt) - 1f) * (float)disease_count;
			}
			else if ((float)disease_count < num)
			{
				result = -this.underPopulationDeathRate * dt;
			}
			else
			{
				result = (Disease.HalfLifeToGrowthRate(this.overPopulationHalfLife, dt) - 1f) * (float)disease_count;
			}
			return result;
		}

		// Token: 0x0400622C RID: 25132
		public float underPopulationDeathRate;

		// Token: 0x0400622D RID: 25133
		public float populationHalfLife;

		// Token: 0x0400622E RID: 25134
		public float overPopulationHalfLife;

		// Token: 0x0400622F RID: 25135
		public float diffusionScale;

		// Token: 0x04006230 RID: 25136
		public float minCountPerKG;

		// Token: 0x04006231 RID: 25137
		public float maxCountPerKG;

		// Token: 0x04006232 RID: 25138
		public int minDiffusionCount;

		// Token: 0x04006233 RID: 25139
		public byte minDiffusionInfestationTickCount;
	}
}
