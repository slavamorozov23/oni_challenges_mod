using System;
using Klei.AI.DiseaseGrowthRules;

namespace Klei.AI
{
	// Token: 0x0200102D RID: 4141
	public class RadiationPoisoning : Disease
	{
		// Token: 0x0600808D RID: 32909 RVA: 0x00339F68 File Offset: 0x00338168
		public RadiationPoisoning(bool statsOnly) : base("RadiationSickness", 100f, Disease.RangeInfo.Idempotent(), Disease.RangeInfo.Idempotent(), Disease.RangeInfo.Idempotent(), Disease.RangeInfo.Idempotent(), 0f, statsOnly)
		{
		}

		// Token: 0x0600808E RID: 32910 RVA: 0x00339FA0 File Offset: 0x003381A0
		protected override void PopulateElemGrowthInfo()
		{
			base.InitializeElemGrowthArray(ref this.elemGrowthInfo, Disease.DEFAULT_GROWTH_INFO);
			base.AddGrowthRule(new GrowthRule
			{
				underPopulationDeathRate = new float?(0f),
				minCountPerKG = new float?(0f),
				populationHalfLife = new float?(600f),
				maxCountPerKG = new float?(float.PositiveInfinity),
				overPopulationHalfLife = new float?(600f),
				minDiffusionCount = new int?(10000),
				diffusionScale = new float?(0f),
				minDiffusionInfestationTickCount = new byte?((byte)1)
			});
			base.InitializeElemExposureArray(ref this.elemExposureInfo, Disease.DEFAULT_EXPOSURE_INFO);
		}

		// Token: 0x04006164 RID: 24932
		public const string ID = "RadiationSickness";
	}
}
