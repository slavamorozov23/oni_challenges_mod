using System;
using System.Collections.Generic;

namespace Database
{
	// Token: 0x02000F1F RID: 3871
	public class ArtifactDropRate : Resource
	{
		// Token: 0x06007C0E RID: 31758 RVA: 0x003028DE File Offset: 0x00300ADE
		public void AddItem(ArtifactTier tier, float weight)
		{
			this.rates.Add(new global::Tuple<ArtifactTier, float>(tier, weight));
			this.totalWeight += weight;
		}

		// Token: 0x06007C0F RID: 31759 RVA: 0x00302900 File Offset: 0x00300B00
		public float GetTierWeight(ArtifactTier tier)
		{
			float result = 0f;
			foreach (global::Tuple<ArtifactTier, float> tuple in this.rates)
			{
				if (tuple.first == tier)
				{
					result = tuple.second;
				}
			}
			return result;
		}

		// Token: 0x04005694 RID: 22164
		public List<global::Tuple<ArtifactTier, float>> rates = new List<global::Tuple<ArtifactTier, float>>();

		// Token: 0x04005695 RID: 22165
		public float totalWeight;
	}
}
