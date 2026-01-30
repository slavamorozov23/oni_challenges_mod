using System;
using Klei.AI.DiseaseGrowthRules;

namespace Klei.AI
{
	// Token: 0x0200102C RID: 4140
	public class PollenGerms : Disease
	{
		// Token: 0x0600808B RID: 32907 RVA: 0x00339C9C File Offset: 0x00337E9C
		public PollenGerms(bool statsOnly) : base("PollenGerms", 5f, new Disease.RangeInfo(263.15f, 273.15f, 363.15f, 373.15f), new Disease.RangeInfo(10f, 100f, 100f, 10f), new Disease.RangeInfo(0f, 0f, 1000f, 1000f), Disease.RangeInfo.Idempotent(), 0f, statsOnly)
		{
		}

		// Token: 0x0600808C RID: 32908 RVA: 0x00339D10 File Offset: 0x00337F10
		protected override void PopulateElemGrowthInfo()
		{
			base.InitializeElemGrowthArray(ref this.elemGrowthInfo, Disease.DEFAULT_GROWTH_INFO);
			base.AddGrowthRule(new GrowthRule
			{
				underPopulationDeathRate = new float?(0.6666667f),
				minCountPerKG = new float?(0.4f),
				populationHalfLife = new float?(3000f),
				maxCountPerKG = new float?((float)500),
				overPopulationHalfLife = new float?(10f),
				minDiffusionCount = new int?(3000),
				diffusionScale = new float?(0.001f),
				minDiffusionInfestationTickCount = new byte?((byte)1)
			});
			base.AddGrowthRule(new StateGrowthRule(Element.State.Solid)
			{
				minCountPerKG = new float?(0.4f),
				populationHalfLife = new float?(10f),
				overPopulationHalfLife = new float?(10f),
				diffusionScale = new float?(1E-06f),
				minDiffusionCount = new int?(1000000)
			});
			base.AddGrowthRule(new StateGrowthRule(Element.State.Gas)
			{
				minCountPerKG = new float?(500f),
				underPopulationDeathRate = new float?(2.6666667f),
				populationHalfLife = new float?(10f),
				overPopulationHalfLife = new float?(10f),
				maxCountPerKG = new float?((float)1000000),
				minDiffusionCount = new int?(1000),
				diffusionScale = new float?(0.015f)
			});
			base.AddGrowthRule(new ElementGrowthRule(SimHashes.Oxygen)
			{
				populationHalfLife = new float?(200f),
				overPopulationHalfLife = new float?(10f)
			});
			base.AddGrowthRule(new StateGrowthRule(Element.State.Liquid)
			{
				minCountPerKG = new float?(0.4f),
				populationHalfLife = new float?(10f),
				overPopulationHalfLife = new float?(10f),
				maxCountPerKG = new float?((float)100),
				diffusionScale = new float?(0.01f)
			});
			base.InitializeElemExposureArray(ref this.elemExposureInfo, Disease.DEFAULT_EXPOSURE_INFO);
			base.AddExposureRule(new ExposureRule
			{
				populationHalfLife = new float?(1200f)
			});
			base.AddExposureRule(new ElementExposureRule(SimHashes.Oxygen)
			{
				populationHalfLife = new float?(float.PositiveInfinity)
			});
		}

		// Token: 0x04006163 RID: 24931
		public const string ID = "PollenGerms";
	}
}
